using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Checkout.Payments;
using Foundation.Features.Checkout.Services;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization.Budgeting;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Infrastructure;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.Features.Checkout.ViewModels
{
    public class CheckoutViewModelFactory
    {
        private readonly LocalizationService _localizationService;
        private readonly PaymentMethodViewModelFactory _paymentMethodViewModelFactory;
        private readonly IAddressBookService _addressBookService;
        private readonly UrlResolver _urlResolver;
        private readonly ServiceAccessor<HttpContextBase> _httpContextAccessor;
        private readonly ShipmentViewModelFactory _shipmentViewModelFactory;
        private readonly ICustomerService _customerService;
        private readonly IOrganizationService _organizationService;
        private readonly IBudgetService _budgetService;
        private readonly ICustomerService _customerContext;

        public CheckoutViewModelFactory(
            LocalizationService localizationService,
            PaymentMethodViewModelFactory paymentMethodViewModelFactory,
            IAddressBookService addressBookService,
            UrlResolver urlResolver,
            ServiceAccessor<HttpContextBase> httpContextAccessor,
            ShipmentViewModelFactory shipmentViewModelFactory,
            ICustomerService customerService,
            IOrganizationService organizationService,
            IBudgetService budgetService,
            ICustomerService customerContext)
        {
            _localizationService = localizationService;
            _paymentMethodViewModelFactory = paymentMethodViewModelFactory;
            _addressBookService = addressBookService;
            _urlResolver = urlResolver;
            _httpContextAccessor = httpContextAccessor;
            _shipmentViewModelFactory = shipmentViewModelFactory;
            _customerService = customerService;
            _organizationService = organizationService;
            _budgetService = budgetService;
            _customerContext = customerContext;
        }

        public virtual CheckoutViewModel CreateCheckoutViewModel(ICart cart, CheckoutPage currentPage, IPaymentMethod paymentOption = null)
        {
            if (cart == null)
            {
                return CreateEmptyCheckoutViewModel(currentPage);
            }

            var currentShippingAddressId = cart.GetFirstShipment()?.ShippingAddress?.Id;
            var currentBillingAdressId = cart.GetFirstForm().Payments.FirstOrDefault()?.BillingAddress?.Id;

            var shipments = _shipmentViewModelFactory.CreateShipmentsViewModel(cart).ToList();
            var useShippingAddressForBilling = shipments.Count == 1;

            var viewModel = new CheckoutViewModel(currentPage)
            {
                Shipments = shipments,
                BillingAddress = CreateBillingAddressModel(currentBillingAdressId),
                UseShippingingAddressForBilling = useShippingAddressForBilling,
                AppliedCouponCodes = cart.GetFirstForm().CouponCodes.Distinct(),
                AvailableAddresses = new List<AddressModel>(),
                ReferrerUrl = GetReferrerUrl(),
                Currency = cart.Currency,
                CurrentCustomer = _customerService.GetCurrentContactViewModel(),
                IsOnHoldBudget = CheckForOnHoldBudgets(),
                Payment = paymentOption,
                PaymentPlanSetting = new PaymentPlanSetting()
                {
                    CycleMode = Mediachase.Commerce.Orders.PaymentPlanCycle.None,
                    IsActive = false,
                    StartDate = DateTime.UtcNow
                },
            };

            UpdatePayments(viewModel, cart);

            var availableAddresses = GetAvailableAddresses();

            if (availableAddresses.Any())
            {
                //viewModel.AvailableAddresses.Add(new AddressModel { Name = _localizationService.GetString("/Checkout/MultiShipment/SelectAddress"), AddressId = "" });

                foreach (var address in availableAddresses)
                {
                    viewModel.AvailableAddresses.Add(address);
                }
            }
            else
            {
                viewModel.AvailableAddresses.Add(new AddressModel { Name = _localizationService.GetString("/Checkout/MultiShipment/NoAddressFound"), AddressId = "" });
            }

            SetDefaultShipmentAddress(viewModel, currentShippingAddressId);

            _addressBookService.LoadAddress(viewModel.BillingAddress);
            PopulateCountryAndRegions(viewModel);

            return viewModel;
        }

        private void SetDefaultShipmentAddress(CheckoutViewModel viewModel, string shippingAddressId)
        {
            if (viewModel.Shipments.Count > 0)
            {
                foreach (var shipment in viewModel.Shipments)
                {
                    if (shipment.ShippingAddressType == 1)
                    {
                        viewModel.Shipments[0].Address = viewModel.AvailableAddresses.SingleOrDefault(x => x.AddressId == shippingAddressId) ??
                                                viewModel.AvailableAddresses.SingleOrDefault(x => x.ShippingDefault) ??
                                                viewModel.BillingAddress;
                    }
                }
            }
        }

        private IList<AddressModel> GetAvailableAddresses()
        {
            var addresses = _addressBookService.List();
            foreach (var address in addresses.Where(x => string.IsNullOrEmpty(x.Name)))
            {
                address.Name = _localizationService.GetString("/Shared/Address/DefaultAddressName");
            }

            return addresses;
        }

        private CheckoutViewModel CreateEmptyCheckoutViewModel(CheckoutPage currentPage)
        {
            return new CheckoutViewModel(currentPage)
            {
                Shipments = new List<ShipmentViewModel>(),
                AppliedCouponCodes = new List<string>(),
                AvailableAddresses = new List<AddressModel>(),
                PaymentMethodViewModels = Enumerable.Empty<PaymentMethodViewModel>(),
                UseShippingingAddressForBilling = true
            };
        }

        private void PopulateCountryAndRegions(CheckoutViewModel viewModel)
        {
            foreach (var shipment in viewModel.Shipments)
            {
                _addressBookService.LoadCountriesAndRegionsForAddress(shipment.Address);
            }
        }

        private void UpdatePayments(CheckoutViewModel viewModel, ICart cart)
        {
            viewModel.PaymentMethodViewModels = _paymentMethodViewModelFactory.GetPaymentMethodViewModels();
            var methodViewModels = viewModel.PaymentMethodViewModels.ToList();
            if (!methodViewModels.Any())
            {
                return;
            }

            var defaultPaymentMethod = methodViewModels.FirstOrDefault(p => p.IsDefault) ?? methodViewModels.First();
            var selectedPaymentMethod = viewModel.Payment == null ?
                defaultPaymentMethod :
                methodViewModels.Single(p => p.SystemKeyword == viewModel.Payment.SystemKeyword);

            viewModel.Payment = selectedPaymentMethod.PaymentOption;
            viewModel.Payments = methodViewModels.Where(x => cart.GetFirstForm().Payments.Any(p => p.PaymentMethodId == x.PaymentMethodId))
                .Select(x => x.PaymentOption)
                .OfType<PaymentOptionBase>()
                .ToList();

            foreach (var viewModelPayment in viewModel.Payments)
            {
                viewModelPayment.Amount =
                    new Money(
                        cart.GetFirstForm().Payments
                            .FirstOrDefault(p => p.PaymentMethodId == viewModelPayment.PaymentMethodId)?.Amount ?? 0,
                        cart.Currency);
            }

            if (!cart.GetFirstForm().
                Payments.Any())
            {
                return;
            }

            var method = methodViewModels.FirstOrDefault(
                x => x.PaymentMethodId == cart.GetFirstForm().
                         Payments.FirstOrDefault().
                         PaymentMethodId);
            if (method == null)
            {
                return;
            }

            viewModel.SelectedPayment = method.Description;
            var payment = cart.GetFirstForm().
                Payments.FirstOrDefault();
            var creditCardPayment = payment as ICreditCardPayment;
            if (creditCardPayment != null)
            {
                viewModel.SelectedPayment +=
                    $" - ({creditCardPayment.CreditCardNumber.Substring(creditCardPayment.CreditCardNumber.Length - 4)})";
            }
        }

        private AddressModel CreateBillingAddressModel(string currentBillingAdressId)
        {
            if (string.IsNullOrEmpty(currentBillingAdressId))
            {
                var preferredBillingAddress = _addressBookService.GetPreferredBillingAddress();

                return new AddressModel
                {
                    AddressId = preferredBillingAddress?.Name,
                    Name = preferredBillingAddress != null ? preferredBillingAddress.Name : Guid.NewGuid().ToString(),
                };
            }
            else
            {
                return new AddressModel
                {
                    AddressId = currentBillingAdressId,
                    Name = currentBillingAdressId,
                };
            }
        }

        private string GetReferrerUrl()
        {
            var httpContext = _httpContextAccessor();
            if (httpContext.Request.UrlReferrer != null &&
                httpContext.Request.UrlReferrer.Host.Equals(httpContext.Request.Url.Host, StringComparison.OrdinalIgnoreCase))
            {
                return httpContext.Request.UrlReferrer.ToString();
            }

            return _urlResolver.GetUrl(ContentReference.StartPage);
        }

        private bool CheckForOnHoldBudgets()
        {
            var currentCustomer = _customerContext.GetContactById(_customerContext.CurrentContactId.ToString());
            if (currentCustomer?.Contact.ContactOrganization != null)
            {
                var subOrganizationId = new Guid(currentCustomer.Contact.ContactOrganization.PrimaryKeyId.Value.ToString());

                var purchaserBudget = _budgetService.GetCustomerCurrentBudget(subOrganizationId, currentCustomer.ContactId);
                if (purchaserBudget != null)
                {
                    if (purchaserBudget.Status.Equals(Constant.BudgetStatus.OnHold))
                    {
                        return true;
                    }
                }

                var suborganizationCurrentBudget = _budgetService.GetCurrentOrganizationBudget(subOrganizationId);
                if (suborganizationCurrentBudget != null)
                {
                    if (suborganizationCurrentBudget.Status.Equals(Constant.BudgetStatus.OnHold))
                    {
                        return true;
                    }
                }

                var organizationCurrentBudget = _budgetService.GetCurrentOrganizationBudget(_organizationService.GetSubOrganizationById(subOrganizationId.ToString()).ParentOrganizationId);
                if (organizationCurrentBudget != null)
                {
                    if (organizationCurrentBudget.Status.Equals(Constant.BudgetStatus.OnHold))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}