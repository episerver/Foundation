// CMS 13: IBlobFactory removed from EPiServer.Framework. BlobJob is fully stubbed.
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using System.IO;
using System.Text;

namespace Foundation.Infrastructure.Jobs
{
    // CMS 13: SortIndex removed from ScheduledPlugInAttribute.
    [ScheduledPlugIn(DisplayName = "Convert File Blobs", Description = "Converts all file blobs into the currently configured blob type")]
    [ServiceConfiguration]
    public class BlobJob : ScheduledJobBase
    {
        // CMS 13: IBlobFactory removed. BlobJob is disabled; property removed.
        private int _count;
        private int _failCount;
        private readonly StringBuilder _errorText = new StringBuilder();

        public BlobJob()
        {
            IsStoppable = false;
        }

        public override string Execute()
        {
            // CMS 13: FileBlobProvider constructor changed (no longer parameterless).
            // BlobJob is disabled until FileBlobProvider DI is resolved.
            return "BlobJob disabled: FileBlobProvider constructor changed in CMS 13. Re-enable when FileBlobProvider is available via DI.";
        }

        public void ProcessFile(string path, string directory)
        {
            // CMS 13: FileBlobProvider constructor changed. ProcessFile disabled.
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