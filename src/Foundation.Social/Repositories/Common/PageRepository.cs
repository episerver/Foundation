using EPiServer;
using EPiServer.Core;
using System;

namespace Foundation.Social.Repositories.Common
{
    /// <summary>
    ///     This class encapsulates common page related operations used by the Episerver Social sample.
    /// </summary>
    public class PageRepository : IPageRepository
    {
        private readonly IContentRepository _contentRepository;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="contentRepository">an instance of the Episerver's content repository</param>
        public PageRepository(IContentRepository contentRepository) => _contentRepository = contentRepository;

        /// <summary>
        ///     Gets the page Id given its page reference.
        /// </summary>
        /// <param name="pageLink">The page reference.</param>
        /// <returns>The page Id.</returns>
        public string GetPageId(PageReference pageLink)
        {
            var pageData = _contentRepository.Get<PageData>(pageLink);
            return pageData != null ? pageData.ContentGuid.ToString() : string.Empty;
        }

        /// <summary>
        ///     Gets the name of the page that has the specified identifier
        /// </summary>
        /// <param name="pageId">the page Id</param>
        /// <returns>the name of the page</returns>
        public string GetPageName(string pageId)
        {
            var pageName = string.Empty;
            try
            {
                if (Guid.TryParse(pageId, out var pageIdGuid) && pageIdGuid != Guid.Empty)
                {
                    var data = _contentRepository.Get<PageData>(pageIdGuid);
                    pageName = data.Name;
                }
            }
            catch (ContentNotFoundException)
            {
                pageName = "[Undetermined page name with Id: " + pageId + "]";
            }

            return pageName;
        }
    }
}