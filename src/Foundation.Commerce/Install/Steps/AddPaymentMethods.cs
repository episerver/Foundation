using EPiServer;
using Foundation.Cms.Extensions;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Foundation.Commerce.Install.Steps
{
    public class AddPaymentMethods : BaseInstallStep
    {
        public AddPaymentMethods(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService) : base(contentRepository, referenceConverter, marketService)
        {
        }

        public override int Order => 5;

        public override string Description => "Adds payment methods to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger)
        {
            using (var stream = new FileStream(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\paymentMethods.xml"), FileMode.Open))
            {
                var allMarkets = MarketService.GetAllMarkets().Where(x => x.IsEnabled).ToList();
                foreach (var language in allMarkets.SelectMany(x => x.Languages).Distinct())
                {
                    var paymentMethodDto = PaymentManager.GetPaymentMethods(language.TwoLetterISOLanguageName);
                    foreach (var pm in paymentMethodDto.PaymentMethod)
                    {
                        pm.Delete();
                    }
                    PaymentManager.SavePayment(paymentMethodDto);

                    foreach (var xPaymentMethod in GetXElements(stream, "PaymentMethod"))
                    {
                        var method = new
                        {
                            Name = xPaymentMethod.Get("Name"),
                            SystemKeyword = xPaymentMethod.Get("SystemKeyword"),
                            Description = xPaymentMethod.Get("Description"),
                            PaymentClass = xPaymentMethod.Get("PaymentClass"),
                            GatewayClass = xPaymentMethod.Get("GatewayClass"),
                            IsDefault = xPaymentMethod.GetBoolOrDefault("IsDefault"),
                            SortOrder = xPaymentMethod.GetIntOrDefault("SortOrder")
                        };
                        AddPaymentMethod(Guid.NewGuid(),
                            method.Name,
                            method.SystemKeyword,
                            method.Description,
                            method.PaymentClass,
                            method.GatewayClass,
                            method.IsDefault,
                            method.SortOrder,
                            allMarkets,
                            language,
                            paymentMethodDto);
                    }
                }
            }
        }

        private static void AddPaymentMethod(Guid id, string name, string systemKeyword, string description, string implementationClass, string gatewayClass,
            bool isDefault, int orderIndex, IEnumerable<IMarket> markets, CultureInfo language, PaymentMethodDto paymentMethodDto)
        {
            var row = paymentMethodDto.PaymentMethod.AddPaymentMethodRow(id, name, description, language.TwoLetterISOLanguageName,
                            systemKeyword, true, isDefault, gatewayClass,
                            implementationClass, false, orderIndex, DateTime.Now, DateTime.Now);

            var paymentMethod = new PaymentMethod(row);
            paymentMethod.MarketId.AddRange(markets.Where(x => x.IsEnabled && x.Languages.Contains(language)).Select(x => x.MarketId));
            paymentMethod.SaveChanges();
        }
    }
}
