using EPiServer.Security;
using Mediachase.Commerce.Security;
using System.Collections.Generic;

namespace Foundation.Features.MyOrganization.QuickOrderBlock
{
    public class QuickOrderViewModel
    {
        public QuickOrderBlock CurrentBlock { get; set; }
        public List<QuickOrderProductViewModel> ProductsList { get; set; }
        public List<string> ReturnedMessages { get; set; }
        public bool HasOrganization
        {
            get
            {
                var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
                return contact?.OwnerId != null;
            }
        }

        public QuickOrderViewModel(QuickOrderBlock currentBlock)
        {
            CurrentBlock = currentBlock;
            ProductsList = new List<QuickOrderProductViewModel>();
            ReturnedMessages = new List<string>();
        }
    }
}