using System.ComponentModel.DataAnnotations;

namespace Foundation.Social.ViewModels
{
    public class ReviewSubmissionViewModel
    {
        public ReviewSubmissionViewModel()
        {
        }

        public ReviewSubmissionViewModel(string productCode) => ProductCode = productCode;

        [Required]
        public string ProductCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a title for your review.")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please add a description to your review.")]
        public string Body { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide your nickname.")]
        public string Nickname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide your location.")]
        public string Location { get; set; }

        [Range(1, 5, ErrorMessage = "Please provide a rating from 1 to 5.")]
        public int Rating { get; set; }
    }
}