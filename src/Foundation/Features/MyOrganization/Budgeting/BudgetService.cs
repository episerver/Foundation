using EPiServer.Logging;
using Foundation.Commerce;
using Foundation.Commerce.Customer;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.MyOrganization.Budgeting
{
    public class BudgetService : IBudgetService
    {
        public List<FoundationBudget> GetActiveUserBudgets(Guid contactId)
        {
            var budgets = GetUserBudgets(contactId);
            if (budgets == null || !budgets.Any())
            {
                return null;
            }

            return budgets.Where(budget => budget.IsActive).ToList();
        }

        public List<FoundationBudget> GetActiveOrganizationBudgets(Guid organizationId)
        {
            var budgets = GetOrganizationBudgets(organizationId);
            if (budgets == null || !budgets.Any())
            {
                return null;
            }

            return budgets.Where(budget => budget.IsActive).ToList();
        }

        public List<FoundationBudget> GetUserBudgets(Guid contactId)
        {
            var budgets = GetAllBudgets();
            if (budgets == null || !budgets.Any())
            {
                return null;
            }

            return budgets.Where(budget => budget.ContactId == contactId).ToList();
        }

        public List<FoundationBudget> GetOrganizationBudgets(Guid organizationId)
        {
            var budgets = GetAllBudgets();
            if (budgets == null || !budgets.Any())
            {
                return null;
            }

            return budgets.Where(budget =>
                budget.OrganizationId == organizationId && budget.PurchaserName == string.Empty).ToList();
        }

        public List<FoundationBudget> GetOrganizationBudgetsWithoutPurchasers(Guid organizationId)
        {
            var budgets = GetAllBudgets();
            if (budgets == null || !budgets.Any())
            {
                return null;
            }

            return budgets.Where(budget =>
                budget.OrganizationId == organizationId && budget.PurchaserName == string.Empty).ToList();
        }

        public List<FoundationBudget> GetAllBudgets()
        {
            var budgets = BusinessManager.List(Constant.Classes.Budget, new List<FilterElement>().ToArray());
            return budgets?.Select(budget => new FoundationBudget(budget)).ToList();
        }

        public FoundationBudget GetBudgetById(int budgetId)
        {
            var budget = BusinessManager.Load(Constant.Classes.Budget, new PrimaryKeyId(budgetId));
            return budget != null ? new FoundationBudget(budget) : null;
        }

        public FoundationBudget GetNewBudget()
        {
            var budgetEntity = BusinessManager.InitializeEntity(Constant.Classes.Budget);
            budgetEntity.PrimaryKeyId = BusinessManager.Create(budgetEntity);
            var budget = new FoundationBudget(budgetEntity);
            budget.SaveChanges();
            return budget;
        }

        public FoundationBudget GetCurrentOrganizationBudget(Guid organizationId)
        {
            var budgets = GetAllBudgets();
            if (budgets == null || !budgets.Any())
            {
                return null;
            }

            var returnedBudgets = budgets.Where(budget =>
                budget.OrganizationId == organizationId && budget.PurchaserName == string.Empty &&
                DateTime.Compare(budget.StartDate, DateTime.Now) <= 0 &&
                DateTime.Compare(DateTime.Now, budget.DueDate) <= 0);
            return returnedBudgets.Any() ? returnedBudgets.First() : null;
        }

        public List<FoundationBudget> GetOrganizationPurchasersBudgets(Guid organizationId)
        {
            var budgets = GetAllBudgets();
            if (budgets == null || !budgets.Any())
            {
                return null;
            }

            return budgets.Where(budget =>
                budget.OrganizationId == organizationId && budget.PurchaserName != string.Empty).ToList();
        }

        public FoundationBudget GetCustomerCurrentBudget(Guid organizationId, Guid purchaserGuid)
        {
            var budgets = GetAllBudgets();
            if (budgets == null || !budgets.Any())
            {
                return null;
            }

            var returnedBudgets = budgets.Where(budget =>
                budget.OrganizationId == organizationId &&
                budget.ContactId == purchaserGuid &&
                DateTime.Compare(budget.StartDate, DateTime.Now) <= 0 &&
                DateTime.Compare(DateTime.Now, budget.DueDate) <= 0);
            return returnedBudgets.Any() ? returnedBudgets.First() : null;
        }

        public void CreateNewBudget(BudgetViewModel budgetModel)
        {
            var budget = GetNewBudget();
            UpdateBudgetEntity(budget, budgetModel);
        }

        public void UpdateBudget(BudgetViewModel budgetModel)
        {
            var budget = GetBudgetById(budgetModel.BudgetId);
            UpdateBudgetEntity(budget, budgetModel);
        }

        public bool LockOrganizationAmount(DateTime startDate, DateTime endDate, Guid guid, decimal amount)
        {
            try
            {
                var deductBudget = GetBudgetByTimeLine(guid, startDate, endDate);
                UpdateBudget(new BudgetViewModel
                {
                    Amount = deductBudget.Amount,
                    OrganizationId = deductBudget.OrganizationId,
                    Currency = deductBudget.Currency,
                    SpentBudget = deductBudget.SpentBudget,
                    DueDate = deductBudget.DueDate,
                    StartDate = deductBudget.StartDate,
                    Status = deductBudget.Status,
                    IsActive = deductBudget.IsActive,
                    BudgetId = deductBudget.BudgetId,
                    ContactId = deductBudget.ContactId,
                    PurchaserName = deductBudget.PurchaserName,
                    LockAmount = deductBudget.LockAmount + amount
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                return false;
            }

            return true;
        }

        public bool LockUserAmount(DateTime startDate, DateTime endDate, Guid organizationGuid, Guid userGuid, decimal amount)
        {
            try
            {
                var deductBudget = GetCustomerCurrentBudget(organizationGuid, userGuid);
                UpdateBudget(new BudgetViewModel
                {
                    Amount = deductBudget.Amount,
                    OrganizationId = deductBudget.OrganizationId,
                    Currency = deductBudget.Currency,
                    SpentBudget = deductBudget.SpentBudget,
                    DueDate = deductBudget.DueDate,
                    StartDate = deductBudget.StartDate,
                    Status = deductBudget.Status,
                    IsActive = deductBudget.IsActive,
                    BudgetId = deductBudget.BudgetId,
                    ContactId = deductBudget.ContactId,
                    PurchaserName = deductBudget.PurchaserName,
                    LockAmount = deductBudget.LockAmount + amount
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                return false;
            }

            return true;
        }

        public bool UnLockOrganizationAmount(DateTime startDate, DateTime endDate, Guid guid, decimal amount)
        {
            try
            {
                var deductBudget = GetBudgetByTimeLine(guid, startDate, endDate);
                UpdateBudget(new BudgetViewModel
                {
                    Amount = deductBudget.Amount,
                    OrganizationId = deductBudget.OrganizationId,
                    Currency = deductBudget.Currency,
                    SpentBudget = deductBudget.SpentBudget,
                    DueDate = deductBudget.DueDate,
                    StartDate = deductBudget.StartDate,
                    Status = deductBudget.Status,
                    IsActive = deductBudget.IsActive,
                    BudgetId = deductBudget.BudgetId,
                    ContactId = deductBudget.ContactId,
                    PurchaserName = deductBudget.PurchaserName,
                    LockAmount = deductBudget.LockAmount - amount
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                return false;
            }

            return true;
        }

        public bool IsTimeOverlapped(DateTime startDate, DateTime dueDateTime, Guid organizationGuid)
        {
            var budgets = GetOrganizationBudgets(organizationGuid);
            if (budgets == null || budgets.Count == 0)
            {
                return true;
            }

            if (budgets.Any(budget => DateTime.Compare(budget.StartDate, dueDateTime) <= 0 &&
                                      DateTime.Compare(startDate, budget.DueDate) <= 0))
            {
                return false;
            }

            return true;
        }

        public bool IsSuborganizationValidTimeSlice(DateTime startDateTime, DateTime finishDateTime,
            Guid organizationGuid)
        {
            var budgets = GetOrganizationBudgets(organizationGuid);
            if (budgets == null || budgets.Count == 0)
            {
                return false;
            }

            if (budgets.Any(budget => DateTime.Compare(budget.StartDate, startDateTime) <= 0 &&
                                      DateTime.Compare(finishDateTime, budget.DueDate) <= 0 &&
                                      DateTime.Compare(budget.StartDate, finishDateTime) <= 0 &&
                                      DateTime.Compare(startDateTime, budget.DueDate) <= 0
            ))
            {
                return true;
            }

            return false;
        }

        public bool HasEnoughAmount(Guid organizationGuid, decimal amount, DateTime startDateTime, DateTime finishDateTime)
        {
            var currentBudget = GetBudgetByTimeLine(organizationGuid, startDateTime, finishDateTime);
            if (currentBudget == null)
            {
                return false;
            }

            return currentBudget.Amount - currentBudget.SpentBudget - currentBudget.LockAmount - amount >= 0;
        }

        public bool HasEnoughAmountOnCurrentBudget(Guid organizationGuid, decimal amount)
        {
            var currentBudget = GetCurrentOrganizationBudget(organizationGuid);
            if (currentBudget == null)
            {
                return false;
            }

            return currentBudget.Amount - currentBudget.SpentBudget - currentBudget.LockAmount - amount >= 0;
        }

        public bool CheckAmount(Guid organizationGuid, decimal newLockAmount, decimal unlockAmount)
        {
            var currentBudget = GetCurrentOrganizationBudget(organizationGuid);
            if (currentBudget == null)
            {
                return false;
            }

            return currentBudget.Amount + unlockAmount - currentBudget.SpentBudget - currentBudget.LockAmount -
                   newLockAmount >= 0;
        }

        public bool ValidateSuborganizationNewAmount(Guid organizationGuid, Guid parentOrganizationId, decimal newLockAmount)
        {
            var currentBudget = GetCurrentOrganizationBudget(organizationGuid);
            if (currentBudget == null)
            {
                return false;
            }

            var parentCurrentBudget = GetCurrentOrganizationBudget(parentOrganizationId);
            if (parentCurrentBudget == null)
            {
                return false;
            }

            return newLockAmount <= parentCurrentBudget.UnallocatedBudget + currentBudget.Amount &&
                   newLockAmount >= currentBudget.LockAmount;
        }

        public bool CheckAmountByTimeLine(Guid organizationGuid, decimal newLockAmount, DateTime startDateTime, DateTime finishDateTime)
        {
            var currentBudget = GetBudgetByTimeLine(organizationGuid, startDateTime, finishDateTime);
            if (currentBudget == null)
            {
                return false;
            }

            return currentBudget.Amount - currentBudget.LockAmount - newLockAmount >= 0;
        }

        public FoundationBudget GetBudgetByTimeLine(Guid organizationId, DateTime startDate, DateTime endDate)
        {
            var organizationBudgets = GetOrganizationBudgets(organizationId);
            if (!organizationBudgets.Any())
            {
                return null;
            }

            var returnBudget = organizationBudgets.Where(budget => DateTime.Compare(budget.StartDate, endDate) <= 0 &&
                                                                   DateTime.Compare(startDate, budget.DueDate) <= 0);
            if (!returnBudget.Any())
            {
                return null;
            }

            return returnBudget.FirstOrDefault();
        }

        private void UpdateBudgetEntity(FoundationBudget budgetEntity, BudgetViewModel budgetModel)
        {
            budgetEntity.Amount = budgetModel.Amount;
            budgetEntity.Currency = budgetModel.Currency;
            budgetEntity.StartDate = budgetModel.StartDate;
            budgetEntity.DueDate = budgetModel.DueDate;
            budgetEntity.Status = budgetModel.Status;
            budgetEntity.PurchaserName = budgetModel.PurchaserName;
            budgetEntity.LockAmount = budgetModel.LockAmount;
            if (budgetModel.OrganizationId != Guid.Empty)
            {
                budgetEntity.OrganizationId = budgetModel.OrganizationId;
            }

            if (budgetModel.ContactId != Guid.Empty)
            {
                budgetEntity.ContactId = budgetModel.ContactId;
            }

            budgetEntity.SaveChanges();
        }
    }
}