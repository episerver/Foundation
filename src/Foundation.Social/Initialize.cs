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
            services.AddSingleton<IPageRepository, PageRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IPageCommentRepository, PageCommentRepository>();
            services.AddSingleton<IPageRatingRepository, PageRatingRepository>();
            services.AddSingleton<IPageSubscriptionRepository, PageSubscriptionRepository>();
            services.AddSingleton<ICommunityActivityAdapter, CommunityActivityAdapter>();
            services.AddSingleton<ICommunityFeedRepository, CommunityFeedRepository>();
            services.AddSingleton<ICommunityActivityRepository, CommunityActivityRepository>();
            services.AddSingleton<ICommunityRepository, CommunityRepository>();
            services.AddSingleton<ICommunityMemberRepository, CommunityMemberRepository>();
            services.AddSingleton<ICommunityMembershipModerationRepository, CommunityMembershipModerationRepository>();
            services.AddSingleton<IReviewActivityService, ReviewActivityService>();
            services.AddSingleton<IBlogCommentRepository, BlogCommentRepository>();
            services.AddTransient<ICommentManagerService, CommentManagerService>();
            services.AddSingleton<IReviewService, ReviewService>();
            services.AddSingleton<IReviewActivityService, ReviewActivityService>();
            services.AddSingleton<ICommentManagerService, CommentManagerService>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {

        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}