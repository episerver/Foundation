using EPiServer;
using EPiServer.Core;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Foundation.Cms.Pages;
using Geta.EpiCategories;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Foundation.Cms.VisitorCriterions.GetaCategoryCriterion
{
    [VisitorGroupCriterion(
        Category = "Site Criteria",
        DisplayName = "Visited Geta Category",
        Description = "Match when the visitor has visited a page with a specified geta category."
    )]
    public class GetaCategoryCriterion : CriterionBase<GetaCategoryCriterionSettings>
    {
        private readonly IStateStorage _stateStorage;
        private readonly IContentLoader _contentLoader;
        private readonly ICategoryContentLoader _categoryContentLoader;
        private const string _STORAGEKEY = "Epi:GetaCategoryViewedPage";

        public GetaCategoryCriterion() 
            : this(ServiceLocator.Current.GetInstance<IStateStorage>(),
            ServiceLocator.Current.GetInstance<IContentLoader>(),
            ServiceLocator.Current.GetInstance<ICategoryContentLoader>())
        {
        }
        public GetaCategoryCriterion(IStateStorage stateStorage, IContentLoader contentLoader, ICategoryContentLoader categoryContentLoader)
        {
            _stateStorage = stateStorage;
            _contentLoader = contentLoader;
            _categoryContentLoader = categoryContentLoader;
        }

        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            if (_stateStorage.IsAvailable && GetVisitedTimes() >= Model.ViewedTimes)
                return true;

            return false;
        }

        public override void Subscribe(ICriterionEvents criterionEvents)
        {
            criterionEvents.VisitedPage += VisitedPage;
        }

        public override void Unsubscribe(ICriterionEvents criterionEvents)
        {
            criterionEvents.VisitedPage -= VisitedPage;
        }

        private void VisitedPage(object sender, CriterionEventArgs e)
        {
            var page = _contentLoader.Get<IContent>(e.GetPageLink()) as FoundationPageData;
            var pageCatIds = page.Categories?.Select(x => x.ID);
            if (_stateStorage.IsAvailable && pageCatIds != null && pageCatIds.Contains(int.Parse(Model.CategoryId))) {
                var times = GetVisitedTimes() + 1;
                _stateStorage.Save(_STORAGEKEY, times);
            }
        }

        private int GetVisitedTimes()
        {
            var timesObj = _stateStorage.Load(_STORAGEKEY);
            if (timesObj == null || string.IsNullOrEmpty(timesObj.ToString()))
            {
                return 0;
            }

            if (int.TryParse(timesObj.ToString(), out int times))
            {
                return times;
            }

            return 0;
        }
    }
}
