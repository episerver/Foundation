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
    [ContentType(DisplayName = "Feature Box Block", GUID = "aa3e5e42-6fbe-416f-8027-e77aa290d09a", Description = "Define a feature box at the footer of the page")]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-11.png")]
    public class FeatureBoxBlock : FoundationBlockData
    {
        /// <summary>
        /// Represents the name of FontAwesome.
        /// </summary>
        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(FontAwesomeSelectionFactory))]
        [Display(Name = "Icon name", GroupName = SystemTabNames.Content)]
        public virtual string IconName { get; set; }

        /// <summary>
        /// Represents the content of the feature box.
        /// </summary>
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Text", GroupName = SystemTabNames.Content)]
        public virtual string Content { get; set; }
    }
}
