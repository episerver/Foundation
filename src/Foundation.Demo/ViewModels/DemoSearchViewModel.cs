using EPiServer.Core;
using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Find.Commerce.ViewModels;
using System.Collections.Generic;

namespace Foundation.Demo.ViewModels
{
    public class DemoSearchViewModel<T> : CommerceSearchViewModel<T> where T : IContent
    {
        public DemoSearchViewModel()
        {

        }

        public DemoSearchViewModel(T currentContent) : base(currentContent)
        {

        }

        public bool ShowProductSearchResults { get; set; }
        public bool ShowContentSearchResults { get; set; }
        public bool ShowPdfSearchResults { get; set; }
        public IEnumerable<Recommendation> Recommendations { get; set; }

    }
}
