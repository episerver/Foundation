using EPiServer.Framework.DataAnnotations;
using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.LikeButtonBlock
{
    /// <summary>
    /// The LikeButtonBlockController is a simple implementation of a Like button using
    /// the Episerver Social Ratings service API. In this implementation we assume that
    /// if the user is not logged in (anonymous), they can "like" the page as many times
    /// as they choose.
    /// </summary>
    [TemplateDescriptor(Default = true)]
    public class LikeButtonBlockController : BlockController<LikeButtonBlock>
    {
        private readonly IRatingService _ratingService;
        private readonly IRatingStatisticsService _ratingStatisticsService;
        private readonly IPageRouteHelper _pageRouteHelper;
        private const int LikedRating = 1;

        /// <summary>
        /// Constructor
        /// </summary>
        public LikeButtonBlockController(IRatingService ratingService, IRatingStatisticsService ratingStatisticsService, IPageRouteHelper pageRouteHelper)
        {
            _ratingStatisticsService = ratingStatisticsService;
            // This is all wired up by the installation of the EPiServer.Social.Ratings.Site package
            _ratingService = ratingService;

            // This is wired up by Episerver Core/Framework
            _pageRouteHelper = pageRouteHelper;
        }

        /// <summary>
        /// Render the Like button block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        /// <returns>Result of the redirect to the current page.</returns>
        public override ActionResult Index(LikeButtonBlock currentBlock)
        {
            var pageLink = _pageRouteHelper.PageLink;
            var targetPageRef = Reference.Create(PermanentLinkUtility.FindGuid(pageLink).ToString());
            var anonymousUser = User.Identity.GetUserId() == null ? true : false;

            // Create a rating block view model to fill the frontend block view
            var blockModel = new LikeButtonBlockViewModel(currentBlock)
            {
                Link = pageLink
            };

            try
            {
                // Using the Episerver Social Rating service, get the existing rating for the current
                // user (rater) and page (target). This is done only if there's a user identity. Anonymous
                // users will never match a previously submitted anonymous Like rating as they are always
                // uniquely generated.
                if (!anonymousUser)
                {
                    var raterUserRef = GetRaterRef();
                    var ratingPage = _ratingService.Get(
                        new Criteria<RatingFilter>
                        {
                            Filter = new RatingFilter
                            {
                                Rater = raterUserRef,
                                Targets = new List<Reference>
                                {
                                    targetPageRef
                                }
                            },
                            PageInfo = new PageInfo
                            {
                                PageSize = 1
                            }
                        }
                    );

                    // Add the current Like rating, if any, to the block view model. If the user is logged
                    // into the site and had previously liked the current page then the CurrentRating value
                    // should be 1 (LIKED_RATING).  Anonymous user Likes are generated with unique random users
                    // and thus the current anonymous user will never see a current rating value as he/she
                    // can Like the page indefinitely.
                    if (ratingPage.Results.Count() > 0)
                    {
                        blockModel.CurrentRating = ratingPage.Results.ToList().FirstOrDefault().Value.Value;
                    }
                }

                // Using the Episerver Social Rating service, get the existing Like statistics for the page (target)
                var ratingStatisticsPage = _ratingStatisticsService.Get(
                    new Criteria<RatingStatisticsFilter>
                    {
                        Filter = new RatingStatisticsFilter
                        {
                            Targets = new List<Reference>
                            {
                                targetPageRef
                            }
                        },
                        PageInfo = new PageInfo
                        {
                            PageSize = 1
                        }
                    }
                );

                // Add the page Like statistics to the block view model
                if (ratingStatisticsPage.Results.Count() > 0)
                {
                    var statistics = ratingStatisticsPage.Results.ToList().FirstOrDefault();
                    if (statistics.TotalCount > 0)
                    {
                        blockModel.TotalCount = statistics.TotalCount;
                    }
                }
            }
            catch (Exception)
            {
                // The rating service may throw a number of possible exceptions
                // should handle each one accordingly -- see rating service documentation
            }

            return PartialView("~/Features/Blocks/Views/LikeButtonBlock.cshtml", blockModel);
        }

        /// <summary>
        /// Submit handles a click of the Like button.  It accepts a Like button block model,
        /// saves the Like rating, and redirects back to the current page.
        /// </summary>
        /// <param name="likeButtonBlock">The Like button block model.</param>
        /// <returns>Result of the redirect to the current page.</returns>
        [HttpPost]
        public ActionResult Submit(LikeButtonBlockViewModel likeButtonBlock)
        {
            var targetPageRef = Reference.Create(PermanentLinkUtility.FindGuid(likeButtonBlock.Link).ToString());
            var raterUserRef = GetRaterRef();

            try
            {
                // Add the rating using the Episerver Social Rating service
                var addedRating = _ratingService.Add(
                    new Rating(
                        raterUserRef,
                        targetPageRef,
                        new RatingValue(LikedRating)
                    )
                );
            }
            catch (Exception)
            {
                // The rating service may throw a number of possible exceptions
                // should handle each one accordingly -- see rating service documentation
            }

            return Redirect(UrlResolver.Current.GetUrl(likeButtonBlock.Link));
        }

        private Reference GetRaterRef()
        {
            Reference raterUserRef;

            // If we have a user identity use it;  if not we generate a unique anonymous user for the rater reference
            var userIdentity = User.Identity.GetUserId();
            if (userIdentity != null)
            {
                raterUserRef = Reference.Create(userIdentity);
            }
            else
            {
                raterUserRef = Reference.Create("anonymous-" + Guid.NewGuid());
            }

            return raterUserRef;
        }
    }
}
