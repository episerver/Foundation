using Foundation.Commerce.Customer;
using System;

namespace Foundation.Features.MyOrganization.Budgeting
{
    public class BudgetViewModel
    {
        public BudgetViewModel(FoundationBudget budget)
        {
            StartDate = budget.StartDate;
            DueDate = budget.DueDate;
            Amount = budget.Amount;
            IsActive = budget.IsActive;
            OrganizationId = budget.OrganizationId;
            ContactId = budget.ContactId;
            BudgetId = budget.BudgetId;
            Currency = budget.Currency;
            Status = budget.Status;
            PurchaserName = budget.PurchaserName;
            SpentBudget = budget.SpentBudget;
            LockAmount = budget.LockAmount;
            RemainingBudget = budget.RemainingBudget;
            UnAllocatedAmount = budget.UnallocatedBudget;
        }

        public BudgetViewModel()
        {
        }

        public int BudgetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public decimal UnAllocatedAmount { get; set; }
        public decimal LockAmount { get; set; }
        public decimal SpentBudget { get; set; }
        public decimal RemainingBudget { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsCurrentBudget { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string PurchaserName { get; set; }
        public Guid ContactId { get; set; }
    }
}