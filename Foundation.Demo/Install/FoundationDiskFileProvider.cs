using Mediachase.BusinessFoundation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace Foundation.Demo.Install
{
    public class FoundationDiskFileProvider : FileProvider
    {
        private const string KeySplitter = "::";

        private bool _searchSubdirectories;
        private static Dictionary<string, List<FoundationDiskFileDescriptor>> _cache = new Dictionary<string, List<FoundationDiskFileDescriptor>>();

        public string ModulesDirectoryPath { get; set; }

        public string SearchDirectoryName { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

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

            string searchSubdirectories = GetTrimmedConfigurationString(config, "searchSubdirectories");
            if (!string.IsNullOrEmpty(searchSubdirectories))
                _searchSubdirectories = bool.Parse(searchSubdirectories);
        }

        public override FileDescriptor[] GetFiles(string structureName, string searchPattern, Selector[] selectors)
        {
            // Collect all files in given directories conforming to search pattern and selectors.
            List<FoundationDiskFileDescriptor> list;

            string cacheKey = BuildCacheKey(selectors, ModulesDirectoryPath, structureName, searchPattern);
            lock (_cache)
            {
                if (_cache.ContainsKey(cacheKey))
                    list = _cache[cacheKey];
                else
                {
                    list = new List<FoundationDiskFileDescriptor>();

                    Dictionary<string, int> moduleOrderByName = new Dictionary<string, int>();
                    int index = 0;
                    foreach (string name in FileResolver.GetOrderedModules())
                    {
                        moduleOrderByName.Add(name.ToUpperInvariant(), index++);
                    }

                    if (Directory.Exists(ModulesDirectoryPath))
                    {
                        foreach (string modulePath in Directory.GetDirectories(ModulesDirectoryPath))
                        {
                            string searchPath = string.Concat(modulePath, Path.DirectorySeparatorChar, SearchDirectoryName, Path.DirectorySeparatorChar, structureName);
                            if (Directory.Exists(searchPath))
                            {
                                foreach (string filePath in Directory.GetFiles(searchPath, searchPattern, _searchSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                                {
                                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                                    if (selectors == null || ValidateFileName(fileNameWithoutExtension, selectors))
                                    {
                                        string moduleName = Path.GetFileName(modulePath);

                                        var descriptor = new FoundationDiskFileDescriptor(this);
                                        descriptor.ModuleName = moduleName;
                                        descriptor.FileNameWithoutExtension = fileNameWithoutExtension;
                                        descriptor.FilePath = filePath;

                                        int moduleOrder = int.MaxValue;

                                        string moduleKey = moduleName.ToUpperInvariant();
                                        if (moduleOrderByName.ContainsKey(moduleKey))
                                            moduleOrder = moduleOrderByName[moduleKey];

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
            string value = config[name];
            if (value != null)
            {
                value = value.Trim();
            }
            return value;
        }
    }
}
