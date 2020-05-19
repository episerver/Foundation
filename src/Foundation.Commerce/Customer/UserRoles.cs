using System.ComponentModel;

namespace Foundation.Commerce.Customer
{
    public enum B2BUserRoles
    {
        Admin,
        Approver,
        Purchaser,
        None
    }

    public enum B2BPermissions
    {
        Default,

        [Description("Full access")]
        FullAcess,

        [Description("View Orders")]
        ViewOrders,

        [Description("View Invoices")]
        ViewInvoices,

        [Description("Can order over budget")]
        OrderOverBudget,

        [Description("Can order over budget without approval")]
        OrderOverBudgetWithoutApproval,

        [Description("Default approver if none is assigned to a user")]
        DefaultApproverIfNotAssigned,

        [Description("Over budget orders require approval")]
        OrderBudgetOrdersRequireApproval,

        [Description("Cannot be assigned as an approver")]
        CannotBeAssignedAsAnApprover,

        [Description("All orders require approval")]
        AllOrdersRequireApproval,

        [Description("Can approve requisitions")]
        ApproveRequisitions,

        [Description("Can only place requisition requests")]
        OnlyPlaceRequisitionRequests,

        [Description("Can place orders")]
        PlaceOrders,
    }
}