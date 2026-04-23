using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

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
            // CMS 13 removed: EnumSelectionFactory<> no longer exists in EPiServer.Shell.ObjectEditing.
            // SelectionFactoryType = typeof(EnumSelectionFactory<>).MakeGenericType(EnumType);
            base.CreateDisplayMetadata(context);
        }
    }
}
