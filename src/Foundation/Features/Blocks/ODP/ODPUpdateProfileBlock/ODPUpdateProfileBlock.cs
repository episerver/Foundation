using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web.Mvc;
using Foundation.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Foundation.Features.Blocks.ODP.ODPUpdateProfileBlock
{
    [ContentType(DisplayName = "ODP Update Profile Block",
        GUID = "1e03cbe4-36e7-4a6c-be49-436989c4ef93",
        Description = "Update profile in ODP.",
        GroupName = GroupNamesCustom.Odp)]
    [ImageUrl("/icons/cms/blocks/HtmlBlock.png")]
    public class ODPUpdateProfileBlock : BaseODPEventBlock
    {
        [CultureSpecific]
        [Display(Name = "Email Address", Order = 10, GroupName = SystemTabNames.Content)]
        public virtual string EmailAddress { get; set; }

        [Display(Name = "List of Fields", 
            Description = "Custom fields reference: https://app.zaius.com/?scope=2772#/custom_fields?activeTab=customers&rowStart=1&rowCount=1000",
            Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<Fields>))]
        public virtual IList<Fields> Fields { get; set; }

        [PropertyDefinitionTypePlugIn]
        public class FieldsProperty : PropertyList<Fields> { }

        [CultureSpecific]
        [Display(Name = "Add event for profile update",
           Description = "This will create an event in ODP for this change.",
           Order = 30,
           GroupName = SystemTabNames.Content)]
        public virtual bool PushEvent { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ShowSnippetEditMode = false;
            ShowSnippetFrontEnd = false;
            PushEvent = false;
        }
    }

    public class Fields
    {
        [Display(Name = "Field", Order = 10)]
        [SelectOne(SelectionFactoryType = typeof(ProfileFieldSelectionFactory))]
        public virtual string Field { get; set; }

        [Display(Name = "Other Profile Field Name (Set if Field is not in drop down)", Description = "Set this if the field doesn't exist in the drop down.", Order = 20)]
        public virtual string Other { get; set; }

        [Display(Name = "Value", Order = 30)]
        public virtual string Value { get; set; }
    }

    public static class ProfileField
    {
        public const string dob_day = "dob_day";
        public const string dob_month = "dob_month";
        public const string dob_year = "dob_year";
        public const string city = "city";
        public const string country = "country";
        public const string first_name = "first_name";
        public const string last_name = "last_name";
        public const string phone = "phone";
        public const string state = "state";
        public const string zip = "zip";
        public const string interests = "interests";
    }

    public class ProfileFieldSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                 new SelectItem { Text = "Interests", Value = ProfileField.interests },
                new SelectItem { Text = "Birthday Day", Value = ProfileField.dob_day },
                new SelectItem { Text = "Birthday Month", Value = ProfileField.dob_month },
                new SelectItem { Text = "Birthday Year", Value = ProfileField.dob_year },
                new SelectItem { Text = "City", Value = ProfileField.city },
                new SelectItem { Text = "Country", Value = ProfileField.country },
                new SelectItem { Text = "First Name", Value = ProfileField.first_name },
                new SelectItem { Text = "Last Name", Value = ProfileField.last_name },
                new SelectItem { Text = "Phone", Value = ProfileField.phone },
                new SelectItem { Text = "State", Value = ProfileField.state },
                new SelectItem { Text = "Zip", Value = ProfileField.zip }

            };
        }
    }

    public class ODPUpdateProfileBlockComponent : AsyncBlockComponent<ODPUpdateProfileBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(ODPUpdateProfileBlock currentBlock)
        {
            return await Task.FromResult(View("~/Features/Blocks/ODP/ODPUpdateProfileBlock/ODPUpdateProfileBlock.cshtml", currentBlock));
        }
    }
}