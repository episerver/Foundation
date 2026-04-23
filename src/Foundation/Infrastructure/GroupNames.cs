namespace Foundation.Infrastructure
{
    [GroupDefinitions]
    public static class GroupNames
    {
        [Display(Name = "Content", Order = 510)]
        public const string Content = "Content";

        [Display(Order = 520)]
        public const string Commerce = "Commerce";
        
        [Display(Order = 530)]
        public const string Account = "Account";

        [Display(Order = 540)]
        public const string Blog = "Blog";

        [Display(Name = "Calendar", Order = 550)]
        public const string Calendar = "Calendar";

        [Display(Order = 570)]
        public const string Forms = "Forms";

        // CMS 13: tab name values cannot contain spaces; display name set via Display(Name=).
        [Display(Name = "Location Blocks", Order = 575)]
        public const string LocationBlocks = "LocationBlocks";

        [Display(Order = 580)]
        public const string Multimedia = "Multimedia";

        // CMS 13: tab name values cannot contain spaces; display name set via Display(Name=).
        [Display(Name = "Social media", Order = 600)]
        public const string SocialMedia = "SocialMedia";

        [Display(Order = 610)]
        public const string Social = "Social";

        [Display(Order = 620)]
        public const string Syndication = "Syndication";
    }
}
