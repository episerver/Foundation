using System.Collections.Generic;

namespace Foundation.Commerce
{
    public static class Constant
    {
        public const string SectionName = "InfoBlock";
        public const string ErrorMessages = "ErrorMessages";
        public const string DefaultDisplayOrder = "10000";

        public static class GroupNames
        {
            public const string Blog = "Blog";
            public const string Commerce = "Commerce";
            public const string Locations = "Locations";
        }

        public static class Classes
        {
            public const string Budget = "Budget";
            public const string BudgetFriendly = "Budget";
            public const string Organization = "Organization";
            public const string Contact = "Contact";
        }

        public static class Fields
        {
            public const string StartDate = "StartDate";
            public const string StartDateFriendly = "Start Date";
            public const string DueDate = "DueDate";
            public const string EndDate = "EndDate";
            public const string DueDateFriendly = "Due Date";
            public const string Amount = "Amount";
            public const string SpentBudget = "SpentBudget";
            public const string Currency = "Currency";
            public const string Status = "Status";
            public const string PurchaserName = "PurchaserName";
            public const string UserRole = "UserRole";
            public const string UserRoleFriendly = "User Role";
            public const string UserLocation = "UserLocation";
            public const string UserLocationFriendly = "User Location";
            public const string SelectedOrganization = "SelectedSuborganization";
            public const string SelectedNavOrganization = "SelectedNavSuborganization";
            public const string LockAmount = "LockOrganizationAmount";
            public const string OverwritedMarket = "OverwritedMarket";
        }

        public static class Forms
        {
            public const string EditForm = "[MC_BaseForm]";
            public const string ShortInfoForm = "[MC_ShortViewForm]";
            public const string ViewForm = "[MC_GeneralViewForm]";
        }

        public static class Attributes
        {
            public const string DisplayBlock = "Ref_DisplayBlock";
            public const string DisplayText = "Ref_DisplayText";
            public const string DisplayOrder = "Ref_DisplayOrder";
        }

        public static class Quote
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
        }

        public static class Customer
        {
            public const string CustomerFullName = "CustomerFullName";
            public const string CustomerEmailAddress = "CustomerEmailAddress";
            public const string CurrentCustomerOrganization = "CurrentCustomerOrganization";
        }

        public static class B2BNavigationRoles
        {
            /// <summary>
            ///     List page's name that admin can view on B2BNavigation.
            ///     These name are hard code so need to create a page with the exactly name as below setting
            /// </summary>
            public static readonly List<string> Admin = new List<string>
            {
                "Overview",
                "Users",
                "Orders",
                "Order Pad",
                "Budgeting",
                "B2B Credit Card"
            };

            public static readonly List<string> Approver = new List<string> { "Overview", "Orders", "Order Pad", "Budgeting" };
        }

        public static class Order
        {
            public const string BudgetPayment = "BudgetPayment";
            public const string PendingApproval = "PendingApproval";
        }

        public static class UserRoles
        {
            public const string Admin = "Admin";
            public const string Purchaser = "Purchaser";
            public const string Approver = "Approver";
            public const string None = "None";
        }

        public static class Product
        {
            public const string Brand = "Brand";
            public const string AvailableColors = "Color";
            public const string AvailableSizes = "Size";
            public const string TopCategory = "Top category";
            public const string Categories = "Category";
        }

        public static class BudgetStatus
        {
            public const string OnHold = "OnHold";
            public const string Active = "Active";
            public const string Planned = "Planned";
        }

        public static class Cookies
        {
            public const string B2BImpersonatingAdmin = "B2B_Impersonating_Admin";
        }

        public static class CacheKeys
        {
            public const string MarketViewModel = "MarketsCacheKey";
            public const string MenuItems = "MenuItemsCacheKey";
        }
    }
}