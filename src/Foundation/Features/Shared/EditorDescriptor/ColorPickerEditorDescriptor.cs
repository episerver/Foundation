using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Foundation.Features.Shared.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = "ColorPicker")]
    public class ColorPickerEditorDescriptor : EditorDescriptor
    {
        private const string _editingClient = "foundation/Editors/ColorPicker";

        public ColorPickerEditorDescriptor()
        {
            ClientEditingClass = _editingClient;
        }

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            ClientEditingClass = _editingClient;
            base.ModifyMetadata(metadata, attributes);
        }
    }
}