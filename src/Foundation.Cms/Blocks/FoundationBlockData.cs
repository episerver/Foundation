using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.EditorDescriptors;
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

        [SelectOne(SelectionFactoryType = typeof(PaddingSelectionFactory))]
        [Display(Name = "Padding", GroupName = CmsTabNames.BlockStyling, Order = 1)]
        public virtual string Padding { get; set; }

        [SelectOne(SelectionFactoryType = typeof(MarginSelectionFactory))]
        [Display(Name = "Margin", GroupName = CmsTabNames.BlockStyling, Order = 2)]
        public virtual string Margin { get; set; }

        [SelectOne(SelectionFactoryType = typeof(BackgroundColorSelectionFactory))]
        [Display(Name = "Background color", GroupName = CmsTabNames.BlockStyling, Order = 3)]
        public virtual string BackgroundColor { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Padding = "p-1";
            Margin = "m-0";
            BackgroundColor = "background-color: transparent;";
        }
    }
}