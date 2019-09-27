using EPiServer.Shell;

namespace Foundation.Find.Cms.Models.Blocks.ProductFilters
{
    [UIDescriptorRegistration]
    public class CatalogContentUiDescriptor : UIDescriptor<FilterBaseBlock>
    {
        public CatalogContentUiDescriptor() => DefaultView = CmsViewNames.AllPropertiesView;
    }
}