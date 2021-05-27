using EPiServer.Framework.Cache;
using EPiServer.Logging;
using EPiServer.Social.Comments.Core;
using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using Foundation.Social.Composites;
using Foundation.Social.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ICommentService _commentService;
        private readonly IRatingService _ratingService;
        private readonly IRatingStatisticsService _ratingStatisticsService;
        private readonly ISynchronizedObjectInstanceCache _cache;
        private readonly ILogger _logger = LogManager.GetLogger(typeof(ReviewService));

        private readonly string CachePrefix = "Foundation:Review:";

        public ReviewService(ICommentService commentService, IRatingService ratingService,
            IRatingStatisticsService ratingStatisticsService, ISynchronizedObjectInstanceCache cache)
        {
            _commentService = commentService;
            _ratingService = ratingService;
            _ratingStatisticsService = ratingStatisticsService;
            _cache = cache;
        }

        public ReviewViewModel Add(ReviewSubmissionViewModel review)
        {
            // Instantiate a reference for the product
            var product = Reference.Create($"product://{review.ProductCode}");

            // Instantiate a reference for the contributor
            var contributor = Reference.Create($"visitor://{review.Nickname}");

            // Add the contributor's rating for the product
            var submittedRating = new Rating(contributor, product, new RatingValue(review.Rating));
            var storedRating = _ratingService.Add(submittedRating);

            // Compose a comment representing the review
            var comment = new Comment(product, contributor, review.Body, true);
            var extension = new Review
            {
                Title = review.Title,
                Location = review.Location,
                Nickname = review.Nickname,
                Rating = new ReviewRating
                {
                    Value = review.Rating,
                    Reference = storedRating.Id.Id
                }
            };

            var result = _commentService.Add(comment, extension);
            _cache.Remove(CachePrefix + review.ProductCode);
            // Add the composite comment for the product
            return ViewModelAdapter.Adapt(result);
        }

        public ReviewsViewModel Get(string productCode)
        {
            return _cache.ReadThrough(CachePrefix + productCode, () =>
            {
                // Instantiate a reference for the product
                var product = Reference.Create($"product://{productCode}");

                try
                {
                    // Retrieve the rating statistics for the product
                    var statistics = GetProductStatistics(product);

                    // Retrieve the reviews for the product
                    var reviews = GetProductReviews(product);

                    // Return the data as a ReviewsViewModel
                    return new ReviewsViewModel
                    {
                        Statistics = ViewModelAdapter.Adapt(statistics),
                        Reviews = ViewModelAdapter.Adapt(reviews)
                    };
                }
                catch (SocialRepositoryException)
                {
                    //DO SOMETHING
                }

                return new ReviewsViewModel();
            },
            (x) => new CacheEvictionPolicy(TimeSpan.FromMinutes(15), CacheTimeoutType.Absolute),
            ReadStrategy.Wait);
        }

        public IEnumerable<ReviewViewModel> Get(Visibility visibility, int page, int limit, out long total)
        {
            var result = new List<ReviewViewModel>();
            try
            {
                // Retrieve the reviews for the product
                //var reviews = this.GetProductReviews(visibility, page, limit, out total);
                // Retrieve the comments for the product, page, blog...
                var comments = GetComments(visibility, page, limit, out total);

                // Return the data as a ReviewsViewModel
                return comments;
            }
            catch (SocialRepositoryException)
            {
                //DO SOMETHING
            }

            total = result.Count;
            return result;
        }

        public IEnumerable<ReviewStatisticsViewModel> GetRatings(IEnumerable<string> productCodes)
        {
            //ResultPage<Composite<RatingStatistics, Review>> statistics = null;
            ResultPage<RatingStatistics> statistics = null;

            var statisticsCriteria = new Criteria<RatingStatisticsFilter>
            {
                Filter = new RatingStatisticsFilter
                {
                    Targets = productCodes.Select(x => Reference.Create($"product://{x}"))
                },
                PageInfo = new PageInfo
                {
                    PageSize = productCodes.Count()
                }
            };

            try
            {
                statistics = _ratingStatisticsService.Get(statisticsCriteria);

                if (!statistics.Results.Any())
                {
                    return new List<ReviewStatisticsViewModel>();
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.",
                    ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException(
                    "The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return statistics.Results.Select(x => ViewModelAdapter.Adapt(x));
        }

        private RatingStatistics GetProductStatistics(Reference product)
        {
            var statisticsCriteria = new Criteria<RatingStatisticsFilter>
            {
                Filter = new RatingStatisticsFilter
                {
                    Targets = new List<Reference> { product }
                },
                PageInfo = new PageInfo
                {
                    PageSize = 1
                }
            };

            RatingStatistics statistics = null;

            try
            {
                statistics = _ratingStatisticsService.Get(statisticsCriteria).Results.FirstOrDefault();
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.",
                    ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException(
                    "The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return statistics;
        }

        private IEnumerable<Composite<Comment, Review>> GetProductReviews(Reference product)
        {
            var commentCriteria = new CompositeCriteria<CommentFilter, Review>
            {
                Filter = new CommentFilter
                {
                    Parent = product
                },
                PageInfo = new PageInfo
                {
                    PageSize = 20
                },
                OrderBy = new List<SortInfo>
                {
                    new SortInfo(CommentSortFields.Created, false)
                }
            };

            ResultPage<Composite<Comment, Review>> ratings = null;

            try
            {
                ratings = _commentService.Get(commentCriteria);
                //return this._commentService.Get(commentCriteria).Results;
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.",
                    ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException(
                    "The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return ratings.Results;
        }

        private IEnumerable<Composite<Comment, Review>> GetProductReviews(Visibility visibility, int page, int limit,
            out long total)
        {
            var commentCriteria = new CompositeCriteria<CommentFilter, Review>
            {
                Filter = new CommentFilter
                {
                    Visibility = visibility
                },
                PageInfo = new PageInfo
                {
                    PageSize = limit,
                    CalculateTotalCount = true,
                    PageOffset = (page - 1) * limit
                },
                OrderBy = new List<SortInfo>
                {
                    new SortInfo(CommentSortFields.Created, false)
                }
            };

            ResultPage<Composite<Comment, Review>> ratings = null;

            try
            {
                ratings = _commentService.Get(commentCriteria);
                total = ratings.TotalCount;
                //return this._commentService.Get(commentCriteria).Results;
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.",
                    ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException(
                    "The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return ratings.Results;
        }

        private IEnumerable<ReviewViewModel> GetComments(Visibility visibility, int page, int limit, out long total)
        {
            var criteria = new Criteria<CommentFilter>
            {
                Filter = new CommentFilter
                {
                    Visibility = visibility
                },
                PageInfo = new PageInfo
                {
                    PageSize = limit,
                    CalculateTotalCount = true,
                    PageOffset = (page - 1) * limit
                },
                OrderBy = new List<SortInfo>
                {
                    new SortInfo(CommentSortFields.Created, false)
                }
            };

            try
            {
                var result = new List<ReviewViewModel>();
                var cmts = _commentService.Get(criteria);
                total = cmts.TotalCount;
                foreach (var cmt in cmts.Results)
                {
                    if (cmt.Parent.Id.IndexOf("product") > -1)
                    {
                        try
                        {
                            var review = _commentService.Get<Review>(cmt.Id);
                            result.Add(ViewModelAdapter.Adapt(review));
                        }
                        catch
                        {
                            result.Add(new ReviewViewModel
                            {
                                AddedOnStr = cmt.Created.ToString("MM/dd/yyyy hh:mm:ss"),
                                AddedOn = cmt.Created,
                                Body = cmt.Body,
                                Location = "",
                                Nickname = "",
                                Rating = 0,
                                Title = "",
                                Id = cmt.Id,
                                Parent = cmt.Parent,
                                Author = cmt.Author,
                                IsVisible = cmt.IsVisible
                            });
                        }
                    }
                    else
                    {
                        result.Add(new ReviewViewModel
                        {
                            AddedOnStr = cmt.Created.ToString("MM/dd/yyyy hh:mm:ss"),
                            AddedOn = cmt.Created,
                            Body = cmt.Body,
                            Location = "",
                            Nickname = "",
                            Rating = 0,
                            Title = "",
                            Id = cmt.Id,
                            Parent = cmt.Parent,
                            Author = cmt.Author,
                            IsVisible = cmt.IsVisible
                        });
                    }
                }

                return result;
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.",
                    ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException(
                    "The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }
        }
    }
}