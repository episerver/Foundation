using EPiServer.Shell;
using Foundation.Cms.Extensions;
using System.Collections.Generic;

namespace Foundation.Commerce.Extensions
{
    [UIDescriptorRegistration]
    public class DisableOpeUIDescriptor : UIDescriptor<IDisableOPE>
    {
        public DisableOpeUIDescriptor()
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
