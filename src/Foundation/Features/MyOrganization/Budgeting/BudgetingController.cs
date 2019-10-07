using EPiServer.Logging;
using EPiServer.Web.Mvc;
using Foundation.Cms;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce;
using Mediachase.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.MyOrganization.Budgeting
{
    [Authorize]
    public class BudgetingController : PageController<BudgetingPage>
    {
        private readonly IBudgetService _budgetService;
        private readonly IOrganizationService _organizationService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICustomerService _customerService;
        private readonly CookieService _cookieService = new CookieService();

        public BudgetingController(IBudgetService budgetService, IOrganizationService organizationService, ICurrentMarket currentMarket, ICustomerService customerService)
        {
            _budgetService = budgetService;
            _organizationService = organizationService;
            _currentMarket = currentMarket;
            _customerService = customerService;
        }

        [NavigationAuthorize("Admin,Approver,Purchaser")]
        public ActionResult Index(BudgetingPage currentPage)
        {
            var organizationBudgets = new List<BudgetViewModel>();
            var suborganizationsBudgets = new List<BudgetViewModel>();

            var currentOrganization = !string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization))
                ? _organizationService.GetSubFoundationOrganizationById(_cookieService.Get(Constant.Fields.SelectedSuborganization))
                : _organizationService.GetCurrentFoundationOrganization();

            var viewModel = new BudgetingPageViewModel
            {
                CurrentContent = currentPage,
                OrganizationBudgets = organizationBudgets,
                IsSubOrganization = !string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization))
            };

            if (currentOrganization != null)
            {
                var currentBudget = _budgetService.GetCurrentOrganizationBudget(currentOrganization.OrganizationId);
                if (currentBudget != null)
                {
                    viewModel.CurrentBudgetViewModel = new BudgetViewModel(currentBudget);
                }

                var budgets = _budgetService.GetOrganizationBudgets(currentOrganization.OrganizationId);
                if (budgets != null)
                {
                    organizationBudgets.AddRange(budgets.Select(budget => new BudgetViewModel(budget) { OrganizationName = currentOrganization.Name, IsCurrentBudget = (currentBudget?.BudgetId == budget.BudgetId) }));
                    viewModel.OrganizationBudgets = organizationBudgets;
                }

                var suborganizations = currentOrganization.SubOrganizations;
                if (suborganizations != null)
                {
                    foreach (var suborganization in suborganizations)
                    {
                        var suborgBudgets = _budgetService.GetCurrentOrganizationBudget(suborganization.OrganizationId);
                        if (suborgBudgets != null)
                        {
                            suborganizationsBudgets.Add(new BudgetViewModel(suborgBudgets) { OrganizationName = suborganization.Name });
                        }
                        viewModel.SubOrganizationsBudgets = suborganizationsBudgets;
                    }
                }

                var purchasersBudgetsViewModel = new List<BudgetViewModel>();
                var purchasersBudgets = _budgetService.GetOrganizationPurchasersBudgets(currentOrganization.OrganizationId);
                if (purchasersBudgets != null)
                {
                    purchasersBudgetsViewModel.AddRange(purchasersBudgets.Select(purchaserBudget => new BudgetViewModel(purchaserBudget)));
                }
                viewModel.PurchasersSpendingLimits = purchasersBudgetsViewModel;
            }
            viewModel.IsAdmin = CustomerContext.Current.CurrentContact.Properties[Constant.Fields.UserRole].Value.ToString() == Constant.UserRoles.Admin;

            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult AddBudget(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentContent = currentPage };
            try
            {
                if (!string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization)))
                {
                    var suborganization = _organizationService.GetSubOrganizationById(_cookieService.Get(Constant.Fields.SelectedSuborganization));
                    var organizationCurrentBudget = _budgetService.GetCurrentOrganizationBudget(suborganization.ParentOrganization.OrganizationId);
                    viewModel.AvailableCurrencies = new List<string> { organizationCurrentBudget.Currency };
                    viewModel.IsSubOrganization = true;
                    viewModel.NewBudgetOption = new BudgetViewModel(organizationCurrentBudget);
                }
                else
                {
                    var availableCurrencies = _currentMarket.GetCurrentMarket().Currencies as List<Currency>;
                    if (availableCurrencies != null)
                    {
                        var currencies = new List<string>();
                        currencies.AddRange(availableCurrencies.Select(currency => currency.CurrencyCode));
                        viewModel.AvailableCurrencies = currencies;
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult NewBudget(DateTime startDateTime, DateTime finishDateTime, decimal amount, string currency, string status)
        {
            var result = "true";
            try
            {
                var currentOrganization = _organizationService.GetCurrentFoundationOrganization();
                var organizationId = currentOrganization.OrganizationId;

                // Set finish date to the end of the day.
                finishDateTime = finishDateTime.AddHours(23);
                finishDateTime = finishDateTime.AddMinutes(59);
                finishDateTime = finishDateTime.AddSeconds(59);

                if (!string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization)))
                {
                    organizationId = Guid.Parse(_cookieService.Get(Constant.Fields.SelectedSuborganization));
                    // Validate Ammount of available budget.
                    if (!_budgetService.CheckAmountByTimeLine(currentOrganization.OrganizationId, amount, startDateTime, finishDateTime))
                    {
                        return Json(new { result = "Not enough amount on organization time line budget." });
                    }
                    // It should overlap with another budget of the parent organization
                    if (!_budgetService.IsSuborganizationValidTimeSlice(startDateTime, finishDateTime, currentOrganization.OrganizationId))
                    {
                        return Json(new { result = "Do not overlap the orgnization budget time line." });
                    }
                    // Validate for existing current budget. Avoid duplicate current budget since the budgets of suborg. must fit org. date times. 
                    if (_budgetService.GetBudgetByTimeLine(organizationId, startDateTime, finishDateTime) != null)
                    {
                        return Json(new { result = "Duplicate budget on selected time line." });
                    }
                    // Have to deduct from organization correpondent budget.
                    if (!_budgetService.LockOrganizationAmount(startDateTime, finishDateTime, currentOrganization.OrganizationId, amount))
                    {
                        return Json(new { result = "Cannot lock amount." });
                    }
                }
                else
                {
                    // Invalid date selection. Overlaps with another budget.
                    if (!_budgetService.IsTimeOverlapped(startDateTime, finishDateTime, organizationId))
                    {
                        return Json(new { result = "Invalid Date. Overlaps another budget." });
                    }
                }

                _budgetService.CreateNewBudget(new BudgetViewModel
                {
                    Amount = amount,
                    SpentBudget = 0,
                    Currency = currency,
                    StartDate = startDateTime,
                    DueDate = finishDateTime,
                    OrganizationId = organizationId,
                    IsActive = true,
                    Status = status,
                    LockAmount = 0
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                result = "Server Error.";
            }

            return Json(new { result });
        }

        [NavigationAuthorize("Admin")]
        public ActionResult EditBudget(BudgetingPage currentPage, int budgetId)
        {
            var currentOrganization = !string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization))
                ? _organizationService.GetSubFoundationOrganizationById(_cookieService.Get(Constant.Fields.SelectedSuborganization))
                : _organizationService.GetCurrentFoundationOrganization();

            var currentBudget = _budgetService.GetCurrentOrganizationBudget(currentOrganization.OrganizationId);
            var viewModel = new BudgetingPageViewModel
            {
                CurrentContent = currentPage,
                NewBudgetOption = new BudgetViewModel(_budgetService.GetBudgetById(budgetId))
            };
            if (currentBudget != null && currentBudget.BudgetId == budgetId)
            {
                viewModel.NewBudgetOption.IsCurrentBudget = true;
            }

            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult UpdateBudget(DateTime startDateTime, DateTime finishDateTime, decimal amount, string currency, string status, int budgetId)
        {
            var result = "true";

            var currentOrganization = !string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization))
                ? _organizationService.GetSubFoundationOrganizationById(_cookieService.Get(Constant.Fields.SelectedSuborganization))
                : _organizationService.GetCurrentFoundationOrganization();
            var budget = _budgetService.GetBudgetById(budgetId);

            // Set finish date to the end of the day.
            finishDateTime = finishDateTime.AddHours(23);
            finishDateTime = finishDateTime.AddMinutes(59);
            finishDateTime = finishDateTime.AddSeconds(59);

            //Can update bugdets of same organization as request user organization
            if (budget.OrganizationId != currentOrganization.OrganizationId && currentOrganization.SubOrganizations.All(suborg => suborg.OrganizationId != budget.OrganizationId))
            {
                return Json(new { result = "Invalid Update." });
            }

            try
            {
                var isSuborganizationBudget = _organizationService.GetSubOrganizationById(budget.OrganizationId.ToString()).ParentOrganization != null;

                if (!string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization)) ||
                    isSuborganizationBudget)
                {
                    // Foe editing from organization timeline
                    currentOrganization = _organizationService.GetSubFoundationOrganizationById(budget.OrganizationId.ToString());
                    // Check budget ballance.
                    if (!_budgetService.ValidateSuborganizationNewAmount(currentOrganization.OrganizationId, currentOrganization.ParentOrganization.OrganizationId, amount))
                    {
                        return Json(new { result = "Not enough amount on organization time line budget." });
                    }
                    // Have to unlock the old amount and to lock the new amount from the organization correpondent budget.
                    if (
                        !_budgetService.UnLockOrganizationAmount(startDateTime, finishDateTime,
                            currentOrganization.ParentOrganization.OrganizationId, budget.Amount))
                    {
                        return Json(new { result = "Cannot unlock amount." });
                    }

                    if (
                        !_budgetService.LockOrganizationAmount(startDateTime, finishDateTime,
                            currentOrganization.ParentOrganization.OrganizationId, amount))
                    {
                        return Json(new { result = "Cannot lock amount." });
                    }
                }
                else
                {
                    // Check budget lock ammount.
                    if (budget.LockAmount > amount)
                    {
                        return Json(new { result = "Invalid set amount." });
                    }
                }

                _budgetService.UpdateBudget(new BudgetViewModel
                {
                    Amount = amount,
                    LockAmount = budget.LockAmount,
                    RemainingBudget = budget.RemainingBudget,
                    SpentBudget = budget.SpentBudget,
                    StartDate = startDateTime,
                    DueDate = finishDateTime,
                    Status = status,
                    Currency = currency,
                    BudgetId = budgetId
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                result = "Server Error.";
            }

            return Json(new { result });
        }

        [NavigationAuthorize("Admin")]
        public ActionResult AddBudgetToUser(BudgetingPage currentPage)
        {
            var viewModel = new BudgetingPageViewModel { CurrentContent = currentPage };
            var currentOrganization = !string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization))
               ? _organizationService.GetSubFoundationOrganizationById(_cookieService.Get(Constant.Fields.SelectedSuborganization))
               : _organizationService.GetCurrentFoundationOrganization();
            var budget = _budgetService.GetCurrentOrganizationBudget(currentOrganization.OrganizationId);
            if (budget == null)
            {
                return RedirectToAction("Index");
            }
            viewModel.NewBudgetOption = new BudgetViewModel(budget);

            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult NewBudgetToUser(DateTime startDateTime, DateTime finishDateTime, decimal amount, string currency, string status, string userEmail)
        {
            var result = "true";
            try
            {
                var currentOrganization = !string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization))
               ? _organizationService.GetSubFoundationOrganizationById(_cookieService.Get(Constant.Fields.SelectedSuborganization))
               : _organizationService.GetCurrentFoundationOrganization();

                var organizationId = currentOrganization.OrganizationId;
                var user = _customerService.GetContactByEmail(userEmail);

                // Set finish date to the end of the day.
                finishDateTime = finishDateTime.AddHours(23);
                finishDateTime = finishDateTime.AddMinutes(59);
                finishDateTime = finishDateTime.AddSeconds(59);

                //Check user role.
                if (user.Contact.Properties["UserRole"].Value.ToString() != Constant.UserRoles.Purchaser)
                {
                    return Json(new { result = "Invalid User Role." });
                }
                // Can assign only to same organization user.
                if (user.Contact.ContactOrganization.Name != currentOrganization.Name)
                {
                    return Json(new { result = "Cannot assigned to another organization." });
                }

                var userGuid = Guid.Parse(user.Contact.PrimaryKeyId.Value.ToString());
                // Validate Ammount of available budget.
                if (!_budgetService.HasEnoughAmount(currentOrganization.OrganizationId, amount, startDateTime, finishDateTime))
                {
                    return Json(new { result = "Not enough amount on organization time line budget." });
                }

                // It should overlap with another budget of the parent organization
                if (!_budgetService.IsSuborganizationValidTimeSlice(startDateTime, finishDateTime, organizationId))
                {
                    return Json(new { result = "Do not overlap the orgnization budget time line." });
                }

                // Can have only one active budget per purchaser per current period
                if (_budgetService.GetCustomerCurrentBudget(organizationId, userGuid) != null)
                {
                    return Json(new { result = "Duplicate budget on selected time line." });
                }

                // Have to deduct from organization correpondent budget.
                if (!_budgetService.LockOrganizationAmount(startDateTime, finishDateTime, currentOrganization.OrganizationId, amount))
                {
                    return Json(new { result = "Cannot lock amount." });
                }

                _budgetService.CreateNewBudget(new BudgetViewModel
                {
                    Amount = amount,
                    SpentBudget = 0,
                    Currency = currency,
                    StartDate = startDateTime,
                    DueDate = finishDateTime,
                    ContactId = userGuid,
                    OrganizationId = organizationId,
                    IsActive = true,
                    Status = status,
                    PurchaserName = user.FullName,
                    LockAmount = 0
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                result = "Server Error.";
            }

            return Json(new { result });
        }

        [NavigationAuthorize("Admin")]
        public ActionResult EditUserBudget(BudgetingPage currentPage, int budgetId)
        {
            var viewModel = new BudgetingPageViewModel
            {
                CurrentContent = currentPage,
                NewBudgetOption = new BudgetViewModel(_budgetService.GetBudgetById(budgetId))
            };
            viewModel.NewBudgetOption.IsCurrentBudget = true;
            return View(viewModel);
        }

        [NavigationAuthorize("Admin")]
        public ActionResult UpdateUserBudget(DateTime startDateTime, DateTime finishDateTime, decimal amount, string currency, string status, int budgetId)
        {
            var currentOrganization = !string.IsNullOrEmpty(_cookieService.Get(Constant.Fields.SelectedSuborganization))
              ? _organizationService.GetSubFoundationOrganizationById(_cookieService.Get(Constant.Fields.SelectedSuborganization))
              : _organizationService.GetCurrentFoundationOrganization();
            var budget = _budgetService.GetBudgetById(budgetId);

            // Set finish date to the end of the day.
            finishDateTime = finishDateTime.AddHours(23);
            finishDateTime = finishDateTime.AddMinutes(59);
            finishDateTime = finishDateTime.AddSeconds(59);

            //Can update bugdets of same organization as request user organization
            if (budget.OrganizationId != currentOrganization.OrganizationId && currentOrganization.SubOrganizations.All(suborg => suborg.OrganizationId != budget.OrganizationId))
            {
                return Json(new { result = "Cannot edit another organization button." });
            }
            // Amount cannot be lower then spent amount.
            if (budget.SpentBudget > amount)
            {
                return Json(new { result = "Amount cannot be lower then spent amount" });
            }
            // Check budget ballance.
            if (!_budgetService.CheckAmount(budget.OrganizationId, amount, budget.Amount))
            {
                return Json(new { result = "Not enough amount on organization time line budget." });
            }
            // Have to unlock the old amount and to lock the new amount from the organization correpondent budget.
            if (!_budgetService.UnLockOrganizationAmount(startDateTime, finishDateTime, budget.OrganizationId, budget.Amount))
            {
                return Json(new { result = "Cannot unlock amount." });
            }

            if (!_budgetService.LockOrganizationAmount(startDateTime, finishDateTime, budget.OrganizationId, amount))
            {
                return Json(new { result = "Cannot lock amount." });
            }

            var result = "true";
            try
            {
                _budgetService.UpdateBudget(new BudgetViewModel
                {
                    Amount = amount,
                    LockAmount = budget.LockAmount,
                    RemainingBudget = budget.RemainingBudget,
                    SpentBudget = budget.SpentBudget,
                    StartDate = startDateTime,
                    DueDate = finishDateTime,
                    Status = status,
                    Currency = currency,
                    BudgetId = budgetId,
                    PurchaserName = budget.PurchaserName
                });
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex.StackTrace);
                result = "Server Error.";
            }

            return Json(new { result });
        }

    }
}