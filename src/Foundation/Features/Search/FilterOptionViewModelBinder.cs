using Foundation.Infrastructure.Find.Facets;
using Mediachase.Search;
using Mediachase.Search.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Foundation.Features.Search
{
    public class FilterOptionViewModelBinder : IModelBinder
    {
        private readonly IContentLoader _contentLoader;
        private readonly LocalizationService _localizationService;
        private readonly IContentLanguageAccessor _contentLanguageAccessor;
        private readonly IFacetRegistry _facetRegistry;

        public FilterOptionViewModelBinder(IContentLoader contentLoader,
            LocalizationService localizationService,
            IContentLanguageAccessor contentLanguageAccessor,
            IFacetRegistry facetRegistry)
        {
            _contentLoader = contentLoader;
            _localizationService = localizationService;
            _contentLanguageAccessor = contentLanguageAccessor;
            _facetRegistry = facetRegistry;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var model = new FilterOptionViewModel();
            var contentLink = bindingContext.ActionContext.HttpContext.GetContentLink();
            IContent content = null;
            if (!ContentReference.IsNullOrEmpty(contentLink))
            {
                content = _contentLoader.Get<IContent>(contentLink);
            }

            var query = bindingContext.ActionContext.HttpContext.Request.Query["search"];
            var sort = bindingContext.ActionContext.HttpContext.Request.Query["sort"];
            var facets = bindingContext.ActionContext.HttpContext.Request.Query["facets"];
            var section = bindingContext.ActionContext.HttpContext.Request.Query["t"];
            var page = bindingContext.ActionContext.HttpContext.Request.Query["page"];
            var pageSize = bindingContext.ActionContext.HttpContext.Request.Query["pageSize"];
            var confidence = bindingContext.ActionContext.HttpContext.Request.Query["confidence"];
            var viewMode = bindingContext.ActionContext.HttpContext.Request.Query["ViewSwitcher"];
            var sortDirection = bindingContext.ActionContext.HttpContext.Request.Query["sortDirection"];

            EnsurePage(model, page);
            EnsurePageSize(model, pageSize);
            EnsureViewMode(model, viewMode);
            EnsureQ(model, query);
            EnsureSort(model, sort);
            EnsureSortDirection(model, sortDirection);
            EnsureSection(model, section);
            EnsureFacets(model, facets, content);
            model.Confidence = decimal.TryParse(confidence, out var confidencePercentage) ? confidencePercentage : 0;
            bindingContext.Result = ModelBindingResult.Success(model);
            await Task.CompletedTask;
        }

        protected virtual void EnsurePage(FilterOptionViewModel model, string page)
        {
            if (model.Page < 1)
            {
                if (!string.IsNullOrEmpty(page))
                {
                    model.Page = int.Parse(page);
                }
                else
                {
                    model.Page = 1;
                }
            }
        }

        protected virtual void EnsurePageSize(FilterOptionViewModel model, string pageSize)
        {
            if (!string.IsNullOrEmpty(pageSize))
            {
                model.PageSize = int.Parse(pageSize);
            }
            else
            {
                model.PageSize = 10;
            }
        }

        protected virtual void EnsureViewMode(FilterOptionViewModel model, string viewMode)
        {
            if (!string.IsNullOrEmpty(viewMode))
            {
                model.ViewSwitcher = viewMode;
            }
            else
            {
                model.ViewSwitcher = "";
            }
        }

        protected virtual void EnsureQ(FilterOptionViewModel model, string q)
        {
            if (!string.IsNullOrEmpty(q))
            {
                model.Q = q;
            }
        }

        protected virtual void EnsureSection(FilterOptionViewModel model, string section)
        {
            if (!string.IsNullOrEmpty(section))
            {
                model.SectionFilter = section;
            }
        }

        protected virtual void EnsureSort(FilterOptionViewModel model, string sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                model.Sort = sort;
            }
        }
        protected virtual void EnsureSortDirection(FilterOptionViewModel model, string sortDirection)
        {
            if (!string.IsNullOrEmpty(sortDirection))
            {
                model.SortDirection = sortDirection;
            }
        }

        protected virtual void EnsureFacets(FilterOptionViewModel model, string facets, IContent content)
        {
            if (model.FacetGroups == null)
            {
                model.FacetGroups = CreateFacetGroups(facets);
            }
        }

        private List<FacetGroupOption> CreateFacetGroups(string facets)
        {
            var facetGroups = new List<FacetGroupOption>();
            if (string.IsNullOrEmpty(facets))
            {
                return facetGroups;
            }
            foreach (var facet in facets.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries))
            {
                var data = facet.Split(':');
                if (data.Length != 2)
                {
                    continue;
                }
                var searchFilter = GetSearchFilter(data[0]);
                if (searchFilter == null)
                {
                    continue;
                }
                var facetGroup = facetGroups.FirstOrDefault(fg => fg.GroupFieldName == searchFilter.FieldName);
                if (facetGroup == null)
                {
                    facetGroup = CreateFacetGroup(searchFilter);
                    facetGroups.Add(facetGroup);
                }
                var facetOption = facetGroup.Facets.FirstOrDefault(fo => fo.Name == data[1]);
                if (facetOption != null)
                {
                    continue;
                }
                facetOption = CreateFacetOption(data[1], $"{data[0]}:{data[1]}");
                facetGroup.Facets.Add(facetOption);
            }
            return facetGroups;
        }

        private FacetDefinition GetSearchFilter(string facet)
        {
            return _facetRegistry.GetFacetDefinitions().FirstOrDefault(filter =>
                filter.FieldName.Equals(facet, System.StringComparison.InvariantCultureIgnoreCase));
        }

        private FacetGroupOption CreateFacetGroup(FacetDefinition searchFilter)
        {
            return new FacetGroupOption
            {
                GroupFieldName = searchFilter.FieldName,
                GroupName = searchFilter.DisplayName,
                Facets = new List<FacetOption>()
            };
        }

        private static FacetOption CreateFacetOption(string name, string key) => new FacetOption { Name = name, Key = key, Selected = true };

        public SearchFilter GetSearchFilterForNode(NodeContent nodeContent)
        {
            var configFilter = new SearchFilter
            {
                field = BaseCatalogIndexBuilder.FieldConstants.Node,
                Descriptions = new Descriptions
                {
                    defaultLocale = _contentLanguageAccessor.Language.Name
                },
                Values = new SearchFilterValues()
            };

            var desc = new Description
            {
                locale = "en",
                Value = _localizationService.GetString("/Facet/Category")
            };
            configFilter.Descriptions.Description = new[] { desc };

            var nodes = _contentLoader.GetChildren<NodeContent>(nodeContent.ContentLink).ToList();
            var nodeValues = new SimpleValue[nodes.Count];
            var index = 0;
            var preferredCultureName = _contentLanguageAccessor.Language.Name;
            foreach (var node in nodes)
            {
                var val = new SimpleValue
                {
                    key = node.Code,
                    value = node.Code,
                    Descriptions = new Descriptions
                    {
                        defaultLocale = preferredCultureName
                    }
                };
                var desc2 = new Description
                {
                    locale = preferredCultureName,
                    Value = node.DisplayName
                };
                val.Descriptions.Description = new[] { desc2 };

                nodeValues[index] = val;
                index++;
            }
            configFilter.Values.SimpleValue = nodeValues;
            return configFilter;
        }
    }
}
