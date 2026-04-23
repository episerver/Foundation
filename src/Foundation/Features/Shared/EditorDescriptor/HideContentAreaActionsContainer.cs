using EPiServer.Applications;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Cms.Shell.UI.ObjectEditing.InternalMetadata;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Web;

namespace Foundation.Features.Shared.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = "HideContentAreaActionsContainer")]
    public class HideContentAreaActionsContainer : ContentAreaEditorDescriptor
    {
        // CMS 13: ContentAreaEditorDescriptor constructor now requires these parameters.
        public HideContentAreaActionsContainer(
            InlineBlockNamePropertiesOptions inlineBlockNamePropertiesOptions,
            ListLengthSettingsProvider listLengthSettingsProvider,
            IApplicationResolver applicationResolver,
            ServiceAccessor<SystemDefinition> systemDefinitionAccessor)
            : base(inlineBlockNamePropertiesOptions, listLengthSettingsProvider, applicationResolver, systemDefinitionAccessor)
        {
        }

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);
            metadata.OverlayConfiguration["className"] = "epi-hide-actionscontainer";
        }
    }
}