using System.Collections.Generic;

namespace Foundation.Social.ViewModels
{
    public class ReviewsViewModel
    {
        public ReviewsViewModel()
        {
            Reviews = new List<ReviewViewModel>();
            Statistics = new ReviewStatisticsViewModel();
        }

        public ReviewStatisticsViewModel Statistics { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}