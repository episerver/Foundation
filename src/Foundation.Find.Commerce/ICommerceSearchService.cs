using EPiServer.Core;
using EPiServer.Find.Api.Querying;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;
using Foundation.Find.Commerce.ViewModels;
using System.Collections.Generic;

namespace Foundation.Find.Commerce
{
    public interface ICommerceSearchService
    {
        /// <summary>
        /// Search products
        /// </summary>
        /// <param name="currentContent"></param>
        /// <param name="filterOptions"></param>
        /// <param name="selectedFacets"></param>
        /// <returns></returns>
        ProductSearchResults Search(IContent currentContent, CommerceFilterOptionViewModel filterOptions, string selectedFacets, int catalogId = 0);
        /// <summary>
        /// Search products with filter
        /// </summary>
        /// <param name="currentContent"></param>
        /// <param name="filterOptions"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        ProductSearchResults SearchWithFilters(IContent currentContent, CommerceFilterOptionViewModel filterOptions, IEnumerable<Filter> filters, int catalogId = 0);
        /// <summary>
        /// Search all products on sale
        /// </summary>
        /// <param name="currentContent"></param>
        /// <returns></returns>
        IEnumerable<ProductTileViewModel> SearchOnSale(SalesPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12);
        /// <summary>
        /// Search top products by sorted by the creation date
        /// </summary>
        /// <param name="currentContent"></param>
        /// <returns></returns>
        IEnumerable<ProductTileViewModel> SearchNewProducts(NewProductsPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12);

        /// <summary>
        /// QuickSearch products
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<ProductTileViewModel> QuickSearch(string query, int catalogId = 0);
        /// <summary>
        /// Quicksearch products
        /// </summary>
        /// <param name="filterOptions"></param>
        /// <returns></returns>
        IEnumerable<ProductTileViewModel> QuickSearch(CommerceFilterOptionViewModel filterOptions, int catalogId = 0);


        IEnumerable<SortOrder> GetSortOrder();
        string GetOutline(string nodeCode);

        IEnumerable<UserSearchResultModel> SearchUsers(string query, int page = 1, int pageSize = 50);
        IEnumerable<SkuSearchResultModel> SearchSkus(string query);
    }
}
