using EPiServer.Core;
using EPiServer.DataAbstraction;
using Geta.EpiCategories;
using Geta.EpiCategories.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    public abstract class FoundationBlockData : BlockData, ICategorizableContent
    {
        [Categories]
        [Display(Description = "Categories associated with this content", GroupName = SystemTabNames.PageHeader, Order = 0)]
        public virtual IList<ContentReference> Categories { get; set; }

        [Display(Name = "Top", GroupName = CmsTabNames.BlockPadding, Order = 1)]
        public virtual int PaddingTop { get; set; }

        [Display(Name = "Right", GroupName = CmsTabNames.BlockPadding, Order = 2)]
        public virtual int PaddingRight { get; set; }

        [Display(Name = "Bottom", GroupName = CmsTabNames.BlockPadding, Order = 3)]
        public virtual int PaddingBottom { get; set; }

        [Display(Name = "Left", GroupName = CmsTabNames.BlockPadding, Order = 4)]
        public virtual int PaddingLeft { get; set; }

        public string PaddingStyles
        {
            get
            {
                var paddingStyles = "";

                paddingStyles += PaddingTop > 0 ? "padding-top: " + PaddingTop + "px;" : "";
                paddingStyles += PaddingRight > 0 ? "padding-right: " + PaddingRight + "px;" : "";
                paddingStyles += PaddingBottom > 0 ? "padding-bottom: " + PaddingBottom + "px;" : "";
                paddingStyles += PaddingLeft > 0 ? "padding-left: " + PaddingLeft + "px;" : "";

                return paddingStyles;
            }
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            PaddingTop = 0;
            PaddingRight = 0;
            PaddingBottom = 0;
            PaddingLeft = 0;
        }
    }
}