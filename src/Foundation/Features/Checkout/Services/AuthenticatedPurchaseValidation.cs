using EPiServer.Framework.Localization;
using Foundation.Features.Checkout.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Checkout.Services
{
    public class AuthenticatedPurchaseValidation : PurchaseValidation
    {
        public AuthenticatedPurchaseValidation(LocalizationService localizationService) : base(localizationService)
        {
        }

        public override bool ValidateModel(ModelStateDictionary modelState, CheckoutViewModel viewModel)
        {
            RemoveModelState(modelState);

            return
                ValidateBillingAddress(modelState, viewModel) &&
                ValidateShippingAddresses(modelState, viewModel) &&
                ValidateShippingMethods(modelState, viewModel);
        }

        private bool ValidateBillingAddress(ModelStateDictionary modelState, CheckoutViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.BillingAddress.AddressId))
            {
                modelState.AddModelError("BillingAddress.AddressId", LocalizationService.GetString("/Shared/Address/Form/Empty/BillingAddress"));
            }

            return modelState.IsValid;
        }

        private bool ValidateShippingAddresses(ModelStateDictionary modelState, CheckoutViewModel viewModel)
        {
            if (viewModel.Shipments.Any(a => string.IsNullOrEmpty(a.Address.AddressId)))
            {
                modelState.AddModelError("ShippingAddress.AddressId", LocalizationService.GetString("/Shared/Address/Form/Empty/ShippingAddress"));
            }

            return modelState.IsValid;
        }

        private void RemoveModelState(ModelStateDictionary modelState)
        {
            foreach (var state in modelState.Where(x => x.Key.StartsWith("BillingAddress")).ToArray())
            {
                modelState.Remove(state);
            }

            foreach (var state in modelState.Where(x => x.Key.StartsWith("Shipments")).ToArray())
            {
                modelState.Remove(state);
            }
        }
    }
}