using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Social.Adapters;
using Foundation.Social.Repositories.ActivityStreams;
using Foundation.Social.Repositories.Comments;
using Foundation.Social.Repositories.Common;
using Foundation.Social.Repositories.Groups;
using Foundation.Social.Repositories.Moderation;
using Foundation.Social.Repositories.Ratings;
using Foundation.Social.Services;

namespace Foundation.Social
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class Initialize : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPageCommentRepository, PageCommentRepository>();
            services.AddTransient<IPageRatingRepository, PageRatingRepository>();
            services.AddTransient<IPageSubscriptionRepository, PageSubscriptionRepository>();
            services.AddTransient<ICommunityActivityAdapter, CommunityActivityAdapter>();
            services.AddTransient<ICommunityFeedRepository, CommunityFeedRepository>();
            services.AddTransient<ICommunityActivityRepository, CommunityActivityRepository>();
            services.AddTransient<ICommunityRepository, CommunityRepository>();
            services.AddTransient<ICommunityMemberRepository, CommunityMemberRepository>();
            services.AddTransient<ICommunityMembershipModerationRepository, CommunityMembershipModerationRepository>();
            services.AddTransient<IReviewActivityService, ReviewActivityService>();
            services.AddTransient<IBlogCommentRepository, BlogCommentRepository>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ICommentManagerService, CommentManagerService>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}