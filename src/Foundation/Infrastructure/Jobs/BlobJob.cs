using EPiServer.Framework.Blobs;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using System;
using System.IO;
using System.Text;

namespace Foundation.Infrastructure.Jobs
{
    [ScheduledPlugIn(DisplayName = "Convert File Blobs", Description = "Converts all file blobs into the currently configured blob type", SortIndex = 10000)]
    public class BlobJob : ScheduledJobBase
    {
        protected Injected<IBlobFactory> BlobFactory { get; set; }
        private int _count;
        private int _failCount;
        private readonly StringBuilder _errorText = new StringBuilder();

        public BlobJob()
        {
            IsStoppable = false;
        }

        public override string Execute()
        {
            OnStatusChanged(string.Format("Starting execution of {0}", GetType()));
            ProcessDirectory(new FileBlobProvider().Path);
            var status = string.Format("Converted {0} blobs <br\\>", _count);
            if (_failCount > 0)
            {
                status = string.Format("Converting errors:{0}. Details:{1}", _failCount, _errorText);
            }
            return status;
        }

        public void ProcessFile(string path, string directory)
        {
            try
            {
                path = Path.GetFileName(path);
                directory = Path.GetFileName(directory);
                var id =
                    new Uri(string.Format("{0}://{1}/{2}/{3}", Blob.BlobUriScheme, Blob.DefaultProvider, directory, path));
                var blob = new FileBlobProvider().GetBlob(id);
                BlobFactory.Service.GetBlob(id).Write(blob.OpenRead());
                _count++;
                if (_count % 50 == 0)
                {
                    OnStatusChanged(string.Format("Converted {0} blobs.", _count));
                }
            }
            catch (Exception ex)
            {
                _failCount++;
                _errorText.AppendLine(ex.ToString());
            }
        }

        public void ProcessDirectory(string targetDirectory)
        {
            foreach (var fileName in Directory.GetFiles(targetDirectory))
            {
                ProcessFile(fileName, targetDirectory);
            }

            foreach (var subdirectory in Directory.GetDirectories(targetDirectory))
            {
                ProcessDirectory(subdirectory);
            }
        }
    }
}