using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;

namespace Foundation.Infrastructure.Find.Facets.Config
{
    public class SelectOneEnumAttribute : SelectOneAttribute
    {
        public SelectOneEnumAttribute(Type enumType)
        {
            EnumType = enumType;
        }

        public Type EnumType { get; set; }

        public new void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            SelectionFactoryType = typeof(EnumSelectionFactory<>).MakeGenericType(EnumType);
            base.CreateDisplayMetadata(context);
        }
    }
}
