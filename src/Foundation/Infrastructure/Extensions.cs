using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Find;
//using EPiServer.Find.Commerce;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Features.Folder;
using Foundation.Features.Home;
using Foundation.Features.Locations.LocationItemPage;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization;
using Foundation.Features.Search;
using Foundation.Infrastructure.Commerce.Customer;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Foundation.Infrastructure.Find;

namespace Foundation.Infrastructure
{
    public static class Extensions
    {
        private static readonly Lazy<IContentRepository> _contentRepository =
            new Lazy<IContentRepository>(() => ServiceLocator.Current.GetInstance<IContentRepository>());

        private static readonly Lazy<IRelationRepository> RelationRepository =
          new Lazy<IRelationRepository>(() => ServiceLocator.Current.GetInstance<IRelationRepository>());

        public static FilterBuilder<T> FilterOutline<T>(this FilterBuilder<T> filterBuilder,
           IEnumerable<string> value)
        {
            var outlineFilterBuilder = new FilterBuilder<ContentData>(filterBuilder.Client);
            outlineFilterBuilder = outlineFilterBuilder.And(x => !x.MatchTypeHierarchy(typeof(EntryContentBase)));
            outlineFilterBuilder = value.Aggregate(outlineFilterBuilder,
                (current, filter) => current.Or(x => ((EntryContentBase)x).Outline().PrefixCaseInsensitive(filter)));
            return filterBuilder.And(x => outlineFilterBuilder);
        }

        public static ITypeSearch<T> FilterOutline<T>(this ITypeSearch<T> search, IEnumerable<string> value)
        {
            var filterBuilder = new FilterBuilder<T>(search.Client)
                .FilterOutline(value);

            return search.Filter(x => filterBuilder);
        }

        public static List<string> AvailableSizes(this GenericProduct genericProduct)
        {
            return genericProduct.ContentLink.GetAllVariants<GenericVariant>()
                .Select(x => x.Size)
                .Distinct()
                .ToList();
        }

        public static List<string> AvailableColors(this GenericProduct genericProduct)
        {
            return genericProduct.ContentLink.GetAllVariants<GenericVariant>()
                .Select(x => x.Color)
                .Distinct()
                .ToList();
        }

        public static IEnumerable<VariationModel> VariationModels(this ProductContent productContent)
        {
            return _contentRepository.Value
                .GetItems(productContent.GetVariants(RelationRepository.Value), productContent.Language)
                .OfType<VariationContent>()
                .Select(x => new VariationModel
                {
                    Code = x.Code,
                    LanguageId = productContent.Language.Name,
                    Name = x.DisplayName,
                    DefaultAssetUrl = ""//(x as IAssetContainer).DefaultImageUrl()
                });
        }

        public static ContentReference GetRelativeStartPage(this IContent content)
        {
            if (content is HomePage)
            {
                return content.ContentLink;
            }

            var ancestors = _contentRepository.Value.GetAncestors(content.ContentLink);
            var startPage = ancestors.FirstOrDefault(x => x is HomePage) as HomePage;
            return startPage == null ? ContentReference.StartPage : startPage.ContentLink;
        }

        public static bool IsEqual(this AddressModel address,
           AddressModel compareAddressViewModel)
        {
            return address.FirstName == compareAddressViewModel.FirstName &&
                   address.LastName == compareAddressViewModel.LastName &&
                   address.Line1 == compareAddressViewModel.Line1 &&
                   address.Line2 == compareAddressViewModel.Line2 &&
                   address.Organization == compareAddressViewModel.Organization &&
                   address.PostalCode == compareAddressViewModel.PostalCode &&
                   address.City == compareAddressViewModel.City &&
                   address.CountryCode == compareAddressViewModel.CountryCode &&
                   address.CountryRegion.Region == compareAddressViewModel.CountryRegion.Region;
        }

        public static List<string> TagString(this LocationItemPage locationList) => new List<string>();// locationList.Categories.Select(cai => _contentRepository.Value.Get<StandardCategory>(cai).Name).ToList();

        public static ContactViewModel GetCurrentContactViewModel(this ICustomerService customerService)
        {
            var currentContact = customerService.GetCurrentContact();
            return currentContact?.Contact != null ? new ContactViewModel(currentContact) : new ContactViewModel();
        }

        public static ContactViewModel GetContactViewModelById(this ICustomerService customerService, string id) => new ContactViewModel(customerService.GetContactById(id));

        public static List<ContactViewModel> GetContactViewModelsForOrganization(this ICustomerService customerService, FoundationOrganization organization = null)
        {
            if (organization == null)
            {
                organization = GetCurrentOrganization(customerService);
            }

            if (organization == null)
            {
                return new List<ContactViewModel>();
            }

            var organizationUsers = customerService.GetContactsForOrganization(organization);

            if (organization.SubOrganizations.Count > 0)
            {
                foreach (var subOrg in organization.SubOrganizations)
                {
                    var contacts = customerService.GetContactsForOrganization(subOrg);
                    organizationUsers.AddRange(contacts);
                }
            }

            return organizationUsers.Select(user => new ContactViewModel(user)).ToList();
        }

        public static IEnumerable<T> FindPagesRecursively<T>(this IContentLoader contentLoader, PageReference pageLink) where T : PageData
        {
            foreach (var child in contentLoader.GetChildren<T>(pageLink))
            {
                yield return child;
            }

            foreach (var folder in contentLoader.GetChildren<FolderPage>(pageLink))
            {
                foreach (var nestedChild in contentLoader.FindPagesRecursively<T>(folder.PageLink))
                {
                    yield return nestedChild;
                }
            }
        }

        public static bool IsVirtualVariant(this ILineItem lineItem)
        {
            var entry = lineItem.GetEntryContent<EntryContentBase>() as GenericVariant;
            return entry != null && entry.VirtualProductMode != null && !string.IsNullOrWhiteSpace(entry.VirtualProductMode) && !entry.VirtualProductMode.Equals("None");
        }

        public static bool IsAjaxRequest(this HttpRequest httpRequest) => httpRequest.Headers["X-Requested-With"] == "XMLHttpRequest";

        private static FoundationOrganization GetCurrentOrganization(ICustomerService customerService)
        {
            var contact = customerService.GetCurrentContact();
            if (contact != null)
            {
                return contact.FoundationOrganization;
            }

            return null;
        }
    }
}