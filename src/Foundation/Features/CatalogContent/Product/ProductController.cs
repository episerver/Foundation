﻿using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Models.Catalog;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Personalization;
using Foundation.Demo.ViewModels;
using Foundation.Social.Services;
using Mediachase.Commerce.Catalog;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.CatalogContent.Product
{
    public class ProductController : CatalogContentControllerBase<GenericProduct>
    {
        private readonly bool _isInEditMode;
        private readonly CatalogEntryViewModelFactory _viewModelFactory;

        public ProductController(IsInEditModeAccessor isInEditModeAccessor,
            CatalogEntryViewModelFactory viewModelFactory,
            IReviewService reviewService,
            IReviewActivityService reviewActivityService,
            ICommerceTrackingService recommendationService,
            ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            ILoyaltyService loyaltyService) : base(referenceConverter, contentLoader, urlResolver, reviewService, reviewActivityService, recommendationService, loyaltyService)
        {
            _isInEditMode = isInEditModeAccessor();
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<ActionResult> Index(GenericProduct currentContent, string variationCode = "", bool skipTracking = false)
        {
            var viewModel = _viewModelFactory.Create<GenericProduct, GenericVariant, DemoGenericProductViewModel>(currentContent, variationCode);

            if (_isInEditMode && viewModel.Variant == null)
            {
                var emptyViewName = "ProductWithoutEntries";
                return Request.IsAjaxRequest() ? (ViewResultBase)PartialView(emptyViewName, viewModel) : View(emptyViewName, viewModel);
            }

            if (viewModel.Variant == null)
            {
                return HttpNotFound();
            }

            await AddInfomationViewModel(viewModel, currentContent.Code, skipTracking);

            var startPage = _contentLoader.Get<PageData>(ContentReference.StartPage) as CommerceHomePage;
            currentContent.AddBrowseHistory();
            viewModel.BreadCrumb = GetBreadCrumb(currentContent.Code);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult QuickView(string productCode, string variantCode)
        {
            var currentContentRef = _referenceConverter.GetContentLink(productCode);
            var currentContent = _contentLoader.Get<ProductContent>(currentContentRef) as GenericProduct;
            if (currentContent != null)
            {
                var viewModel = _viewModelFactory.Create<GenericProduct, GenericVariant, GenericProductViewModel>(currentContent, variantCode);
                return PartialView("_QuickView", viewModel);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product not found.");
        }

        [HttpGet]
        public ActionResult SelectVariant(string productCode, string color, string size, bool isQuickView = true)
        {
            var currentContentRef = _referenceConverter.GetContentLink(productCode);
            var currentContent = _contentLoader.Get<ProductContent>(currentContentRef) as GenericProduct;
            if (currentContent != null)
            {
                var variant = _viewModelFactory.SelectVariant(currentContent, color, size);
                if (variant != null)
                {
                    var viewModel = _viewModelFactory.Create<GenericProduct, GenericVariant, DemoGenericProductViewModel>(currentContent, variant.Code);

                    if (isQuickView)
                    {
                        return PartialView("_QuickView", viewModel);
                    }
                    else
                    {
                        return PartialView("_ProductDetail", viewModel);
                    }
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product not found.");
        }
    }
}