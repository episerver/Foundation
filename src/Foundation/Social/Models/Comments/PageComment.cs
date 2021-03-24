using System;

namespace Foundation.Social.Models.Comments
{
    public class PageComment
    {
        public string AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public string Body { get; set; }

        public string Target { get; set; }

        public DateTime Created { get; set; }
    }
}