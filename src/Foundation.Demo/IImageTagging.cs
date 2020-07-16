using EPiServer.Framework.Blobs;
using System.Collections.Generic;

namespace Foundation.Demo
{
    public interface IImageTagging
    {
        string FileSize { get; set; }
        string AccentColor { get; set; }
        string Caption { get; set; }
        string ClipArtType { get; set; }
        string DominantColorBackground { get; set; }
        string DominantColorForeground { get; set; }
        IList<string> DominantColors { get; set; }
        IList<string> ImageCategories { get; set; }
        bool IsAdultContent { get; set; }
        bool IsBwImg { get; set; }
        bool IsRacyContent { get; set; }
        string LineDrawingType { get; set; }
        IList<string> Tags { get; set; }
        Blob BinaryData { get; set; }
    }
}
