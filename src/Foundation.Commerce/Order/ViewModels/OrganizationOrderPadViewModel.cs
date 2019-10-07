using System.Collections.Generic;

namespace Foundation.Commerce.Order.ViewModels
{
    public class OrganizationOrderPadViewModel
    {
        public List<UsersOrderPadViewModel> UsersOrderPad { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }
    }
}