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
    public static class ViewModelAdapter
    {
        public static ReviewStatisticsViewModel Adapt(RatingStatistics statistics)
        {
            var viewModel = new ReviewStatisticsViewModel();

            if (statistics != null)
            {
                viewModel.OverallRating = Convert.ToDouble(statistics.Sum) / Convert.ToDouble(statistics.TotalCount);
                viewModel.TotalRatings = statistics.TotalCount;
                viewModel.Code = statistics.Target.Id.Replace("product://", "");
            }

            return viewModel;
        }

        public static IEnumerable<ReviewViewModel> Adapt(IEnumerable<Composite<Comment, Review>> reviews) => reviews.Select(Adapt);

        public static ReviewViewModel Adapt(Composite<Comment, Review> review)
        {
            return new ReviewViewModel
            {
                AddedOnStr = review.Data.Created.ToString("MM/dd/yyyy hh:mm:ss"),
                AddedOn = review.Data.Created,
                Body = review.Data.Body,
                Location = review.Extension?.Location ?? "",
                Nickname = review.Extension?.Nickname ?? "",
                Rating = review.Extension?.Rating.Value ?? 0,
                Title = review.Extension?.Title ?? "",
                Id = review.Data.Id,
                Parent = review.Data.Parent,
                Author = review.Data.Author,
                IsVisible = review.Data.IsVisible
            };
        }
    }
}