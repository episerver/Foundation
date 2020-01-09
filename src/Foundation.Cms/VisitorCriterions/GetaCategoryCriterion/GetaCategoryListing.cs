using EPiServer;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Geta.EpiCategories;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Cms.VisitorCriterions.GetaCategoryCriterion
{
    public class GetaCategoryListing : ISelectionFactory
    {
        private readonly Injected<ICategoryContentLoader> _categoryContentLoader;
        private readonly Injected<IContentLoader> _contentLoader;

        public IEnumerable<SelectListItem> GetSelectListItems(Type propertyType)
        {
            var categories = _categoryContentLoader.Service.GetGlobalCategories<CategoryData>();
            var results = new List<SelectListItem>();

            foreach (var c in categories)
            {
                GetChildren(c, results, "");
            }

            return results;
        }

        private void GetChildren(CategoryData categoryData, List<SelectListItem> list, string prefix)
        {
            list.Add(new SelectListItem() { Text = prefix + categoryData.Name, Value = categoryData.ContentLink.ID.ToString() });
            var children = _contentLoader.Service.GetChildren<CategoryData>(categoryData.ContentLink);
            foreach(var c in children)
            {
                GetChildren(c, list, prefix + "-");
            }
        }
    }
}
