using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using Foundation.Features.Checkout.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Checkout.Services
{
    public abstract class PurchaseValidation
    {
        protected readonly LocalizationService LocalizationService;

        public abstract bool ValidateModel(ModelStateDictionary modelState, CheckoutViewModel viewModel);

        protected PurchaseValidation(LocalizationService localizationService)
        {
            LocalizationService = localizationService;
        }

        public virtual bool ValidateOrderOperation(ModelStateDictionary modelState, Dictionary<ILineItem, List<ValidationIssue>> validationIssueCollections)
        {
            foreach (var validationIssue in validationIssueCollections)
            {
                foreach (var issue in validationIssue.Value)
                {
                    switch (issue)
                    {
                        case ValidationIssue.None:
                            break;
                        case ValidationIssue.CannotProcessDueToMissingOrderStatus:
                            modelState.AddModelError("", string.Format(LocalizationService.GetString("/Checkout/Payment/Errors/CannotProcessDueToMissingOrderStatus"),
                                    validationIssue.Key.Code));
                            break;
                        case ValidationIssue.RemovedDueToCodeMissing:
                        case ValidationIssue.RemovedDueToNotAvailableInMarket:
                        case ValidationIssue.RemovedDueToInactiveWarehouse:
                        case ValidationIssue.RemovedDueToMissingInventoryInformation:
                        case ValidationIssue.RemovedDueToUnavailableCatalog:
                        case ValidationIssue.RemovedDueToUnavailableItem:
                            modelState.AddModelError("", string.Format(LocalizationService.GetString("/Checkout/Payment/Errors/RemovedDueToUnavailableItem"),
                                validationIssue.Key.Code));
                            break;
                        case ValidationIssue.RemovedDueToInsufficientQuantityInInventory:
                            modelState.AddModelError("", string.Format(LocalizationService.GetString("/Checkout/Payment/Errors/RemovedDueToInsufficientQuantityInInventory"),
                                    validationIssue.Key.Code));
                            break;
                        case ValidationIssue.RemovedDueToInvalidPrice:
                            modelState.AddModelError("", string.Format(LocalizationService.GetString("/Checkout/Payment/Errors/RemovedDueToInvalidPrice"),
                                    validationIssue.Key.Code));
                            break;
                        case ValidationIssue.AdjustedQuantityByMinQuantity:
                        case ValidationIssue.AdjustedQuantityByMaxQuantity:
                        case ValidationIssue.AdjustedQuantityByBackorderQuantity:
                        case ValidationIssue.AdjustedQuantityByPreorderQuantity:
                        case ValidationIssue.AdjustedQuantityByAvailableQuantity:
                            modelState.AddModelError("", string.Format(LocalizationService.GetString("/Checkout/Payment/Errors/AdjustedQuantity"),
                                    validationIssue.Key.Code));
                            break;
                        case ValidationIssue.PlacedPricedChanged:
                            modelState.AddModelError("", string.Format(LocalizationService.GetString("/Checkout/Payment/Errors/PlacedPricedChanged"),
                                    validationIssue.Key.Code));
                            break;
                        default:
                            modelState.AddModelError("", string.Format(LocalizationService.GetString("/Checkout/Payment/Errors/PreProcessingFailure"),
                                    validationIssue.Key.Code));
                            break;
                    }
                }
            }

            return modelState.IsValid;
        }

        protected bool ValidateShippingMethods(ModelStateDictionary modelState, CheckoutViewModel checkoutViewModel)
        {
            if (checkoutViewModel.Shipments.Any(s => s.ShippingMethodId == Guid.Empty))
            {
                modelState.AddModelError("Shipment.ShippingMethod", LocalizationService.GetString("/Shared/Address/Form/Empty/ShippingMethod"));
            }

            return modelState.IsValid;
        }
    }
}