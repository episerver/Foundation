using EPiServer.Framework.Localization;
using Foundation.Cms.Attributes;
using Foundation.Features.MyOrganization.Organization;
using Mediachase.Commerce.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.CreditCard
{
    /// <summary>
    /// Use to store detail information of credit card
    /// </summary>
    public class CreditCardModel : IDataErrorInfo
    {
        protected readonly LocalizationService LocalizationService;
        private static readonly string[] ValidatedProperties =
        {
            "CreditCardNumber",
            "CreditCardSecurityCode",
            "ExpirationYear",
            "ExpirationMonth",
        };

        public List<SelectListItem> Months { get; set; }

        public List<SelectListItem> Years { get; set; }

        public List<SelectListItem> Types { get; set; }

        public string CreditCardTypeFriendlyName { get; set; }

        public string CreditCardId { get; set; }

        public OrganizationModel Organization { get; set; }

        [Display(Name = "Organization")]
        [Required(ErrorMessage = "Organization is required")]
        public string OrganizationId { get; set; }

        public CustomerContact CurrentContact { get; set; }

        [LocalizedDisplay("/CreditCard/Labels/CreditCardName")]
        [LocalizedRequired("/CreditCard/Empty/CreditCardName")]
        public string CreditCardName { get; set; }

        [LocalizedDisplay("/CreditCard/Labels/CreditCardNumber")]
        [LocalizedRequired("/CreditCard/Empty/CreditCardNumber")]
        public string CreditCardNumber { get; set; }

        public string LastFourDigit => CreditCardNumber.Substring(CreditCardNumber.Length - 4);

        [LocalizedDisplay("/CreditCard/Labels/CreditCardSecurityCode")]
        [LocalizedRequired("/CreditCard/Empty/CreditCardSecurityCode")]
        public string CreditCardSecurityCode { get; set; }

        [LocalizedDisplay("/CreditCard/Labels/ExpirationMonth")]
        [LocalizedRequired("/CreditCard/Empty/ExpirationMonth")]
        public int? ExpirationMonth { get; set; }

        [LocalizedDisplay("/CreditCard/Labels/ExpirationYear")]
        [LocalizedRequired("/CreditCard/Empty/ExpirationYear")]
        public int? ExpirationYear { get; set; }

        public Mediachase.Commerce.Customers.CreditCard.eCreditCardType CreditCardType { get; set; }

        string IDataErrorInfo.Error => null;

        string IDataErrorInfo.this[string columnName] => GetValidationError(columnName);

        public CreditCardModel()
        {
            LocalizationService = LocalizationService.Current;
            InitializeValues();
        }

        public CreditCardModel(LocalizationService localizationService)
        {
            LocalizationService = localizationService;
            InitializeValues();
        }

        public bool ValidateData() => IsValid;

        private bool IsValid
        {
            get
            {
                foreach (var property in ValidatedProperties)
                {
                    if (GetValidationError(property) != null)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string GetValidationError(string property)
        {
            string error = null;

            switch (property)
            {
                case "CreditCardNumber":
                    error = ValidateCreditCardNumber();
                    break;

                case "CreditCardSecurityCode":
                    error = ValidateCreditCardSecurityCode();
                    break;

                case "ExpirationYear":
                    error = ValidateExpirationYear();
                    break;

                case "ExpirationMonth":
                    error = ValidateExpirationMonth();
                    break;

                default:
                    break;
            }

            return error;
        }

        private string ValidateExpirationMonth()
        {
            if (ExpirationYear == DateTime.Now.Year && ExpirationMonth < DateTime.Now.Month)
            {
                return LocalizationService.GetString("/CreditCard/ValidationErrors/ExpirationMonth", "Expiration month can't be older than the current month");
            }

            return null;
        }

        private string ValidateExpirationYear()
        {
            if (ExpirationYear < DateTime.Now.Year)
            {
                return LocalizationService.GetString("/CreditCard/ValidationErrors/ExpirationYear", "Expiration year can't be older than the current year");
            }

            return null;
        }

        private string ValidateCreditCardSecurityCode()
        {
            if (string.IsNullOrEmpty(CreditCardSecurityCode))
            {
                return LocalizationService.GetString("/CreditCard/Empty/CreditCardSecurityCode", "Security code is required");
            }

            if (!Regex.IsMatch(CreditCardSecurityCode, "^[0-9]{3}$"))
            {
                return LocalizationService.GetString("/CreditCard/ValidationErrors/CreditCardSecurityCode", "The CSV code should be 3 digits");
            }

            return null;
        }

        private string ValidateCreditCardNumber()
        {
            if (string.IsNullOrEmpty(CreditCardNumber))
            {
                return LocalizationService.GetString("/CreditCard/Empty/CreditCardNumber", "Credit card number is required");
            }

            return null;
        }

        private void InitializeValues()
        {
            Months = new List<SelectListItem>();
            Years = new List<SelectListItem>();
            Types = new List<SelectListItem>();

            for (var i = 1; i < 13; i++)
            {
                Months.Add(new SelectListItem
                {
                    Text = i.ToString(CultureInfo.InvariantCulture),
                    Value = i.ToString(CultureInfo.InvariantCulture)
                });
            }

            for (var i = 0; i < 7; i++)
            {
                var year = (DateTime.Now.Year + i).ToString(CultureInfo.InvariantCulture);
                Years.Add(new SelectListItem
                {
                    Text = year,
                    Value = year
                });
            }

            var listType = Enum.GetValues(typeof(Mediachase.Commerce.Customers.CreditCard.eCreditCardType));
            foreach (Mediachase.Commerce.Customers.CreditCard.eCreditCardType ct in listType)
            {
                Types.Add(new SelectListItem
                {
                    Text = ct.ToString(),
                    Value = ((int)ct).ToString()
                });
            }
        }
    }
}