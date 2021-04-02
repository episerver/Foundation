using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;

namespace Foundation.Features.Shared.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = "ColorPicker")]
    public class ColorPickerEditorDescriptor : EditorDescriptor
    {
        private const string _editingClient = "foundation/editors/ColorPicker";

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