using EPiServer.Personalization.VisitorGroups;

namespace Foundation.Infrastructure
{
    // CMS 13: EPiServer.Personalization.Commerce removed (no CMS 13 version).
    // CampaignVisitorGroupFilter (EPiServer.Commerce.Marketing) requires IVisitorGroupRoleRepository.
    // This no-op stub prevents DI resolution failure. Phase 4 TODO: assess if a real implementation is needed.
    public class NoOpVisitorGroupRoleRepository : IVisitorGroupRoleRepository
    {
        public bool TryGetRole(string roleName, out VisitorGroupRole role)
        {
            role = null;
            return false;
        }
    }
}
