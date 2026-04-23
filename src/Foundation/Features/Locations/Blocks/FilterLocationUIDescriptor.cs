using EPiServer.Shell;

namespace Foundation.Features.Locations.Blocks
{
    [UIDescriptorRegistration]
    public class FilterLocationUIDescriptor : UIDescriptor<IFilterBlock>
    {
        public FilterLocationUIDescriptor()
        {
            DefaultView = CmsViewNames.AllPropertiesView;
            if (DisabledViews == null)
            {
                DisabledViews = new List<string>();
            }
            DisabledViews.Add(CmsViewNames.OnPageEditView);
            DisabledViews.Add(CmsViewNames.PreviewView);
            DisabledViews.Add(CmsViewNames.SideBySideCompareView);
        }
    }
}
