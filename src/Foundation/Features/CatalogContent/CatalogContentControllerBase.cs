using EPiServer.Cms.Shell;
using Foundation.Infrastructure.Commerce.Customer.Services;

namespace Foundation.Features.CatalogContent
{
    public class CatalogContentControllerBase<T> : ContentController<T> where T : CatalogContentBase
    {
        protected readonly ReferenceConverter _referenceConverter;
        protected readonly IContentLoader _contentLoader;
        protected readonly UrlResolver _urlResolver;
        //protected readonly IReviewService _reviewService;
        //protected readonly IReviewActivityService _reviewActivityService;
        protected readonly ILoyaltyService _loyaltyService;

        public CatalogContentControllerBase(ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            //IReviewService reviewService,
            //IReviewActivityService reviewActivityService,
            ILoyaltyService loyaltyService)
        {
            _referenceConverter = referenceConverter;
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            //_reviewService = reviewService;
            //_reviewActivityService = reviewActivityService;
            _loyaltyService = loyaltyService;
        }

        protected List<KeyValuePair<string, string>> GetBreadCrumb(string catalogCode)
        {
            var model = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Home", "/")
            };
            var entryLink = _referenceConverter.GetContentLink(catalogCode);
            if (entryLink != null)
            {
                var entry = _contentLoader.Get<CatalogContentBase>(entryLink);
                var product = entry;
                if (entry is VariationContent)
                {
                    var parentLink = (entry as VariationContent).GetParentProducts().FirstOrDefault();
                    if (!ContentReference.IsNullOrEmpty(parentLink))
                    {
                        product = _contentLoader.Get<CatalogContentBase>(parentLink);
                    }
                }
                var ancestors = _contentLoader.GetAncestors(product.ContentLink);
                foreach (var anc in ancestors.Reverse())
                {
                    if (anc is NodeContent)
                    {
                        model.Add(new KeyValuePair<string, string>(anc.Name, anc.PublicUrl(_urlResolver)));
                    }
                }
            }

            return model;
        }

        // AddInfomationViewModel removed: EPiServer.Personalization.Commerce (ICommerceTrackingService) has no CMS 13 version.
    }
}
