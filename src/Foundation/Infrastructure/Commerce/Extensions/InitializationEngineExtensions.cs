using EPiServer.Commerce.Routing;
using EPiServer.Framework.Initialization;
using Foundation.Infrastructure.Commerce.Install;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Meta.Management;
using Mediachase.BusinessFoundation.Data.Modules;
using Mediachase.Commerce.Core.RecentReferenceHistory;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.Data.Provider;
using Mediachase.MetaDataPlus;
using Mediachase.MetaDataPlus.Configurator;

namespace Foundation.Infrastructure.Commerce.Extensions
{
    public static class InitializationEngineExtensions
    {
        public static void InitializeFoundationCommerce(this InitializationEngine context)
        {
            CatalogRouteHelper.MapDefaultHierarchialRouter(false);
            AddBusinessFoundationIfNeccessary(context);
            AddOrderMetaFieldsIfNesccessary();
            var installService = context.Locate.Advanced.GetInstance<IInstallService>();
            if (!installService.ShouldInstall())
            {
                return;
            }
            installService.RunInstallSteps();
        }

        private static void AddOrderMetaFieldsIfNesccessary()
        {
            var orderContext = OrderContext.MetaDataContext;
            if (orderContext == null)
            {
                return;
            }

            var purchaseOrderMetaClass = OrderContext.Current.PurchaseOrderMetaClass;
            if (purchaseOrderMetaClass == null)
            {
                return;
            }

            TryAddMetaField(orderContext, purchaseOrderMetaClass, Constant.Quote.QuoteExpireDate, MetaDataType.DateTime, 8);
            TryAddMetaField(orderContext, purchaseOrderMetaClass, Constant.Quote.QuoteStatus, MetaDataType.LongString, 4000);
            TryAddMetaField(orderContext, purchaseOrderMetaClass, Constant.Quote.PreQuoteTotal, MetaDataType.Decimal, 17, false, false, false);
            TryAddMetaField(orderContext, purchaseOrderMetaClass, Constant.Customer.CurrentCustomerOrganization, MetaDataType.ShortString, 512);
            TryAddMetaField(orderContext, purchaseOrderMetaClass, Constant.Customer.CustomerFullName, MetaDataType.ShortString, 512);
            TryAddMetaField(orderContext, purchaseOrderMetaClass, Constant.Customer.CustomerEmailAddress, MetaDataType.ShortString, 512);
        }

        private static void AddBusinessFoundationIfNeccessary(InitializationEngine context)
        {
            var bafConnectionString = context.Locate.Advanced.GetInstance<IConnectionStringHandler>().Commerce.ConnectionString;
            if (bafConnectionString == null)
            {
                return;
            }

            DataContext.Current = new DataContext(bafConnectionString);
            ModuleManager.InitializeActiveModules();
            var fields = DataContext.Current.MetaModel.MetaClasses[ContactEntity.ClassName].Fields;
            if (fields.Contains("UserRole"))
            {
                return;
            }

            using (var scope = DataContext.Current.MetaModel.BeginEdit(MetaClassManagerEditScope.SystemOwner, AccessLevel.System))
            {
                var manager = DataContext.Current.MetaModel;
                var contactMetaClass = manager.MetaClasses[ContactEntity.ClassName];
                var changeTrackingManifest = ChangeTrackingManager.CreateModuleManifest();
                var recentReferenceManifest = RecentReferenceManager.CreateModuleManifest();

                using (var builder = new MetaFieldBuilder(contactMetaClass))
                {
                    builder.CreateText("UserRole", "{Customer:UserRole}", true, 50, false);
                    builder.CreateText("UserLocation", "{Customer:UserLocation}", true, 50, false);
                    builder.CreateInteger("Points", "{Customer:Points}", true, 0);
                    builder.CreateInteger("NumberOfOrders", "{Customer:NumberOfOrders}", true, 0);
                    builder.CreateInteger("NumberOfReviews", "{Customer:NumberOfReviews}", true, 0);
                    builder.CreateText("Tier", "{Customer:Tier}", true, 100, false);
                    builder.CreateText("ElevatedRole", "{Customer:ElevatedRole}", true, 100, false);
                    builder.CreateHtml("Bookmarks", "{Customer:Bookmarks}", true);
                    builder.SaveChanges();
                }

                var budgetClass = manager.CreateMetaClass("Budget", "{Customer:Budget}", "{Customer:Budget}", "cls_Budget", PrimaryKeyIdValueType.Integer);
                ModuleManager.Activate(budgetClass, changeTrackingManifest);
                using (var builder = new MetaFieldBuilder(budgetClass))
                {
                    builder.CreateDateTime("StartDate", "{Customer:StartDate}", true, false);
                    builder.CreateDateTime("EndDate", "{Customer:EndDate}", true, false);
                    builder.CreateCurrency("Amount", "{Customer:Amount}", true, 0, true);
                    builder.CreateText("Currency", "{Customer:Currency}", true, 50, false);
                    builder.CreateText("Status", "{Customer:Status}", true, 50, false);
                    builder.CreateCurrency("SpentBudget", "{Customer:SpentBudget}", true, 0, true);
                    builder.CreateText("PurchaserName", "{Customer:PurchaserName}", true, 50, false);
                    builder.CreateCurrency("LockOrganizationAmount", "{Customer:LockOrganizationAmount}", true, 0, true);
                    budgetClass.Fields[MetaClassManager.GetPrimaryKeyName(budgetClass.Name)].FriendlyName = "{GlobalMetaInfo:PrimaryKeyId}";
                    var contactReference = builder.CreateReference("Contact", "{Customer:CreditCard_mf_Contact}", true, "Contact", false);
                    contactReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayBlock, "InfoBlock");
                    contactReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayText, "{Customer:Budget}");
                    contactReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayOrder, "10000");
                    var orgReference = builder.CreateReference("Organization", "{Customer:CreditCard_mf_Organization}", true, "Organization", false);
                    orgReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayBlock, "InfoBlock");
                    orgReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayText, "{Customer:Budget}");
                    orgReference.Attributes.Add(McDataTypeAttribute.ReferenceDisplayOrder, "10000");
                    builder.SaveChanges();
                }

                budgetClass.AddPermissions();
                scope.SaveChanges();
            }


        }

        private static void TryAddMetaField(MetaDataContext context,
            Mediachase.MetaDataPlus.Configurator.MetaClass metaClass,
            string name,
            MetaDataType metaDataType,
            int length,
            bool allowNulls = true,
            bool multiLingual = false,
            bool allowSearch = false)
        {
            var metaField = Mediachase.MetaDataPlus.Configurator.MetaField.Load(context, name) ?? Mediachase.MetaDataPlus.Configurator.MetaField.Create(
                                context: context,
                                metaNamespace: metaClass.Namespace,
                                name: name,
                                friendlyName: name,
                                description: name,
                                dataType: metaDataType,
                                length: length,
                                allowNulls: allowNulls,
                                multiLanguageValue: multiLingual,
                                allowSearch: allowSearch,
                                isEncrypted: false);

            if (metaClass.MetaFields.All(x => x.Id != metaField.Id))
            {
                metaClass.AddField(metaField);
            }
            else if (!metaField.DataType.Equals(metaDataType))
            {
                metaClass.DeleteField(metaField.Name);
                Mediachase.MetaDataPlus.Configurator.MetaField.Delete(context, metaField.Id);
                metaField = Mediachase.MetaDataPlus.Configurator.MetaField.Create(context,
                                 metaClass.Namespace,
                                 name,
                                 name,
                                 name,
                                 metaDataType,
                                 length,
                                 allowNulls,
                                 multiLingual,
                                 allowSearch,
                                 false);
                metaClass.AddField(metaField);
            }
        }
    }
}
