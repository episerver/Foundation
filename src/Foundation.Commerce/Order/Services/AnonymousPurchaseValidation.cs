using EPiServer.Framework.Localization;
using Foundation.Commerce.Order.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Commerce.Order.Services
{
    public class AnonymousPurchaseValidation : PurchaseValidation
    {
        public AnonymousPurchaseValidation(LocalizationService localizationService) : base(localizationService)
        {
        }

        public override bool ValidateModel(ModelStateDictionary modelState, CheckoutViewModel viewModel)
        {
            return
                ValidateBillingAddress(modelState, viewModel) &&
                ValidateShippingMethods(modelState, viewModel);
        }

        private bool ValidateBillingAddress(ModelStateDictionary modelState, CheckoutViewModel viewModel)
        {
            if (viewModel.UseShippingingAddressForBilling)
            {
                foreach (var state in modelState.Where(x => x.Key.StartsWith("Shipments")).ToArray())
                {
                    modelState.Remove(state);
                }
            }

            if (string.IsNullOrEmpty(viewModel.BillingAddress.Email))
            {
                modelState.AddModelError("BillingAddress.Email", LocalizationService.GetString("/Shared/Address/Form/Empty/Email"));
            }

            return modelState.IsValid;
        }
    }
}