using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using System;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(String),
    UIHint = UIHint)]
    public class ColorPiclerEditorDescriptor : EditorDescriptor
    {
        public const string UIHint = "ColorPicker";
        private const string EditingClient = "foundation/editors/ColorPicker";

        public ColorPiclerEditorDescriptor() => ClientEditingClass = EditingClient;

        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            ClientEditingClass = EditingClient;
            base.ModifyMetadata(metadata, attributes);
        }
    }
}
