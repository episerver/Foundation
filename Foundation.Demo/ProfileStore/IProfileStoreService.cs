using EPiServer.Core;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Demo.ProfileStore
{
    public interface IProfileStoreService
    {
        Task<ProfileStoreItems> GetAllProfiles(ProfileStoreFilterOptions profileStoreFilterOptions);
        Task<ProfileStoreItems> GetProfiles(string queryString);
        Task<ProfileStoreModel> GetProfileById(string scope, Guid profileId);
        Task EditOrCreateProfile(string scope, ProfileStoreModel model);
        void LoadCountry(ProfileStoreModel profileModel);
        Task<ScopeItems> GetAllScopes();
        Task<ScopeItems> GetScopesById(Guid scopeId);
        Task<SegmentItems> GetAllSegments();
        Task<SegmentItems> GetSegmentById(Guid scopeId);
        Task EditOrCreateSegment(SegmentModel model);
        Task<BlacklistItems> GetAllBlacklist();
        Task<BlacklistItems> GetBlacklistById(Guid blacklistId);
        Task<VisualizationItems> GetVisualizationItems(string queryString);
        Task<TrackEventItems> GetAllTrackEvents(ProfileStoreFilterOptions profileStoreFilterOptions);
        void TrackSearch(PageData pageData, HttpContextBase httpContextBase, string keyword);
    }
}
