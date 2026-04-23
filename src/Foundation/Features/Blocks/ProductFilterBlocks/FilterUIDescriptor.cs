using EPiServer.Shell;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    [UIDescriptorRegistration]
    public class FilterUIDescriptor : UIDescriptor<FilterBaseBlock>
    {
        public FilterUIDescriptor()
        {
            DefaultView = CmsViewNames.AllPropertiesView;
            DisabledViews = new List<string>
            {
                CmsViewNames.OnPageEditView,
                CmsViewNames.PreviewView,
                CmsViewNames.SideBySideCompareView
            };
        }
    }
}
