namespace Foundation.Social.Models.Groups
{
    public class CommunityMember
    {
        public CommunityMember(string user, string groupId, string email, string company)
        {
            User = user;
            GroupId = groupId;
            Email = email;
            Company = company;
        }

        public string User { get; set; }

        public string GroupId { get; set; }

        public string Email { get; set; }

        public string Company { get; set; }
    }
}