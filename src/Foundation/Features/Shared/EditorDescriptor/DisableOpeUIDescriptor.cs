using EPiServer.Shell;
using System.Collections.Generic;

namespace Foundation.Features.Shared.EditorDescriptors
{
    public interface IDisableOPE
    {
    }

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