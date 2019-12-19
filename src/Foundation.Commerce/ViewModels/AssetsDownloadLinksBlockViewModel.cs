using EPiServer.Core;
using Foundation.Commerce.Models.Blocks;
using System.Collections.Generic;

namespace Foundation.Commerce.ViewModels
{
    public class AssetsDownloadLinksBlockViewModel
    {
        public AssetsDownloadLinksBlock CurrentBlock { get; set; }
        public IEnumerable<MediaData> Assets { get; set; }

        public AssetsDownloadLinksBlockViewModel(AssetsDownloadLinksBlock currentBlock) => CurrentBlock = currentBlock;
    }
}
