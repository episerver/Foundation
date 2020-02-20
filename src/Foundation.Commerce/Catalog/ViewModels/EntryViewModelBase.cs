using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Cms.ViewModels;
using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Commerce.Catalog.ViewModels
{
    public abstract class EntryViewModelBase<T> : ContentViewModel<T> where T : EntryContentBase
    {
        protected EntryViewModelBase()
        {

        }

        protected EntryViewModelBase(T currentContent) : base(currentContent)
        {

        }

        public Injected<UrlResolver> UrlResolver { get; set; }
        public IList<string> Images { get; set; }
        public StoreViewModel Stores { get; set; }
        public IEnumerable<ProductTileViewModel> StaticAssociations { get; set; }
        public bool HasOrganization { get; set; }
        public List<string> ReturnedMessages { get; set; }
        public Money? DiscountedPrice { get; set; }
        public Money ListingPrice { get; set; }
        public Money? SubscriptionPrice { get; set; }
        public Money? MsrpPrice { get; set; }
        public Money? MapPrice { get; set; }
        public bool IsAvailable { get; set; }
        public bool ShowRecommendations { get; set; }
        public bool IsSalesRep { get; set; }
        public List<MediaData> SalesMaterials { get; set; }
        public List<MediaData> Documents { get; set; }
        public List<KeyValuePair<string, string>> BreadCrumb { get; set; }
        public int MinQuantity { get; set; }
        public bool HasSaleCode { get; set; }
    }
}