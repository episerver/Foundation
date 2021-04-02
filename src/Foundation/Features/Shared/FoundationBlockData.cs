using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Shell.ObjectEditing;
using Foundation.Features.Shared.SelectionFactories;
using Foundation.Infrastructure;
using Geta.EpiCategories;
using Geta.EpiCategories.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Shared
{
    public abstract class FoundationBlockData : BlockData, ICategorizableContent
    {
        [Categories]
        [Display(Description = "Categories associated with this content", GroupName = SystemTabNames.PageHeader, Order = 0)]
        public virtual IList<ContentReference> Categories { get; set; }

        [SelectOne(SelectionFactoryType = typeof(PaddingSelectionFactory))]
        [Display(Name = "Padding", GroupName = TabNames.BlockStyling, Order = 1)]
        public virtual string Padding
        {
            get => this.GetPropertyValue(page => page.Padding) ?? "p-1";
            set => this.SetPropertyValue(page => page.Padding, value);
        }

        [SelectOne(SelectionFactoryType = typeof(MarginSelectionFactory))]
        [Display(Name = "Margin", GroupName = TabNames.BlockStyling, Order = 2)]
        public virtual string Margin
        {
            get => this.GetPropertyValue(page => page.Margin) ?? "m-0";
            set => this.SetPropertyValue(page => page.Margin, value);
        }

        [Display(Name = "Background color", GroupName = TabNames.BlockStyling, Order = 3)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string BackgroundColor
        {
            get { return this.GetPropertyValue(page => page.BackgroundColor) ?? "#00000000"; }
            set { this.SetPropertyValue(page => page.BackgroundColor, value); }
        }

        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        [Display(Name = "Block opacity (0 to 1)", GroupName = TabNames.BlockStyling, Order = 4)]
        public virtual double? BlockOpacity
        {
            get => this.GetPropertyValue(page => page.BlockOpacity) ?? 1;
            set => this.SetPropertyValue(page => page.BlockOpacity, value);
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            Padding = "p-1";
            Margin = "m-0";
            BackgroundColor = "#00000000";
            BlockOpacity = 1;

            base.SetDefaultValues(contentType);
        }
    }
}