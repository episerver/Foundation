﻿using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Mail;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Exceptions;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Security;
using Mediachase.Web.Console.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Commerce.Order.Services
{
    public class CheckoutService
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly IOrderGroupFactory _orderGroupFactory;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IOrderRepository _orderRepository;
        private readonly IContentRepository _contentRepository;
        private readonly CustomerContext _customerContext;
        private readonly LocalizationService _localizationService;
        private readonly IMailService _mailService;
        private readonly IPromotionEngine _promotionEngine;
        private readonly ILogger _log = LogManager.GetLogger(typeof(CheckoutService));
        private readonly ILoyaltyService _loyaltyService;


        public AuthenticatedPurchaseValidation AuthenticatedPurchaseValidation { get; private set; }
        public AnonymousPurchaseValidation AnonymousPurchaseValidation { get; private set; }
        public CheckoutAddressHandling CheckoutAddressHandling { get; private set; }

        public CheckoutService(
            IAddressBookService addressBookService,
            IOrderGroupFactory orderGroupFactory,
            IOrderGroupCalculator orderGroupCalculator,
            IPaymentProcessor paymentProcessor,
            IOrderRepository orderRepository,
            IContentRepository contentRepository,
            LocalizationService localizationService,
            IMailService mailService,
            IPromotionEngine promotionEngine,
            ILoyaltyService loyaltyService)
        {
            _addressBookService = addressBookService;
            _orderGroupFactory = orderGroupFactory;
            _orderGroupCalculator = orderGroupCalculator;
            _paymentProcessor = paymentProcessor;
            _orderRepository = orderRepository;
            _contentRepository = contentRepository;
            _customerContext = CustomerContext.Current;
            _localizationService = localizationService;
            _mailService = mailService;
            _promotionEngine = promotionEngine;
            _loyaltyService = loyaltyService;

            AuthenticatedPurchaseValidation = new AuthenticatedPurchaseValidation(_localizationService);
            AnonymousPurchaseValidation = new AnonymousPurchaseValidation(_localizationService);
            CheckoutAddressHandling = new CheckoutAddressHandling(_addressBookService);
        }

        public virtual void UpdateShippingMethods(ICart cart, IList<ShipmentViewModel> shipmentViewModels)
        {
            var index = 0;
            foreach (var shipment in cart.GetFirstForm().Shipments)
            {
                shipment.ShippingMethodId = shipmentViewModels[index++].ShippingMethodId;
            }
        }

        public virtual void UpdateShippingAddresses(ICart cart, CheckoutViewModel viewModel)
        {
            var shipments = cart.GetFirstForm().Shipments;
            for (var index = 0; index < shipments.Count; index++)
            {
                shipments.ElementAt(index).ShippingAddress = _addressBookService.ConvertToAddress(viewModel.Shipments[index].Address, cart);
            }
        }

        public virtual void ChangeAddress(ICart cart, CheckoutViewModel viewModel, UpdateAddressViewModel updateAddressViewModel)
        {
            if (updateAddressViewModel.AddressType == AddressType.Billing)
            {
                foreach (var payment in cart.GetFirstForm().Payments)
                {
                    payment.BillingAddress = _addressBookService.ConvertToAddress(viewModel.BillingAddress, cart);
                }
            }
            else
            {
                var shipments = cart.GetFirstForm().Shipments;
                shipments.ElementAt(updateAddressViewModel.ShippingAddressIndex).ShippingAddress =
                        _addressBookService.ConvertToAddress(viewModel.Shipments[updateAddressViewModel.ShippingAddressIndex].Address, cart);

            }
        }

        /// <summary>
        /// Update payment plan information
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="viewModel"></param>
        public virtual void UpdatePaymentPlan(ICart cart, CheckoutViewModel viewModel)
        {
            if (viewModel.IsUsePaymentPlan)
            {
                cart.Properties["IsUsePaymentPlan"] = true;
                cart.Properties["PaymentPlanSetting"] = viewModel.PaymentPlanSetting;
            }
            else
            {
                cart.Properties["IsUsePaymentPlan"] = false;
            }
        }

        public virtual void ApplyDiscounts(ICart cart) => cart.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());

        public virtual void CreateAndAddPaymentToCart(ICart cart, CheckoutViewModel viewModel)
        {
            var total = viewModel.OrderSummary.PaymentTotal;
            var paymentMethod = viewModel.Payment;
            if (paymentMethod == null)
            {
                return;
            }

            var payment = cart.GetFirstForm().Payments.FirstOrDefault(x => x.PaymentMethodId == paymentMethod.PaymentMethodId);
            if (payment == null)
            {
                payment = paymentMethod.CreatePayment(total, cart);
                cart.AddPayment(payment, _orderGroupFactory);
            }
            else
            {
                payment.Amount = viewModel.OrderSummary.PaymentTotal;
            }
        }

        public virtual void RemovePaymentFromCart(ICart cart, CheckoutViewModel viewModel)
        {
            var paymentMethod = viewModel.Payment;
            if (paymentMethod == null)
            {
                return;
            }
            var payment = cart.GetFirstForm().Payments.FirstOrDefault(x => x.PaymentMethodId == paymentMethod.PaymentMethodId);
            cart.GetFirstForm().Payments.Remove(payment);
        }

        public virtual IPurchaseOrder PlaceOrder(ICart cart, ModelStateDictionary modelState, CheckoutViewModel checkoutViewModel)
        {
            try
            {

                if (cart.Properties[Constant.Quote.ParentOrderGroupId] != null)
                {
                    var orderLink = int.Parse(cart.Properties[Constant.Quote.ParentOrderGroupId].ToString());
                    if (orderLink != 0)
                    {
                        var quoteOrder = _orderRepository.Load<IPurchaseOrder>(orderLink);
                        if (quoteOrder.Properties[Constant.Quote.QuoteStatus] != null)
                        {
                            checkoutViewModel.QuoteStatus = quoteOrder.Properties[Constant.Quote.QuoteStatus].ToString();
                            if (quoteOrder.Properties[Constant.Quote.QuoteStatus].ToString().Equals(Constant.Quote.RequestQuotationFinished))
                            {
                                DateTime.TryParse(quoteOrder.Properties[Constant.Quote.QuoteExpireDate].ToString(),
                                    out var quoteExpireDate);
                                if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
                                {
                                    _orderRepository.Delete(cart.OrderLink);
                                    _orderRepository.Delete(quoteOrder.OrderLink);
                                    throw new InvalidOperationException("Quote Expired");
                                }
                            }
                        }
                    }
                }
                cart.ProcessPayments(_paymentProcessor, _orderGroupCalculator);

                var processedPayments = cart.GetFirstForm().Payments.Where(x => x.Status.Equals(PaymentStatus.Processed.ToString()));
                if (!processedPayments.Any())
                {
                    // Return null in case there is no payment was processed.
                    return null;
                }

                var totalProcessedAmount = processedPayments.Sum(x => x.Amount);
                if (totalProcessedAmount != cart.GetTotal(_orderGroupCalculator).Amount)
                {
                    throw new InvalidOperationException("Wrong amount");
                }

                var orderReference = (cart.Properties["IsUsePaymentPlan"] != null && cart.Properties["IsUsePaymentPlan"].Equals(true)) ? SaveAsPaymentPlan(cart) : _orderRepository.SaveAsPurchaseOrder(cart);
                var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderReference.OrderGroupId);
                _orderRepository.Delete(cart.OrderLink);

                cart.AdjustInventoryOrRemoveLineItems((item, validationIssue) => { });

                //Loyalty Program: Add Points and Number of orders
                _loyaltyService.AddNumberOfOrders();

                return purchaseOrder;
            }
            catch (PaymentException ex)
            {
                modelState.AddModelError("", _localizationService.GetString("/Checkout/Payment/Errors/ProcessingPaymentFailure") + ex.Message);
            }

            return null;
        }

        public virtual bool SendConfirmation(CheckoutViewModel viewModel, IPurchaseOrder purchaseOrder)
        {
            var startpage = _contentRepository.Get<CommerceHomePage>(ContentReference.StartPage);
            var sendOrderConfirmationMail = startpage.SendOrderConfirmationMail;
            if (sendOrderConfirmationMail)
            {
                var queryCollection = new NameValueCollection
                {
                    {"contactId", _customerContext.CurrentContactId.ToString()},
                    {"orderNumber", purchaseOrder.OrderLink.OrderGroupId.ToString(CultureInfo.CurrentCulture)}
                };

                try
                {
                    _mailService.Send(startpage.OrderConfirmationMail, queryCollection, purchaseOrder.GetFirstForm().Payments.FirstOrDefault().BillingAddress.Email, startpage.Language.Name);
                }
                catch (Exception e)
                {
                    _log.Warning(string.Format("Unable to send purchase receipt to '{0}'.", purchaseOrder.GetFirstForm().Payments.FirstOrDefault().BillingAddress.Email), e);
                    return false;
                }
            }
            return true;
        }

        public virtual string BuildRedirectionUrl(CheckoutViewModel checkoutViewModel, IPurchaseOrder purchaseOrder, bool confirmationSentSuccessfully)
        {
            var queryCollection = new NameValueCollection
            {
                {"contactId", _customerContext.CurrentContactId.ToString()},
                {"orderNumber", purchaseOrder.OrderLink.OrderGroupId.ToString(CultureInfo.CurrentCulture)}
            };

            if (!confirmationSentSuccessfully)
            {
                queryCollection.Add("notificationMessage", string.Format(_localizationService.GetString("/OrderConfirmationMail/ErrorMessages/SmtpFailure"), checkoutViewModel.BillingAddress?.Email));
            }

            var confirmationPage = checkoutViewModel.CommerceHomePage?.OrderConfirmationPage ?? ContentReference.EmptyReference;
            if (ContentReference.IsNullOrEmpty(confirmationPage))
            {
                return null;
            }
            return new UrlBuilder(UrlResolver.Current.GetUrl(confirmationPage)) { QueryCollection = queryCollection }.ToString();
        }

        public void ProcessPaymentCancel(CheckoutViewModel viewModel, TempDataDictionary tempData, ControllerContext controlerContext)
        {
            var message = tempData["message"] != null ? tempData["message"].ToString() : controlerContext.HttpContext.Request.QueryString["message"];
            if (!string.IsNullOrEmpty(message))
            {
                viewModel.Message = message;
            }
        }

        #region Payment Plan

        /// <summary>
        /// Save cart as payment plan
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        private OrderReference SaveAsPaymentPlan(ICart cart)
        {
            var orderReference = _orderRepository.SaveAsPaymentPlan(cart);
            var paymentPlanSetting = cart.Properties["PaymentPlanSetting"] as PaymentPlanSetting;

            IPaymentPlan _paymentPlan;
            _paymentPlan = _orderRepository.Load<IPaymentPlan>(orderReference.OrderGroupId);
            _paymentPlan.CycleMode = PaymentPlanCycle.Days;
            _paymentPlan.CycleLength = paymentPlanSetting.CycleLength;
            _paymentPlan.StartDate = DateTime.Now.AddDays(paymentPlanSetting.CycleLength);
            _paymentPlan.EndDate = paymentPlanSetting.EndDate;
            _paymentPlan.IsActive = paymentPlanSetting.IsActive;

            var principal = PrincipalInfo.CurrentPrincipal;
            AddNoteToCart(_paymentPlan, $"Note: New payment plan placed by {principal.Identity.Name} in 'vnext site'.", OrderNoteTypes.System.ToString(), principal.GetContactId());

            _orderRepository.Save(_paymentPlan);

            _paymentPlan.AdjustInventoryOrRemoveLineItems((item, validationIssue) => { });
            _orderRepository.Save(_paymentPlan);

            //create first order
            orderReference = _orderRepository.SaveAsPurchaseOrder(_paymentPlan);
            var purchaseOrder = _orderRepository.Load(orderReference);
            OrderGroupWorkflowManager.RunWorkflow((OrderGroup)purchaseOrder, OrderGroupWorkflowManager.CartCheckOutWorkflowName);
            var noteDetailPattern = "New purchase order placed by {0} in {1} from payment plan {2}";
            var noteDetail = string.Format(noteDetailPattern, ManagementHelper.GetUserName(PrincipalInfo.CurrentPrincipal.GetContactId()), "VNext site", (_paymentPlan as PaymentPlan).Id);
            AddNoteToPurchaseOrder(purchaseOrder as IPurchaseOrder, noteDetail, OrderNoteTypes.System, PrincipalInfo.CurrentPrincipal.GetContactId());
            _orderRepository.Save(purchaseOrder);

            _paymentPlan.LastTransactionDate = DateTime.UtcNow;
            _paymentPlan.CompletedCyclesCount++;
            _orderRepository.Save(_paymentPlan);

            return orderReference;
        }

        /// <summary>
        /// Add note to purchase order
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <param name="noteDetails"></param>
        /// <param name="type"></param>
        /// <param name="customerId"></param>
        private void AddNoteToPurchaseOrder(IPurchaseOrder purchaseOrder, string noteDetails, OrderNoteTypes type, Guid customerId)
        {
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException("purchaseOrder");
            }
            var orderNote = purchaseOrder.CreateOrderNote();

            if (!orderNote.OrderNoteId.HasValue)
            {
                var newOrderNoteId = -1;

                if (purchaseOrder.Notes.Count != 0)
                {
                    newOrderNoteId = Math.Min(purchaseOrder.Notes.ToList().Min(n => n.OrderNoteId.Value), 0) - 1;
                }

                orderNote.OrderNoteId = newOrderNoteId;
            }

            orderNote.CustomerId = customerId;
            orderNote.Type = type.ToString();
            orderNote.Title = noteDetails.Substring(0, Math.Min(noteDetails.Length, 24)) + "...";
            orderNote.Detail = noteDetails;
            orderNote.Created = DateTime.UtcNow;
        }

        /// <summary>
        /// Add note to cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="noteDetails"></param>
        /// <param name="type"></param>
        /// <param name="originator"></param>
        private void AddNoteToCart(IOrderGroup cart, string noteDetails, string type, Guid originator)
        {
            var note = new OrderNote
            {
                CustomerId = originator,
                Type = type,
                Title = noteDetails.Substring(0, Math.Min(noteDetails.Length, 24)) + "...",
                Detail = noteDetails,
                Created = DateTime.UtcNow
            };
            cart.Notes.Add(note);
        }
        #endregion


    }
}