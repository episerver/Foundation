using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Framework.Localization;
using EPiServer.Web.Routing;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels.Header;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Order.Services;
using Foundation.Commerce.ViewModels.Header;
using Foundation.Demo.Models;
using Mediachase.Commerce;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Markets;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Demo.ViewModels
{
    public class DemoHeaderViewModelFactory : CommerceHeaderViewModelFactory
    {
        private readonly ICustomerService _customerService;
        private readonly CustomerContext _customerContext;
        private readonly CmsHeaderViewModelFactory _cmsHeaderViewModelFactory;

        public DemoHeaderViewModelFactory(LocalizationService localizationService,
            ICustomerService customerService,
            CartViewModelFactory cartViewModelFactory,
            IUrlResolver urlResolver,
            IMarketService marketService,
            ICurrentMarket currentMarket,
            IBookmarksService bookmarksService,
            ICartService cartService,
            CustomerContext customerContext,
            IContentCacheKeyCreator contentCacheKeyCreator,
            IContentLoader contentLoader,
            CmsHeaderViewModelFactory cmsHeaderViewModelFactory,
            IDatabaseMode databaseMode) :
            base(localizationService,
                customerService,
                cartViewModelFactory,
                urlResolver,
                marketService,
                currentMarket,
                bookmarksService,
                cartService,
                contentCacheKeyCreator,
                contentLoader,
                databaseMode)
        {
            _customerService = customerService;
            _customerContext = customerContext;
            _cmsHeaderViewModelFactory = cmsHeaderViewModelFactory;
        }

        public override THeaderViewModel CreateHeaderViewModel<THeaderViewModel>(IContent currentContent, CmsHomePage homePage)
        {
            var demoHomePage = homePage as DemoHomePage;
            if (demoHomePage == null)
            {
                return null;
            }
            var contact = _customerService.GetCurrentContact();
            var isBookmarked = IsBookmarked(currentContent);
            var viewModel = CreateViewModel<DemoHeaderViewModel>(currentContent, demoHomePage, contact, isBookmarked);
            AddCommerceComponents(contact, viewModel);
            AddAnonymousComponents(demoHomePage, viewModel);
            AddMarketViewModel(currentContent, viewModel);
            AddMyAccountMenu(demoHomePage, viewModel);
            viewModel.LargeHeaderMenu = demoHomePage.LargeHeaderMenu;
            viewModel.ShowCommerceControls = demoHomePage.ShowCommerceHeaderComponents;
            viewModel.DemoUsers = GetDemoUsers(demoHomePage.ShowCommerceHeaderComponents);
            return viewModel as THeaderViewModel;
        }

        public override void AddMyAccountMenu<THomePage, THeaderViewModel>(THomePage homePage, THeaderViewModel viewModel)
        {
            var demoHomePage = homePage as DemoHomePage;
            if (demoHomePage == null)
            {
                return;
            }

            if (demoHomePage.ShowCommerceHeaderComponents)
            {
                base.AddMyAccountMenu(homePage, viewModel);
                return;
            }

            _cmsHeaderViewModelFactory.AddMyAccountMenu(homePage, viewModel);
        }

        private List<DemoUserViewModel> GetDemoUsers(bool showCommerceUsers)
        {
            return _customerContext.GetContacts(0, 1000)
                .Select(_ => new FoundationContact(_))
                .Where(_ => showCommerceUsers ? _.ShowInDemoUserMenu > 1 : _.ShowInDemoUserMenu == 2)
                .Select(_ => new DemoUserViewModel
                {
                    Description = _.DemoUserDescription,
                    Title = _.DemoUserTitle,
                    Id = _.ContactId,
                    Email = _.Email,
                    FullName = _.FullName,
                    SortOrder = _.DemoSortOrder
                })
                .OrderBy(_ => _.SortOrder)
                .ToList();
        }
    }
}
