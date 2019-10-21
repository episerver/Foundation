using EPiServer.Shell;
using System.Collections.Generic;

namespace Foundation.Find.Cms.Models.Blocks.ProductFilters
{
    [UIDescriptorRegistration]
    public class FilterUIDescriptor : UIDescriptor<FilterBaseBlock>
    {
        public FilterUIDescriptor()
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