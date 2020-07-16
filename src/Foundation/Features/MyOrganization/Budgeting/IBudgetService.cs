using Foundation.Commerce.Customer;
using System;
using System.Collections.Generic;

namespace Foundation.Features.MyOrganization.Budgeting
{
    public interface IBudgetService
    {
        void CreateNewBudget(BudgetViewModel budgetModel);
        void UpdateBudget(BudgetViewModel budgetModel);
        bool IsTimeOverlapped(DateTime startDate, DateTime dueDateTime, Guid organizationGuid);
        bool HasEnoughAmount(Guid organizationGuid, decimal amount, DateTime startDateTime, DateTime finishDateTime);
        bool HasEnoughAmountOnCurrentBudget(Guid organizationGuid, decimal amount);
        bool IsSuborganizationValidTimeSlice(DateTime startDateTime, DateTime finishDateTime, Guid organizationGuid);
        FoundationBudget GetBudgetByTimeLine(Guid organizationId, DateTime startDate, DateTime endDate);
        bool LockOrganizationAmount(DateTime startDate, DateTime endDate, Guid guid, decimal amount);
        bool UnLockOrganizationAmount(DateTime startDate, DateTime endDate, Guid guid, decimal amount);
        bool CheckAmount(Guid organizationGuid, decimal newLockAmount, decimal unlockAmount);
        bool LockUserAmount(DateTime startDate, DateTime endDate, Guid organizationGuid, Guid userGuid, decimal amount);

        bool CheckAmountByTimeLine(Guid organizationGuid, decimal newLockAmount, DateTime startDateTime, DateTime finishDateTime);

        bool ValidateSuborganizationNewAmount(Guid organizationGuid, Guid parentOrganizationId, decimal newLockAmount);

        List<FoundationBudget> GetActiveUserBudgets(Guid contactId);
        List<FoundationBudget> GetActiveOrganizationBudgets(Guid organizationId);
        List<FoundationBudget> GetUserBudgets(Guid contactId);
        List<FoundationBudget> GetOrganizationBudgets(Guid organizationId);
        List<FoundationBudget> GetAllBudgets();
        FoundationBudget GetNewBudget();
        FoundationBudget GetBudgetById(int budgetId);
        FoundationBudget GetCurrentOrganizationBudget(Guid organizationId);
        List<FoundationBudget> GetOrganizationPurchasersBudgets(Guid organizationId);
        List<FoundationBudget> GetOrganizationBudgetsWithoutPurchasers(Guid organizationId);
        FoundationBudget GetCustomerCurrentBudget(Guid organizationId, Guid purchaserGuid);
    }
}