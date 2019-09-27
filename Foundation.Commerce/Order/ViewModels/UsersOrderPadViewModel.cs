using EPiServer.Commerce.Order;

namespace Foundation.Commerce.Order.ViewModels
{
    public class UsersOrderPadViewModel
    {
        public ICart WishCartList { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}