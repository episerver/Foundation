using EPiServer.Commerce.Order;

namespace Foundation.Features.Checkout.ViewModels
{
    public class UsersOrderPadViewModel
    {
        public ICart WishCartList { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}