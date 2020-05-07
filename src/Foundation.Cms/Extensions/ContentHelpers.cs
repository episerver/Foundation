using EPiServer.Core;

namespace Foundation.Cms.Extensions
{
    public static class ContentHelpers
    {
        public static string GetFileSize(MediaData media)
        {
            if (media != null)
            {
                using (var stream = media.BinaryData.OpenRead())
                {
                    return $"{stream.Length / 1024} kB";
                }
            }
            return string.Empty;
        }
    }
}