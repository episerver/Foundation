using EPiServer.Core;

namespace Foundation.Social.ViewModels
{
    public class RatingFormViewModel
    {
        public int? SubmittedRating { get; set; }

        public bool SendActivity { get; set; }

        public ContentReference CurrentLink { get; set; }
    }
}