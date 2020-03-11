using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using System.Web.Mvc;

namespace Foundation.Find.Cms.ViewModels
{
    public class CmsFilterOptionViewModelBinder : DefaultModelBinder
    {
        private readonly IContentLoader _contentLoader;

        public CmsFilterOptionViewModelBinder(IContentLoader contentLoader) => _contentLoader = contentLoader;

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bindingContext.ModelName = "FilterOption";
            CmsFilterOptionViewModel model = null;
            try
            {
                model = (CmsFilterOptionViewModel)base.BindModel(controllerContext, bindingContext);
            }
            catch
            {
                model = new CmsFilterOptionViewModel();
            }

            if (model == null)
            {
                return model;
            }

            var contentLink = controllerContext.RequestContext.GetContentLink();
            IContent content = null;
            if (!ContentReference.IsNullOrEmpty(contentLink))
            {
                content = _contentLoader.Get<IContent>(contentLink);
            }

            var query = controllerContext.HttpContext.Request.QueryString["search"];
            var sort = controllerContext.HttpContext.Request.QueryString["sort"];
            var section = controllerContext.HttpContext.Request.QueryString["t"];
            var contentFacet = controllerContext.HttpContext.Request.QueryString["c"];
            var page = controllerContext.HttpContext.Request.QueryString["p"];
            var confidence = controllerContext.HttpContext.Request.QueryString["confidence"];
            SetupModel(model, query, sort, section, contentFacet, page, content, confidence);
            return model;
        }

        protected virtual void SetupModel(CmsFilterOptionViewModel model, string q, string sort, string section,string contentFacet, string page, IContent content, string confidence)
        {
            EnsurePage(model, page);
            EnsureQ(model, q);
            EnsureSort(model, sort);
            EnsureSection(model, section);
            EnsureContentFacet(model, section);
            model.Confidence = decimal.TryParse(confidence, out var confidencePercentage) ? confidencePercentage : 0;
        }

        protected virtual void EnsurePage(CmsFilterOptionViewModel model, string page)
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

        protected virtual void EnsureQ(CmsFilterOptionViewModel model, string q)
        {
            if (string.IsNullOrEmpty(model.Q))
            {
                model.Q = q;
            }
        }

        protected virtual void EnsureSection(CmsFilterOptionViewModel model, string section)
        {
            if (string.IsNullOrEmpty(model.SectionFilter))
            {
                model.SectionFilter = section;
            }
        }

        protected virtual void EnsureContentFacet(CmsFilterOptionViewModel model, string contentFacet)
        {
            if (string.IsNullOrEmpty(model.ContentFacet))
            {
                model.ContentFacet = contentFacet;
            }
        }

        protected virtual void EnsureSort(CmsFilterOptionViewModel model, string sort)
        {
            if (string.IsNullOrEmpty(model.Sort))
            {
                model.Sort = sort;
            }
        }
    }
}
