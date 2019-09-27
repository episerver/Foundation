namespace Foundation.Social.ExtensionData
{
    public class MemberExtensionData
    {
        public MemberExtensionData()
        {
        }

        public MemberExtensionData(string email, string company)
        {
            Email = email;
            Company = company;
        }

        public string Email { get; set; }

        public string Company { get; set; }
    }
}