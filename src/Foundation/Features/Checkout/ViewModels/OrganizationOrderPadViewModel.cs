using System.Collections.Generic;

namespace Foundation.Features.Checkout.ViewModels
{
    public class OrganizationOrderPadViewModel
    {
        public List<UsersOrderPadViewModel> UsersOrderPad { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationId { get; set; }
    }
}