using EPiServer.Core;
using EPiServer.DataAbstraction;
using System;
using System.Collections.Generic;

namespace Foundation.Features.Shared.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(ContentData))]
    public class MoveCategoryEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            foreach (ExtendedMetadata property in metadata.Properties)
            {
                if (property.PropertyName == "icategorizable_category")
                {
                    property.GroupName = SystemTabNames.PageHeader;
                    property.Order = 9000;
                }
            }
        }
    }
}