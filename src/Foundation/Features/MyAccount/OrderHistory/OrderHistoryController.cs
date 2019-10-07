using EPiServer;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.Services;
using Foundation.Commerce.Order.ViewModels;
using Foundation.Features.MyAccount.OrderConfirmation;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.OrderHistory
{
    [Authorize]
    public class OrderHistoryController : OrderConfirmationControllerBase<OrderHistoryPage>
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IOrderRepository _orderRepository;
        private readonly IContentLoader _contentLoader;
        private readonly ICartService _cartService;
        private readonly IOrderGroupFactory _orderGroupFactory;

        public OrderHistoryController(IAddressBookService addressBookService,
            IOrderRepository orderRepository,
            ConfirmationService confirmationService,
            ICartService cartService,
            IOrderGroupCalculator orderGroupCalculator,
            IContentLoader contentLoader,
            UrlResolver urlResolver, IOrderGroupFactory orderGroupFactory, ICustomerService customerService) :
            base(confirmationService, addressBookService, orderGroupCalculator, urlResolver, customerService)
        {
            _addressBookService = addressBookService;
            _orderRepository = orderRepository;
            _contentLoader = contentLoader;
            _cartService = cartService;
            _orderGroupFactory = orderGroupFactory;
        }

        [HttpGet]
        public ActionResult Index(OrderHistoryPage currentPage)
        {
            var purchaseOrders = _orderRepository.Load<IPurchaseOrder>(PrincipalInfo.CurrentPrincipal.GetContactId(), _cartService.DefaultCartName)
                                             .OrderByDescending(x => x.Created)
                                             .ToList();

            var viewModel = new OrderHistoryViewModel(currentPage)
            {
                CurrentContent = currentPage,
                Orders = new List<OrderViewModel>()
            };

            foreach (var purchaseOrder in purchaseOrders)
            {

                // Assume there is only one form per purchase.
                var form = purchaseOrder.GetFirstForm();
                var billingAddress = new AddressModel();
                var payment = form.Payments.FirstOrDefault();
                if (payment != null)
                {
                    billingAddress = _addressBookService.ConvertToModel(payment.BillingAddress);
                }
                var orderViewModel = new OrderViewModel
                {
                    PurchaseOrder = purchaseOrder,
                    Items = form.GetAllLineItems().Select(lineItem => new OrderHistoryItemViewModel
                    {
                        LineItem = lineItem,
                    }).GroupBy(x => x.LineItem.Code).Select(group => group.First()),
                    BillingAddress = billingAddress,
                    ShippingAddresses = new List<AddressModel>()
                };

                foreach (var orderAddress in purchaseOrder.Forms.SelectMany(x => x.Shipments).Select(s => s.ShippingAddress))
                {
                    var shippingAddress = _addressBookService.ConvertToModel(orderAddress);
                    orderViewModel.ShippingAddresses.Add(shippingAddress);
                }

                viewModel.Orders.Add(orderViewModel);
            }
            viewModel.OrderDetailsPageUrl =
             UrlResolver.Current.GetUrl(_contentLoader.Get<CommerceHomePage>(ContentReference.StartPage).OrderDetailsPage);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Detail(OrderHistoryPage currentPage, int orderid)
        {

            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderid);
            if (purchaseOrder == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            var viewModel = CreateViewModel(currentPage, purchaseOrder);
            //viewModel.OrderDetailsPageUrl =
            //UrlResolver.Current.GetUrl(_contentLoader.Get<StartPage>(ContentReference.StartPage).OrderDetailsPage);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAsPaymentPlan(int orderid, int cycleMode, int cycleLength)
        {
            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderid);
            if (purchaseOrder == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var cart = _orderRepository.Create<ICart>(Guid.NewGuid().ToString());
            cart.CopyFrom(purchaseOrder, _orderGroupFactory);
            var orderReference = _orderRepository.SaveAsPaymentPlan(cart);
            _orderRepository.Delete(cart.OrderLink);
            var paymentPlan = _orderRepository.Load<IPaymentPlan>(orderReference.OrderGroupId);
            paymentPlan.CycleMode = (PaymentPlanCycle)cycleMode;
            paymentPlan.CycleLength = cycleLength;
            paymentPlan.StartDate = DateTime.UtcNow;
            paymentPlan.IsActive = true;

            var principal = PrincipalInfo.CurrentPrincipal;
            AddNoteToOrder(paymentPlan, $"Note: New payment plan placed by {principal.Identity.Name}.", OrderNoteTypes.System, principal.GetContactId());
            paymentPlan.AdjustInventoryOrRemoveLineItems((__, _) => { });
            _orderRepository.Save(paymentPlan);

            //create first order
            orderReference = _orderRepository.SaveAsPurchaseOrder(paymentPlan);
            var newPurchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderReference.OrderGroupId);
            OrderGroupWorkflowManager.RunWorkflow((OrderGroup)newPurchaseOrder, OrderGroupWorkflowManager.CartCheckOutWorkflowName);
            var noteDetailPattern = "New purchase order placed by {0} in {1} from payment plan {2}";
            var noteDetail = string.Format(noteDetailPattern, principal.Identity.Name, "VNext site", (paymentPlan as PaymentPlan).Id);
            AddNoteToOrder(newPurchaseOrder, noteDetail, OrderNoteTypes.System, principal.GetContactId());
            _orderRepository.Save(newPurchaseOrder);

            paymentPlan.LastTransactionDate = DateTime.UtcNow;
            paymentPlan.CompletedCyclesCount++;
            _orderRepository.Save(paymentPlan);
            var homePage = _contentLoader.Get<CommerceHomePage>(ContentReference.StartPage);
            var paymentPlanPageUrl = Url.ContentUrl(homePage.PaymentPlanDetailsPage) + $"?paymentPlanId={paymentPlan.OrderLink.OrderGroupId}";
            return Redirect(paymentPlanPageUrl);
        }

        private void AddNoteToOrder(IOrderGroup order, string noteDetails, OrderNoteTypes type, Guid customerId)
        {
            if (order == null)
            {
                throw new ArgumentNullException("purchaseOrder");
            }
            var orderNote = order.CreateOrderNote();

            if (!orderNote.OrderNoteId.HasValue)
            {
                var newOrderNoteId = -1;

                if (order.Notes.Count != 0)
                {
                    newOrderNoteId = Math.Min(order.Notes.ToList().Min(n => n.OrderNoteId.Value), 0) - 1;
                }

                orderNote.OrderNoteId = newOrderNoteId;
            }

            orderNote.CustomerId = customerId;
            orderNote.Type = type.ToString();
            orderNote.Title = noteDetails.Substring(0, Math.Min(noteDetails.Length, 24)) + "...";
            orderNote.Detail = noteDetails;
            orderNote.Created = DateTime.UtcNow;
        }
    }
}