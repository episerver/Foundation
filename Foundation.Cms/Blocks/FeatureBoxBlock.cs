using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    /// <summary>
    /// FeatureBox Block model presents information at the footer of the page.
    /// </summary>
    [ContentType(DisplayName = "FeatureBox Block", Description = "Define a feature box at the footer of the page", GUID = "aa3e5e42-6fbe-416f-8027-e77aa290d09a")]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-11.png")]
    public class FeatureBoxBlock : FoundationBlockData
    {
        /// <summary>
        /// Represents the name of FontAwesome.
        /// </summary>
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 1)]
        [SelectOne(SelectionFactoryType = typeof(FontAwesomeSelectionFactory))]
        public virtual string IconName { get; set; }

        /// <summary>
        /// Represents the content of the feature box.
        /// </summary>
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 2)]
        [UIHint(UIHint.Textarea)]
        public virtual string Content { get; set; }
    }
}
