﻿using EPiServer.SpecializedProperties;
using Geta.Optimizely.Categories;

namespace Foundation.Features.Category
{
    [ContentType(GUID = "A9BBD7FC-27C5-4718-890A-E28ACBE5EE26",
        DisplayName = "Standard Category",
        Description = "Used to categorize content")]
    public class StandardCategory : CategoryData, IFoundationContent
    {
        #region Implement IFoundationContent

        [CultureSpecific]
        [Display(Name = "Hide site header", GroupName = TabNames.Settings, Order = 100)]
        public virtual bool HideSiteHeader { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site footer", GroupName = TabNames.Settings, Order = 200)]
        public virtual bool HideSiteFooter { get; set; }

        [Display(Name = "CSS files", GroupName = TabNames.Styles, Order = 100)]
        public virtual LinkItemCollection CssFiles { get; set; }

        [Display(Name = "CSS", GroupName = TabNames.Styles, Order = 200)]
        [UIHint(UIHint.Textarea)]
        public virtual string Css { get; set; }

        [Display(Name = "Script files", GroupName = TabNames.Scripts, Order = 100)]
        public virtual LinkItemCollection ScriptFiles { get; set; }

        [UIHint(UIHint.Textarea)]
        [Display(GroupName = TabNames.Scripts, Order = 200)]
        public virtual string Scripts { get; set; }

        #endregion
    }
}