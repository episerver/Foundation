using EPiServer.Commerce.Order;
using Mediachase.BusinessFoundation;
using Mediachase.Commerce.Engine;
using Mediachase.Commerce.Manager.Apps.Core.CommandHandlers.Common;
using Mediachase.Commerce.Manager.Apps_Code.Order;
using Mediachase.Commerce.Manager.Order.CommandHandlers.PurchaseOrderHandlers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Web.Console.BaseClasses;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.CommerceManager.CustomCommands
{

    public class B2BSaveChangesHandler : SaveChangesHandler
    {
        public const string RequestQuotation = "RequestQuotation";
        public const string RequestQuotationFinished = "RequestQuotationFinished";

        protected override void DoCommand(IOrderGroup order, CommandParameters cp)
        {
            Mediachase.Ibn.Web.UI.CHelper.RequireDataBind();
            var purchaseOrder = order as PurchaseOrder;
            var workflowResults = OrderGroupWorkflowManager.RunWorkflow(purchaseOrder, "SaveChangesWorkflow",
                false,
                //false,
                new Dictionary<string, object>
                {
                    {
                        "PreventProcessPayment",
                        !string.IsNullOrEmpty(order.Properties["QuoteStatus"] as string) &&
                        (order.Properties["QuoteStatus"].ToString() == RequestQuotation||
                        order.Properties["QuoteStatus"].ToString() == RequestQuotationFinished)
                    }
                });

            if (workflowResults.Status != WorkflowStatus.Completed)
            {
                var msg = "Unknow error";
                if (workflowResults.Exception != null)
                {
                    msg = workflowResults.Exception.Message;
                }

                ErrorManager.GenerateError(msg);
            }
            else
            {
                WriteOrderChangeNotes(purchaseOrder);
                SavePurchaseOrderChanges(purchaseOrder);
                OrderHelper.ExitPurchaseOrderFromEditMode(purchaseOrder.OrderGroupId);
            }

            var warnings = OrderGroupWorkflowManager.GetWarningsFromWorkflowResult(workflowResults);
            if (warnings.Any())
            {
                CommandHandlerHelper.ShowStatusMessage(string.Join(", ", warnings), CommandManager);
            }
        }
    }
}