using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Tracking.Commerce.Data;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer.Services;
using Foundation.Personalization;
using Foundation.Social.Models.ActivityStreams;
using Foundation.Social.Services;
using Foundation.Social.ViewModels;
using Mediachase.Commerce.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.CatalogContent
{
    public class CatalogContentControllerBase<T> : ContentController<T> where T : CatalogContentBase
    {
        protected readonly ReferenceConverter _referenceConverter;
        protected readonly IContentLoader _contentLoader;
        protected readonly UrlResolver _urlResolver;
        protected readonly IReviewService _reviewService;
        protected readonly IReviewActivityService _reviewActivityService;
        protected readonly ICommerceTrackingService _recommendationService;
        protected readonly ILoyaltyService _loyaltyService;

        public CatalogContentControllerBase(ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            IReviewService reviewService,
            IReviewActivityService reviewActivityService,
            ICommerceTrackingService recommendationService,
            ILoyaltyService loyaltyService)
        {
            _referenceConverter = referenceConverter;
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _reviewService = reviewService;
            _reviewActivityService = reviewActivityService;
            _recommendationService = recommendationService;
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

        protected void AddActivity(string product,
            int rating,
            string user)
        {
            // Create the review activity
            var activity = new ReviewActivity
            {
                Product = product,
                Rating = rating,
                Contributor = user,
            };

            // Add the review activity 
            _reviewActivityService.Add(user, product, activity);
        }

        protected ReviewsViewModel GetReviews(string productCode) =>

            //Testing to query FIND with GetRatingAverage
            //var searchClient = Client.CreateFromConfig();
            //var contentResult = searchClient.Search<FashionProduct>()
            //                .Filter(c => c.GetRatingAverage().GreaterThan(0))
            //                .OrderByDescending(c => c.GetRatingAverage()).Take(25)
            //                .GetContentResult();

            // Return reviews for the product with the ReviewService
            _reviewService.Get(productCode);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAReview(ReviewSubmissionViewModel reviewForm)
        {
            // Invoke the ReviewService to add the submission
            try
            {
                var model = _reviewService.Add(reviewForm);
                //Loyalty Program: Add Points and Number Of Reviews
                _loyaltyService.AddNumberOfReviews();
                AddActivity(reviewForm.ProductCode, reviewForm.Rating, reviewForm.Nickname);
                return PartialView("_ReviewItem", model);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        protected async Task AddInfomationViewModel(IEntryViewModelBase viewModel, string productCode, bool skipTracking)
        {
            viewModel.Reviews = GetReviews(productCode);
            var trackingResponse = new TrackingResponseData();
            if (!skipTracking)
            {
                trackingResponse = await _recommendationService.TrackProduct(HttpContext, productCode, false);
            }
            viewModel.AlternativeProducts = trackingResponse.GetAlternativeProductsRecommendations(_referenceConverter);
            viewModel.CrossSellProducts = trackingResponse.GetCrossSellProductsRecommendations(_referenceConverter);
        }
    }
}