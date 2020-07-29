using EPiServer.Commerce.Order;
using EPiServer.Logging;
using Foundation.Commerce;
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
        protected override bool IsCommandEnable(IOrderGroup order, CommandParameters cp)
        {
            var flag = base.IsCommandEnable(order, cp);
            if (flag && !string.IsNullOrEmpty(order.Properties[Constant.Quote.QuoteStatus] as string))
            {
                flag = order.Properties[Constant.Quote.QuoteStatus].ToString() == Constant.Quote.RequestQuotation;
            }

            return flag;
        }

        protected override void DoCommand(IOrderGroup order, CommandParameters commandParameters)
        {
            try
            {
                var purchaseOrder = order as PurchaseOrder;
                int.TryParse(ConfigurationManager.AppSettings[Constant.Quote.QuoteExpireDate], out var quoteExpireDays);
                purchaseOrder[Constant.Quote.QuoteExpireDate] =
                    string.IsNullOrEmpty(ConfigurationManager.AppSettings[Constant.Quote.QuoteExpireDate])
                        ? DateTime.Now.AddDays(30)
                        : DateTime.Now.AddDays(quoteExpireDays);

                purchaseOrder[Constant.Quote.QuoteStatus] = Constant.Quote.RequestQuotationFinished;
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