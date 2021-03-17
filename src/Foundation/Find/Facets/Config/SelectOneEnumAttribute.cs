using EPiServer.Shell.ObjectEditing;
using System;
using System.Web.Mvc;

namespace Foundation.Find.Facets.Config
{
    public class SelectOneEnumAttribute : SelectOneAttribute, IMetadataAware
    {
        public SelectOneEnumAttribute(Type enumType) => EnumType = enumType;

        public Type EnumType { get; set; }

        public new void OnMetadataCreated(ModelMetadata metadata)
        {
            var enumType = metadata.ModelType;

            SelectionFactoryType = typeof(EnumSelectionFactory<>).MakeGenericType(EnumType);

            base.OnMetadataCreated(metadata);
        }
    }
}
