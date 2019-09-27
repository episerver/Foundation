using EPiServer;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Menu Item Block", GUID = "a6d0242a-3946-4a80-9eec-4d9b2e5fc2d0", Description = "")]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-23.png")]
    public class MenuItemBlock : BlockData
    {

        [CultureSpecific]
        [Display(
            Name = "Name",
            Description = "Name in Menu",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Name { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Link",
            Description = "Link",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual Url Link { get; set; }

        [Display(
            Name = "Menu Item Image",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference MenuImage { get; set; }

        [Display(
            Name = "Child Items",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 4)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<GroupLinkCollection>))]
        [JsonIgnore]
        public virtual IList<GroupLinkCollection> ChildItems { get; set; }

        [Display(
            Name = "Teaser Text",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 6)]
        public virtual XhtmlString TeaserText { get; set; }

        [Display(
            Name = "Button Text",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 7)]
        public virtual string ButtonText { get; set; }

        [Display(
            Name = "Button Link",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 8)]
        public virtual Url ButtonLink { get; set; }
    }

    public class GroupLinkCollection
    {
        [Display(
            Name = "Main Category Text",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string MainCategoryText { get; set; }

        [Display(
            Name = "Category Links",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual LinkItemCollection ListCategories { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class GroupLinkCollectionProperty : PropertyList<GroupLinkCollection>
    {
        protected override GroupLinkCollection ParseItem(string value)
        {
            return JsonConvert.DeserializeObject<GroupLinkCollection>(value);
        }
    }
}