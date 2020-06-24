using System;

namespace Foundation.Features.MyOrganization.Users
{
    public class UserSearchResultModel
    {
        public Guid ContactId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}