using System;

namespace Foundation.Demo.ViewModels
{
    public class AzureBlob
    {
        public string Name { get; set; }
        public DateTime LastModified { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public string BlobType { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public bool IsContainer { get; set; }
        public bool IsDirectory { get; set; }
        public bool IsBlob { get; set; }
        public string Etag { get; set; }

        public string SizeString
        {
            get
            {
                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                var order = 0;
                while (Size >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    Size = Size / 1024;
                }

                // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
                // show a single decimal place, and no space.
                return string.Format("{0:0.##} {1}", Size, sizes[order]);
            }
        }
    }
}