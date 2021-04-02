using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Recommendations.WidgetBlock
{
    [ContentType(DisplayName = "Recommendation Widget",
        GUID = "d5cc427b-afa4-4c4d-8986-eb5f73e0b9fe",
        Description = "Block that adds recommendations based on selected widget type",
        GroupName = "Personalization")]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-07.png")]
    public class WidgetBlock : BlockData
    {
        [SelectOne(SelectionFactoryType = typeof(WidgetSelectionFactory))]
        [Display(Name = "Widget type", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string WidgetType { get; set; }

        [Display(Name = "Number of recommendations", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual int NumberOfRecommendations { get; set; }

        [Display(Name = "Attribute name", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string Name { get; set; }

        [Display(Name = "Attribute value", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual string Value { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            NumberOfRecommendations = 4;
        }
    }
}