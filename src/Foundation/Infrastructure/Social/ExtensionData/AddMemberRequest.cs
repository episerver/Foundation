namespace Foundation.Social.ExtensionData
{
    public class AddMemberRequest : MemberExtensionData
    {
        public AddMemberRequest()
        {
        }

        public AddMemberRequest(string group, string user, string email, string company)
        {
            Group = group;
            User = user;
            Company = company;
            Email = email;
        }

        public string Group { get; set; }

        public string User { get; set; }
    }
}