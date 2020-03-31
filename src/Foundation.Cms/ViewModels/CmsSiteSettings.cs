using EPiServer.Core;
using EPiServer.SpecializedProperties;

namespace Foundation.Cms.ViewModels
{
    public class CmsSiteSettings
    {
        public virtual ContentArea MainMenu { get; set; }
        public virtual LinkItemCollection MyAccountCmsMenu { get; set; }

        #region Footer

        public virtual string Introduction { get; set; }
        public virtual string CompanyHeader { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string CompanyAddress { get; set; }
        public virtual string CompanyPhone { get; set; }
        public virtual string CompanyEmail { get; set; }
        public virtual string LinksHeader { get; set; }
        public virtual LinkItemCollection Links { get; set; }
        public virtual string SocialHeader { get; set; }
        public virtual LinkItemCollection SocialLinks { get; set; }
        public virtual ContentArea ContentArea { get; set; }
        public virtual string FooterCopyrightText { get; set; }

        #endregion
    }
}