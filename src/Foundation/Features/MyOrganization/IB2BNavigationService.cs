using EPiServer.SpecializedProperties;

namespace Foundation.Features.MyOrganization
{
    public interface IB2BNavigationService
    {
        LinkItemCollection FilterB2BNavigationForCurrentUser(LinkItemCollection b2BLinks);
    }
}