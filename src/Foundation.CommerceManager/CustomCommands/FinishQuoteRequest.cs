using EPiServer.Commerce.Order;
using EPiServer.Logging;
using Mediachase.BusinessFoundation;
using Mediachase.Commerce.Manager.Order.CommandHandlers.PurchaseOrderHandlers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using System;
using System.Configuration;

namespace Foundation.CommerceManager.CustomCommands
{
    public class FinishQuoteRequest : TransactionCommandHandler
    {
        public const string QuoteExpireDate = "QuoteExpireDate";
        public const string ParentOrderGroupId = "ParentOrderGroupId";
        public const string QuoteStatus = "QuoteStatus";
        public const string RequestQuotation = "RequestQuotation";
        public const string RequestQuotationFinished = "RequestQuotationFinished";
        public const string PreQuoteTotal = "PreQuoteTotal";
        public const string PreQuotePrice = "PreQuotePrice";
        public const string QuoteExpired = "QuoteExpired";
        public const string RequestQuoteStatus = "RequestQuoteStatus";

        protected override bool IsCommandEnable(IOrderGroup order, CommandParameters cp)
        {
            var flag = base.IsCommandEnable(order, cp);
            if (flag && !string.IsNullOrEmpty(order.Properties[QuoteStatus] as string))
            {
                flag = order.Properties[QuoteStatus].ToString() == RequestQuotation;
            }

            return flag;
        }

        protected override void DoCommand(IOrderGroup order, CommandParameters commandParameters)
        {
            try
            {
                var purchaseOrder = order as PurchaseOrder;
                int.TryParse(ConfigurationManager.AppSettings[QuoteExpireDate], out var quoteExpireDays);
                purchaseOrder[QuoteExpireDate] =
                    string.IsNullOrEmpty(ConfigurationManager.AppSettings[QuoteExpireDate])
                        ? DateTime.Now.AddDays(30)
                        : DateTime.Now.AddDays(quoteExpireDays);

                purchaseOrder[QuoteStatus] = RequestQuotationFinished;
                OrderStatusManager.ReleaseHoldOnOrder(purchaseOrder);
                AddNoteToOrder(purchaseOrder, "OrderNote_ChangeOrderStatusPattern", purchaseOrder.Status);
                SavePurchaseOrderChanges(purchaseOrder);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error("Failed to process request quote approve.", ex);
            }
        }
    }
}