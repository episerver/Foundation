using EPiServer.SpecializedProperties;

namespace Foundation.Commerce.Customer.Services
{
    public interface IB2BNavigationService
    {
        LinkItemCollection FilterB2BNavigationForCurrentUser(LinkItemCollection b2BLinks);
    }
}