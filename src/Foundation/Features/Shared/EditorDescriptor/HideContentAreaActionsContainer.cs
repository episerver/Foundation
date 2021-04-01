using EPiServer.Core;
using System;
using System.Collections.Generic;

namespace Foundation.Features.Shared.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(ContentArea), UIHint = "HideContentAreaActionsContainer")]
    public class HideContentAreaActionsContainer : ContentAreaEditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);
            metadata.OverlayConfiguration["className"] = "epi-hide-actionscontainer";
        }
    }
}