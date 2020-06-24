using Foundation.Commerce.Customer;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.MyAccount.CreditCard
{
    /// <summary>
    /// Represent for data of credit card on the view
    /// </summary>
    public class CreditCardViewModel : ContentViewModel<CreditCardPage>
    {
        public CreditCardViewModel()
        {
        }

        public CreditCardViewModel(CreditCardPage currentPage) : base(currentPage)
        {
        }

        public CreditCardModel CreditCard { get; set; }
        public bool IsB2B { get; set; }
        public List<FoundationOrganization> Organizations { get; set; }
        public string ErrorMessage { get; set; }

        public List<FoundationOrganization> GetAllOrganizationAndSub(FoundationOrganization organizationInfo)
        {
            var result = new List<FoundationOrganization>();
            if (organizationInfo != null)
            {
                GetAllOganizationAndSub(organizationInfo, result, 0);
            }

            return result;
        }

        private void GetAllOganizationAndSub(FoundationOrganization organization, List<FoundationOrganization> list, int level)
        {
            if (organization != null)
            {
                while (level > 0)
                {
                    organization.Name = ".." + organization.Name;
                    level--;
                }

                list.Add(organization);
                if (organization.SubOrganizations.Count > 0)
                {
                    foreach (var subOrg in organization.SubOrganizations)
                    {
                        GetAllOganizationAndSub(subOrg, list, level + 1);
                    }
                }
            }
        }
    }
}