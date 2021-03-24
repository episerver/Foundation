using Foundation.Commerce.Extensions;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.BusinessFoundation.Data.Meta.Management;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;

namespace Foundation.Commerce.Customer
{
    public class FoundationBudget
    {
        public List<Currency> BudgetCurrencies;

        public FoundationBudget(EntityObject budgetEntity) => BudgetEntity = budgetEntity;

        public EntityObject BudgetEntity { get; set; }

        public int BudgetId
        {
            get => BudgetEntity.PrimaryKeyId ?? 0;
            set => BudgetEntity.PrimaryKeyId = value;
        }

        public DateTime StartDate
        {
            get => BudgetEntity.GetDateTimeValue(Constant.Fields.StartDate);
            set => BudgetEntity[Constant.Fields.StartDate] = value;
        }

        public DateTime DueDate
        {
            get => BudgetEntity.GetDateTimeValue(Constant.Fields.EndDate);
            set => BudgetEntity[Constant.Fields.EndDate] = value;
        }

        public decimal Amount
        {
            get => BudgetEntity.GetDecimalValue(Constant.Fields.Amount);
            set => BudgetEntity[Constant.Fields.Amount] = value;
        }

        public decimal SpentBudget
        {
            get => BudgetEntity.GetDecimalValue(Constant.Fields.SpentBudget);
            set => BudgetEntity[Constant.Fields.SpentBudget] = value;
        }

        public string Currency
        {
            get => BudgetEntity.GetStringValue(Constant.Fields.Currency);
            set => BudgetEntity[Constant.Fields.Currency] = value;
        }

        public decimal LockAmount
        {
            get => BudgetEntity.GetDecimalValue(Constant.Fields.LockAmount);
            set => BudgetEntity[Constant.Fields.LockAmount] = value;
        }

        public Guid OrganizationId
        {
            get => BudgetEntity.GetGuidValue(MetaClassManager.GetPrimaryKeyName(Constant.Classes.Organization));
            set => BudgetEntity[MetaClassManager.GetPrimaryKeyName(Constant.Classes.Organization)] = new PrimaryKeyId(value);
        }

        public Guid ContactId
        {
            get => BudgetEntity.GetGuidValue(MetaClassManager.GetPrimaryKeyName(Constant.Classes.Contact));
            set => BudgetEntity[MetaClassManager.GetPrimaryKeyName(Constant.Classes.Contact)] = new PrimaryKeyId(value);
        }

        public string Status
        {
            get => BudgetEntity.GetStringValue(Constant.Fields.Status);
            set => BudgetEntity[Constant.Fields.Status] = value;
        }

        public string PurchaserName
        {
            get => BudgetEntity.GetStringValue(Constant.Fields.PurchaserName);
            set => BudgetEntity[Constant.Fields.PurchaserName] = value;
        }

        public bool IsActive => StartDate <= DateTime.Now && DueDate > DateTime.Now;

        public decimal RemainingBudget => Amount - SpentBudget;
        public decimal UnallocatedBudget => Amount - LockAmount;

        public void SaveChanges() => BusinessManager.Update(BudgetEntity);
    }
}