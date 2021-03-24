using Mediachase.BusinessFoundation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace Foundation.Commerce.Install
{
    public class FoundationDiskFileProvider : FileProvider
    {
        private bool _searchSubdirectories;
        private static readonly Dictionary<string, List<FoundationDiskFileDescriptor>> _cache = new Dictionary<string, List<FoundationDiskFileDescriptor>>();

        public string ModulesDirectoryPath { get; set; }

        public string SearchDirectoryName { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "FoundationDiskFileProvider";
            }

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Disk file provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            ModulesDirectoryPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Foundation.CommerceManager\Apps").FullName;
            SearchDirectoryName = GetTrimmedConfigurationString(config, "searchDirectoryName");

            if (string.IsNullOrEmpty(SearchDirectoryName))
            {
                SearchDirectoryName = "Config";
            }

            var searchSubdirectories = GetTrimmedConfigurationString(config, "searchSubdirectories");
            if (!string.IsNullOrEmpty(searchSubdirectories))
            {
                _searchSubdirectories = bool.Parse(searchSubdirectories);
            }
        }

        public override FileDescriptor[] GetFiles(string structureName, string searchPattern, Selector[] selectors)
        {
            // Collect all files in given directories conforming to search pattern and selectors.
            List<FoundationDiskFileDescriptor> list;

            var cacheKey = BuildCacheKey(selectors, ModulesDirectoryPath, structureName, searchPattern);
            lock (_cache)
            {
                if (_cache.ContainsKey(cacheKey))
                {
                    list = _cache[cacheKey];
                }
                else
                {
                    list = new List<FoundationDiskFileDescriptor>();

                    var moduleOrderByName = new Dictionary<string, int>();
                    var index = 0;
                    foreach (var name in FileResolver.GetOrderedModules())
                    {
                        moduleOrderByName.Add(name.ToUpperInvariant(), index++);
                    }

                    if (Directory.Exists(ModulesDirectoryPath))
                    {
                        foreach (var modulePath in Directory.GetDirectories(ModulesDirectoryPath))
                        {
                            var searchPath = string.Concat(modulePath, Path.DirectorySeparatorChar, SearchDirectoryName, Path.DirectorySeparatorChar, structureName);
                            if (Directory.Exists(searchPath))
                            {
                                foreach (var filePath in Directory.GetFiles(searchPath, searchPattern, _searchSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                                {
                                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                                    if (selectors == null || ValidateFileName(fileNameWithoutExtension, selectors))
                                    {
                                        var moduleName = Path.GetFileName(modulePath);

                                        var descriptor = new FoundationDiskFileDescriptor(this)
                                        {
                                            ModuleName = moduleName,
                                            FileNameWithoutExtension = fileNameWithoutExtension,
                                            FilePath = filePath
                                        };

                                        var moduleOrder = int.MaxValue;

                                        var moduleKey = moduleName.ToUpperInvariant();
                                        if (moduleOrderByName.ContainsKey(moduleKey))
                                        {
                                            moduleOrder = moduleOrderByName[moduleKey];
                                        }

                                        descriptor.ModuleOrder = moduleOrder;
                                        descriptor.FileCreationTimeTicks = File.GetCreationTime(filePath).Ticks;

                                        list.Add(descriptor);
                                    }
                                }
                            }
                        }
                    }

                    list.Sort(FoundationDiskFileDescriptor.Compare);
                    _cache.Add(cacheKey, list);
                }
            }

            return list.ToArray();
        }

        private static string GetTrimmedConfigurationString(NameValueCollection config, string name)
        {
            var value = config[name];
            if (value != null)
            {
                value = value.Trim();
            }
            return value;
        }
    }
}
