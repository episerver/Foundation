﻿using Episerver.Marketing.Connector.Framework;
using Episerver.Marketing.Connector.Framework.Data;
using EPiServer.ConnectForCampaign.Implementation.Services;
using EPiServer.ConnectForCampaign.Services.Implementation;
using EPiServer.Data.Dynamic;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Tracking.Core;
using Foundation.Cms.ViewModels.Header;
using Foundation.Demo.Campaign;
using Foundation.Demo.Install;
using Foundation.Demo.Install.Steps;
using Foundation.Demo.Personalization;
using Foundation.Demo.ProfileStore;
using Foundation.Demo.ViewModels;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.ViewModels;
using Foundation.Find.Commerce;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Web;

namespace Foundation.Demo
{
    [ModuleDependency(typeof(Commerce.Personalization.Initialize), typeof(Find.Commerce.Initialize), typeof(Social.Initialize))]
    public class Initialize : IConfigurableModule
    {
        private const string ConfigUsername = "campaign:Username";
        private const string ConfigPassword = "campaign:Password";
        private const string ConfigClientid = "campaign:Clientid";

        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddTransient(_ => HttpContext.Current.GetOwinContext());
            services.AddSingleton<IHeaderViewModelFactory, DemoHeaderViewModelFactory>();
            services.AddSingleton<ISearchViewModelFactory, DemoSearchViewModelFactory>();
            services.AddTransient<ContentExportProcessor>();
            services.AddSingleton<IInstallService, InstallService>();
            services.AddSingleton<IInstallStep, AddCurrencies>();
            services.AddSingleton<IInstallStep, AddCustomers>();
            services.AddSingleton<IInstallStep, AddMarkets>();
            services.AddSingleton<IInstallStep, AddPaymentMethods>();
            services.AddSingleton<IInstallStep, AddPromotions>();
            services.AddSingleton<IInstallStep, AddShippingMethods>();
            services.AddSingleton<IInstallStep, AddTaxes>();
            services.AddSingleton<IInstallStep, AddWarehouses>();
            services.AddSingleton<IStorageService, StorageService>();
            services.AddSingleton<IProfileStoreService, ProfileStoreService>();
            services.AddSingleton<ITrackingDataInterceptor, TrackingDataInterceptor>();

            context.ConfigurationComplete += (o, e) =>
            {
                e.Services.AddTransient<IOptinProcessService, DemoOptinProcessService>();
                e.Services.AddSingleton<IRecipientListService, ExtendedRecipientListService>();
            };
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            UpdateCampaignSettings(context);

            var registry = context.Locate.Advanced.GetInstance<IFacetRegistry>();
            GetFacets(context.Locate.Advanced.GetInstance<ICurrentMarket>())
                .ForEach(x => registry.AddFacetDefinitions(x));
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }

        private List<FacetDefinition> GetFacets(ICurrentMarket currentMarket)
        {
            var brand = new FacetStringDefinition
            {
                FieldName = "Brand",
                DisplayName = "Brand"
            };

            var color = new FacetStringListDefinition
            {
                DisplayName = "Color",
                FieldName = "AvailableColors"
            };

            var size = new FacetStringListDefinition
            {
                DisplayName = "Size",
                FieldName = "AvailableSizes"
            };

            var priceRanges = new FacetNumericRangeDefinition(currentMarket)
            {
                DisplayName = "Price",
                FieldName = "DefaultPrice",
                BackingType = typeof(double)

            };
            priceRanges.Range.Add(new SelectableNumericRange() { To = 50 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 50, To = 100 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 100, To = 500 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 500, To = 1000 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 1000 });

            return new List<FacetDefinition> { priceRanges, brand, size, color };
        }

        private static void UpdateCampaignSettings(InitializationEngine context)
        {
            var authService = context.Locate.Advanced.GetInstance<IAuthenticationService>();
            var manager = context.Locate.Advanced.GetInstance<IMarketingConnectorManager>();
            var settings = manager.GetConnectorCredentials(EPiServer.ConnectForCampaign.Core.Helpers.Constants.ConnectorId.ToString(),
                EPiServer.ConnectForCampaign.Core.Helpers.Constants.DefaultConnectorInstanceId.ToString());

            if (settings == null || CampaignSettingsFactory.Current.Password == null)
            {
                // Look for config in the settings
                if (ConfigurationManager.AppSettings[ConfigUsername] != null &&
                    ConfigurationManager.AppSettings[ConfigPassword] != null &&
                    ConfigurationManager.AppSettings[ConfigClientid] != null)
                {
                    var campaignSettings = CampaignSettingsFactory.Current;
                    campaignSettings.UserName = ConfigurationManager.AppSettings[ConfigUsername];
                    campaignSettings.Password = ConfigurationManager.AppSettings[ConfigPassword];
                    campaignSettings.MandatorId = ConfigurationManager.AppSettings[ConfigClientid];
                    campaignSettings.CacheTimeout = 10;

                    if (settings == null)
                    {
                        settings = new ConnectorCredentials()
                        {
                            ConnectorName = EPiServer.ConnectForCampaign.Core.Helpers.Constants.DefaultConnectorName,
                            ConnectorId = EPiServer.ConnectForCampaign.Core.Helpers.Constants.ConnectorId,
                            ConnectorInstanceId = EPiServer.ConnectForCampaign.Core.Helpers.Constants.DefaultConnectorInstanceId,
                            CredentialFields = new Dictionary<string, object>()
                        };
                    }
                    settings.CredentialFields.Add(EPiServer.ConnectForCampaign.Implementation.Helpers.Constants.UsernameFieldKey, campaignSettings.UserName);
                    settings.CredentialFields.Add(EPiServer.ConnectForCampaign.Implementation.Helpers.Constants.PasswordFieldKey, campaignSettings.Password);
                    settings.CredentialFields.Add(EPiServer.ConnectForCampaign.Implementation.Helpers.Constants.MandatorIdFieldKey, campaignSettings.MandatorId);
                    settings.CredentialFields.Add(EPiServer.ConnectForCampaign.Implementation.Helpers.Constants.CacheTimeoutFieldKey, 10);

                    // Test the credentials before saving to database
                    var token = authService.GetToken(
                        settings.CredentialFields[EPiServer.ConnectForCampaign.Implementation.Helpers.Constants.MandatorIdFieldKey] as string,
                        settings.CredentialFields[EPiServer.ConnectForCampaign.Implementation.Helpers.Constants.UsernameFieldKey] as string,
                        settings.CredentialFields[EPiServer.ConnectForCampaign.Implementation.Helpers.Constants.PasswordFieldKey] as string,
                        false);

                    if (string.IsNullOrEmpty(token))
                    {
                        throw new AuthenticationException("Authentication failed");
                    }

                    manager.SaveConnectorCredentials(settings);
                }
            }

            var store = DynamicDataStoreFactory.Instance.GetStore("Marketing_Automation_Settings");
            var globalSettingsList = store.LoadAll<GlobalSettings>().ToList();
            GlobalSettings globalSettings = null;
            if (globalSettingsList.Any() && globalSettingsList.Count > 1)
            {
                globalSettings = globalSettingsList.OrderBy(x => x.LastUpdated).FirstOrDefault();
            }
            else if (globalSettingsList.Any())
            {
                globalSettings = globalSettingsList.FirstOrDefault();
            }
            if (globalSettings == null)
            {
                store.Save(new GlobalSettings
                {
                    EnableFormAutoFill = true,
                    LastUpdated = DateTime.UtcNow
                });
            }
        }
    }
}