using EPiServer.Commerce.Order;
using EPiServer.ServiceLocation;
using Foundation.Commerce;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Features.MyOrganization;
using Foundation.Features.MyOrganization.Budgeting;
using Foundation.Infrastructure;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Checkout.Payments
{
    public class BudgetPaymentGateway : AbstractPaymentGateway, IPaymentPlugin
    {
        private static Injected<IBudgetService> _budgetService;
        private static Injected<ICustomerService> _customerService;
        private static Injected<IOrdersService> _ordersService;
        private static Injected<IOrderRepository> _orderRepository;
        public IOrderGroup OrderGroup { get; set; }

        public PaymentProcessingResult ProcessPayment(IOrderGroup orderGroup, IPayment payment)
        {
            if (orderGroup == null)
            {
                return PaymentProcessingResult.CreateUnsuccessfulResult("Failed to process your payment.");
            }

            var currentOrder = orderGroup;
            var customer = _customerService.Service.GetContactViewModelById(currentOrder.CustomerId.ToString());
            if (customer == null)
            {
                return PaymentProcessingResult.CreateUnsuccessfulResult("Failed to process your payment.");
            }

            var isQuoteOrder = currentOrder.Properties[Constant.Quote.ParentOrderGroupId] != null &&
                               Convert.ToInt32(currentOrder.Properties[Constant.Quote.ParentOrderGroupId]) != 0;

            if (isQuoteOrder && customer.Role != B2BUserRoles.Approver)
            {
                return PaymentProcessingResult.CreateUnsuccessfulResult("Failed to process your payment.");
            }

            var purchaserCustomer = !isQuoteOrder ? customer : _ordersService.Service.GetPurchaserCustomer(currentOrder);
            if (AreBudgetsOnHold(purchaserCustomer))
            {
                return PaymentProcessingResult.CreateUnsuccessfulResult("Budget on hold.");
            }

            if (customer.Role == B2BUserRoles.Purchaser)
            {
                var budget =
                    _budgetService.Service.GetCustomerCurrentBudget(purchaserCustomer.Organization.OrganizationId,
                        purchaserCustomer.ContactId);
                if (budget == null || budget.RemainingBudget < payment.Amount)
                {
                    return PaymentProcessingResult.CreateUnsuccessfulResult("Insufficient budget.");
                }
            }

            if (payment.TransactionType == TransactionType.Capture.ToString())
            {
                UpdateUserBudgets(purchaserCustomer, payment.Amount);
                payment.Status = PaymentStatus.Processed.ToString();
                _orderRepository.Service.Save(currentOrder);
            }

            return PaymentProcessingResult.CreateSuccessfulResult("");
        }

        public override bool ProcessPayment(Payment payment, ref string message)
        {
            var result = ProcessPayment(OrderGroup, payment);
            message = result.Message;
            return result.IsSuccessful;
        }

        private void UpdateUserBudgets(ContactViewModel customer, decimal amount)
        {
            var budgetsToUpdate = new List<FoundationBudget>
            {
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.OrganizationId),
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.ParentOrganizationId),
                _budgetService.Service.GetCustomerCurrentBudget(customer.Organization.OrganizationId,
                    customer.ContactId)
            }.Where(x => x != null).ToList();

            if (budgetsToUpdate.All(budget => budget == null))
            {
                return;
            }

            foreach (var budget in budgetsToUpdate)
            {
                budget.SpentBudget += amount;
                budget.SaveChanges();
            }
        }

        private bool AreBudgetsOnHold(ContactViewModel customer)
        {
            if (customer?.Organization == null)
            {
                return true;
            }

            var budgetsToCheck = new List<FoundationBudget>
            {
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.OrganizationId),
                _budgetService.Service.GetCurrentOrganizationBudget(customer.Organization.ParentOrganizationId),
                _budgetService.Service.GetCustomerCurrentBudget(customer.Organization.OrganizationId,
                    customer.ContactId)
            }.Where(x => x != null);
            return budgetsToCheck.Any(budget => budget.Status.Equals(Constant.BudgetStatus.OnHold));
        }
    }
}