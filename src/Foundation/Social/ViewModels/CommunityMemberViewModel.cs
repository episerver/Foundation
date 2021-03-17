namespace Foundation.Social.ViewModels
{
    public class CommunityMemberViewModel
    {
        public CommunityMemberViewModel(string company, string name)
        {
            Company = company;
            Name = name;
        }

        public string Company { get; set; }

        public string Name { get; set; }
    }
}