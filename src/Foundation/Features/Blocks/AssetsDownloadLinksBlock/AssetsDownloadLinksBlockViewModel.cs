using EPiServer.Core;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.AssetsDownloadLinksBlock
{
    public class AssetsDownloadLinksBlockViewModel
    {
        public AssetsDownloadLinksBlock CurrentBlock { get; set; }
        public IEnumerable<MediaData> Assets { get; set; }

        public AssetsDownloadLinksBlockViewModel(AssetsDownloadLinksBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }
    }
}
