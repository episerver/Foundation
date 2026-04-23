namespace Foundation.Features.Shared.Components.Money
{
    public class MoneyViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(decimal amount, Currency currency)
        {
            var money = new Mediachase.Commerce.Money(amount, currency);
            return View("~/Features/Shared/Components/Money/Default.cshtml", money);
        }
    }
}
