using System;

namespace Foundation.Features.Header
{
    public class DemoUserViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public int SortOrder { get; set; }
    }
}
