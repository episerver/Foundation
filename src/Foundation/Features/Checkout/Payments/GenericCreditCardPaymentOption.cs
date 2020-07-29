using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using Foundation.Cms.Attributes;
using Foundation.Commerce.Markets;
using Foundation.Features.MyAccount.CreditCard;
using Mediachase.Commerce;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Foundation.Features.Checkout.Payments
{
    public class GenericCreditCardPaymentOption : PaymentOptionBase, IDataErrorInfo
    {
        private static readonly string[] ValidatedProperties =
        {
            "CreditCardNumber",
            "CreditCardSecurityCode",
            "ExpirationYear",
            "ExpirationMonth",
        };

        public override string SystemKeyword => "GenericCreditCard";

        public List<SelectListItem> Months { get; set; }

        public List<SelectListItem> Years { get; set; }

        public List<SelectListItem> AvaiableCreditCards { get; set; }

        [LocalizedRequired("/Checkout/Payment/Methods/CreditCard/Empty/SelectCreditCard")]
        public string SelectedCreditCardId { get; set; }

        [DefaultValue(true)]
        public bool UseSelectedCreditCard { get; set; }

        [LocalizedDisplay("/Checkout/Payment/Methods/CreditCard/Labels/CreditCardName")]
        [LocalizedRequired("/Checkout/Payment/Methods/CreditCard/Empty/CreditCardName")]
        public string CreditCardName { get; set; }

        [LocalizedDisplay("/Checkout/Payment/Methods/CreditCard/Labels/CreditCardNumber")]
        [LocalizedRequired("/Checkout/Payment/Methods/CreditCard/Empty/CreditCardNumber")]
        public string CreditCardNumber { get; set; }

        [LocalizedDisplay("/Checkout/Payment/Methods/CreditCard/Labels/CreditCardSecurityCode")]
        [LocalizedRequired("/Checkout/Payment/Methods/CreditCard/Empty/CreditCardSecurityCode")]
        public string CreditCardSecurityCode { get; set; }

        [LocalizedDisplay("/Checkout/Payment/Methods/CreditCard/Labels/ExpirationMonth")]
        [LocalizedRequired("/Checkout/Payment/Methods/CreditCard/Empty/ExpirationMonth")]
        public int ExpirationMonth { get; set; }

        [LocalizedDisplay("/Checkout/Payment/Methods/CreditCard/Labels/ExpirationYear")]
        [LocalizedRequired("/Checkout/Payment/Methods/CreditCard/Empty/ExpirationYear")]
        public int ExpirationYear { get; set; }

        public string CardType { get; set; }

        public CreditCard.eCreditCardType CreditCardType { get; set; }

        string IDataErrorInfo.Error => null;

        string IDataErrorInfo.this[string columnName] => GetValidationError(columnName);

        public GenericCreditCardPaymentOption()
            : this(LocalizationService.Current, ServiceLocator.Current.GetInstance<IOrderGroupFactory>(), ServiceLocator.Current.GetInstance<ICurrentMarket>(), ServiceLocator.Current.GetInstance<LanguageService>(), ServiceLocator.Current.GetInstance<IPaymentService>(), ServiceLocator.Current.GetInstance<ICreditCardService>())
        {
        }

        private readonly ICreditCardService _creditCardService;

        public GenericCreditCardPaymentOption(LocalizationService localizationService,
            IOrderGroupFactory orderGroupFactory,
            ICurrentMarket currentMarket,
            LanguageService languageService,
            IPaymentService paymentService,
            ICreditCardService creditCardService)
            : base(localizationService, orderGroupFactory, currentMarket, languageService, paymentService)
        {
            _creditCardService = creditCardService;

            InitializeValues();

            ExpirationMonth = DateTime.Now.Month;
            CreditCardSecurityCode = "212";
            CardType = "Generic";
            CreditCardNumber = "4662519843660534";
        }

        public override IPayment CreatePayment(decimal amount, IOrderGroup orderGroup)
        {
            var payment = orderGroup.CreateCardPayment(OrderGroupFactory);
            payment.CardType = "Credit card";
            payment.PaymentMethodId = PaymentMethodId;
            payment.PaymentMethodName = SystemKeyword;
            payment.Amount = amount;
            if (UseSelectedCreditCard && !string.IsNullOrEmpty(SelectedCreditCardId))
            {
                var creditCard = _creditCardService.GetCreditCard(SelectedCreditCardId);
                payment.CreditCardNumber = creditCard.CreditCardNumber;
                payment.CreditCardSecurityCode = creditCard.SecurityCode;
                payment.ExpirationMonth = creditCard.ExpirationMonth ?? 1;
                payment.ExpirationYear = creditCard.ExpirationYear ?? DateTime.Now.Year;
            }
            else
            {
                payment.CreditCardNumber = CreditCardNumber;
                payment.CreditCardSecurityCode = CreditCardSecurityCode;
                payment.ExpirationMonth = ExpirationMonth;
                payment.ExpirationYear = ExpirationYear;
            }

            payment.Status = PaymentStatus.Pending.ToString();
            payment.CustomerName = CreditCardName;
            payment.TransactionType = TransactionType.Authorization.ToString();
            return payment;
        }

        public override bool ValidateData() => IsValid;

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
                case "SelectedCreditCardId":
                    error = ValidateSelectedCreditCard();
                    break;

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
            if (!UseSelectedCreditCard && ExpirationYear == DateTime.Now.Year && ExpirationMonth < DateTime.Now.Month)
            {
                return LocalizationService.GetString("/Checkout/Payment/Methods/CreditCard/ValidationErrors/ExpirationMonth");
            }

            return null;
        }

        private string ValidateExpirationYear()
        {
            if (!UseSelectedCreditCard && ExpirationYear < DateTime.Now.Year)
            {
                return LocalizationService.GetString("/Checkout/Payment/Methods/CreditCard/ValidationErrors/ExpirationYear");
            }

            return null;
        }

        private string ValidateCreditCardSecurityCode()
        {
            if (!UseSelectedCreditCard)
            {
                if (string.IsNullOrEmpty(CreditCardSecurityCode))
                {
                    return LocalizationService.GetString("/Checkout/Payment/Methods/CreditCard/Empty/CreditCardSecurityCode");
                }

                if (!Regex.IsMatch(CreditCardSecurityCode, "^[0-9]{3}$"))
                {
                    return LocalizationService.GetString("/Checkout/Payment/Methods/CreditCard/ValidationErrors/CreditCardSecurityCode");
                }
            }

            return null;
        }

        private string ValidateCreditCardNumber()
        {
            if (!UseSelectedCreditCard && string.IsNullOrEmpty(CreditCardNumber))
            {
                return LocalizationService.GetString("/Checkout/Payment/Methods/CreditCard/Empty/CreditCardNumber");
            }

            return null;
        }

        private string ValidateSelectedCreditCard()
        {
            if (UseSelectedCreditCard && !_creditCardService.IsReadyToUse(SelectedCreditCardId))
            {
                return LocalizationService.GetString("/Checkout/Payment/Methods/CreditCard/ValidationErrors/InvalidCreditCard");
            }

            return null;
        }

        public void InitializeValues()
        {
            UseSelectedCreditCard = true;
            Months = new List<SelectListItem>();
            Years = new List<SelectListItem>();
            AvaiableCreditCards = new List<SelectListItem>();

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

            var creditCards = _creditCardService.List(false, true);
            AvaiableCreditCards.Add(new SelectListItem
            {
                Text = LocalizationService.GetString("/Checkout/Payment/Methods/CreditCard/Labels/SelectCreditCard"),
                Value = ""
            });
            for (var i = 0; i < creditCards.Count; i++)
            {
                var cc = creditCards[i];
                AvaiableCreditCards.Add(new SelectListItem
                {
                    Text = $"({(cc.CurrentContact != null ? 'P' : 'O')}) ******{cc.CreditCardNumber.Substring(cc.CreditCardNumber.Length - 4)} - {cc.CreditCardType}",
                    Value = cc.CreditCardId
                });
            }
        }
    }
}