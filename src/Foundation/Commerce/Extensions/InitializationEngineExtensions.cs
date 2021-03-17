using EPiServer.Commerce.Routing;
using EPiServer.Framework.Initialization;
using Foundation.Commerce.Install;
using Mediachase.BusinessFoundation.Configuration;
using Mediachase.BusinessFoundation.Core;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Meta.Management;
using Mediachase.BusinessFoundation.Data.Modules;
using Mediachase.BusinessFoundation.MetaForm;
using Mediachase.Commerce.Core.RecentReferenceHistory;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.MetaDataPlus;
using Mediachase.MetaDataPlus.Configurator;
using System;
using System.IO;
using System.Linq;
using System.Web.Routing;
using System.Xml.Serialization;

namespace Foundation.Commerce.Extensions
{
    public static class InitializationEngineExtensions
    {
        private static readonly XmlSerializer _listViewProfileXmlSerializer = new XmlSerializer(typeof(ListViewProfile),
            new Type[]
            {
                typeof(int[]),
                typeof(double[]),
                typeof(decimal[]),
                typeof(PrimaryKeyId[]),
                typeof(string[]),
                typeof(DateTime[]),
                typeof(object[])
            });

        public static void InitializeFoundationCommerce(this InitializationEngine context)
        {
            CatalogRouteHelper.MapDefaultHierarchialRouter(RouteTable.Routes, false);
            AddBusinessFoundationIfNeccessary();
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

        private static void AddBusinessFoundationIfNeccessary()
        {
            var bafConnectionString = BusinessFoundationConfigurationSection.Instance.Connection.Database;
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

                UpdateMetaForm(GetContactBaseForm());
                UpdateMetaForm(GetContactGeneralForm());
                UpdateMetaForm(GetContactShortForm());
                UpdateMetaForm(GetContactCreateForm());

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

                UpdateMetaForm(GetBudgetBaseForm());
                UpdateMetaForm(GetBudgetGeneralForm());
                UpdateMetaForm(GetBudgetShortForm());
                UpdateMetaForm(GetBudgetCreateForm());

                scope.SaveChanges();
            }

            var contactProfile = @"<ListViewProfile xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                    <Id>{54a649a9-302f-48bd-b657-11ca3604fda9}</Id>
                    <Name>{Customer:AllContacts}</Name>
                    <IsSystem>true</IsSystem>
                    <IsPublic>true</IsPublic>
                    <FieldSetName>Contacts</FieldSetName>
                   <FieldSet>
                    <string>FullName</string>
                    <string>Email</string>
                    <string>LastOrder</string>
                    <string>NumberOfOrders</string>
                    <string>NumberOfReviews</string>
                    <string>Points</string>
                  </FieldSet>
                  <GroupByFieldName />
                  <Filters />
                  <Sorting />
                  <ColumnsUI>
                    <Column field=""FullName"" width=""150"" />
                    <Column field=""Email"" width=""150"" />
                    <Column field=""LastOrder"" width=""150"" />
                    <Column field=""NumberOfOrders"" width=""150"" />
                    <Column field=""NumberOfReviews"" width=""150"" />
                    <Column field=""Points"" width=""150"" />
                  </ColumnsUI>
                </ListViewProfile>";

            var contactviewProfile = (ListViewProfile)_listViewProfileXmlSerializer.Deserialize(new StringReader(contactProfile));
            ListViewProfile.SaveSystemProfile("Contact", "EntityList", Guid.NewGuid(), contactviewProfile);
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

        private static void UpdateMetaForm(FormDocument formDocument) => SqlFormDocumentManager.Save(formDocument);

        private static FormDocument GetContactBaseForm()
        {
            return FormDocument.LoadFromString(@"<?xml version=""1.0""?>
<FormDocument xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" owner=""Contact"" name=""[MC_BaseForm]"" type=""[MC_BaseForm]"" xmlns=""TableLayout"">
  <Handlers />
  <Table xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" width=""100%"" columns=""50%;*"" cellPadding=""5"">
    <Row>
      <Cell colSpan=""2"" name=""cell_11"">
        <Section uid=""9dbbdbdbe7c7477c9377d9a7f2c21a63"" borderType=""1"" showLabel=""true"" itemIndex=""1"">
          <Labels>
            <Label title=""{Customer:ContactList}"" code=""en-US"" />
          </Labels>
          <Control controlType=""SmartTableLayout"" width=""100%"" uid=""9f3a149fa7984481bb827c259d6cc08f"" cellPadding=""5"" columns=""*"">
            <Item rowIndex=""1"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""069a774e0bfd4df29244ffbf353a4159"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""FullName"" readOnly=""False"" uid=""e01a9bf64575409ba0c60f8446c1ce16"" />
            </Item>
            <Item rowIndex=""2"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""ae79985066bc4f438f6a5724f997576d"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""UserRole"" readOnly=""False"" uid=""d1b0e11c3fbd48b5bc905186f1df0d96"" />
            </Item>
            <Item rowIndex=""3"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""a2caae2f64ed48c2b129bb24256503f8"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""UserLocation"" readOnly=""False"" uid=""546a0c6557dd4a38b2e13c9e367d73b5"" />
            </Item>
            <Item rowIndex=""4"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""2b658b7c4828407dbe6209f8f8c6b355"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Points"" readOnly=""False"" uid=""f0b07a43975843bebac0c69777ddd054"" />
            </Item>
            <Item rowIndex=""5"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""7db638a024944ebcaf22f7da49cf7579"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""NumberOfOrders"" readOnly=""False"" uid=""e5f6f01c191c464f86007ba9cd9878e0"" />
            </Item>
            <Item rowIndex=""6"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""37833756b54c477e9cd68e0c43353265"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""NumberOfReviews"" readOnly=""False"" uid=""e2bfef5749f34c16a251803109d1ea2b"" />
            </Item>
            <Item rowIndex=""7"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""4945410837a84e44a41637c70c119fa5"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Tier"" readOnly=""False"" uid=""24b62dd8758c4a16b0c795a15ca28059"" />
            </Item>
            <Item rowIndex=""8"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""8d617272c90e499eb8c695bc1ef8a725"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""ElevatedRole"" readOnly=""False"" uid=""2ab9eccba52f465487a2763de3aeac32"" />
            </Item>
          </Control>
        </Section>
      </Cell>
    </Row>
    <Row>
      <Cell colSpan=""1"" name=""cell_21"" />
      <Cell colSpan=""1"" name=""cell_22"" />
    </Row>
    <Row>
      <Cell colSpan=""2"" name=""cell_31"" />
    </Row>
  </Table>
</FormDocument>");
        }

        private static FormDocument GetContactGeneralForm()
        {
            return FormDocument.LoadFromString(@"<?xml version=""1.0""?>
<FormDocument xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" owner=""Contact"" name=""[MC_GeneralViewForm]"" type=""[MC_GeneralViewForm]"" xmlns=""TableLayout"">
  <Handlers />
  <Table xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" width=""100%"" columns=""50%;*"" cellPadding=""5"">
    <Row>
      <Cell colSpan=""2"" name=""cell_11"">
        <Section uid=""6554fe1c88a74ec7aa9589836d0d8d64"" borderType=""1"" showLabel=""true"" itemIndex=""1"">
          <Labels>
            <Label title=""{Customer:ContactList}"" code=""en-US"" />
          </Labels>
          <Control controlType=""SmartTableLayout"" width=""100%"" uid=""61752affdd404c4fa532f3fe8309ae05"" cellPadding=""5"" columns=""*"">
            <Item rowIndex=""1"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""7e2972fd34d84220892dd936bf627297"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""FullName"" readOnly=""False"" uid=""75fa88f48cb14ed6ba885bc584a6beaa"" />
            </Item>
            <Item rowIndex=""2"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""36a93cd35229492793452cde4cbe5cb4"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""UserRole"" readOnly=""False"" uid=""742f9ed2a4bb4f9189e832648b8d263b"" />
            </Item>
            <Item rowIndex=""3"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""66f29d4c0f9c460193fc873edb6444b3"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""UserLocation"" readOnly=""False"" uid=""a2ff7707aabe4089991f31d1319d8643"" />
            </Item>
            <Item rowIndex=""4"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""f02b9ec9bb2048728c0b0675e7116a7c"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Points"" readOnly=""False"" uid=""df68869f3c794287905f15a7e14cb56d"" />
            </Item>
            <Item rowIndex=""5"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""ac0300a96e424b929fefd44e767036f8"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""NumberOfOrders"" readOnly=""False"" uid=""004e39b195524cffb08a0f6c27e2ac5a"" />
            </Item>
            <Item rowIndex=""6"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""8799f6dc366543729040de647dc9cc71"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""NumberOfReviews"" readOnly=""False"" uid=""ddaa9612efda408fb8711cfebbad701d"" />
            </Item>
            <Item rowIndex=""7"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""ee40d816229941c4b41a667f1a0bb27a"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Tier"" readOnly=""False"" uid=""272268fb4c034d5d8c5d8700d5ce0b74"" />
            </Item>
            <Item rowIndex=""8"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""52063aa7ca424f32bafc40c508dba35b"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""ElevatedRole"" readOnly=""False"" uid=""84499cd9ae5148c3841b990b1b7f0093"" />
            </Item>            
          </Control>
        </Section>
      </Cell>
    </Row>
    <Row>
      <Cell colSpan=""1"" name=""cell_21"" />
      <Cell colSpan=""1"" name=""cell_22"" />
    </Row>
    <Row>
      <Cell colSpan=""2"" name=""cell_31"" />
    </Row>
  </Table>
</FormDocument>");
        }

        private static FormDocument GetContactShortForm()
        {
            return FormDocument.LoadFromString(@"<?xml version=""1.0""?>
<FormDocument xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" owner=""Contact"" name=""[MC_ShortViewForm]"" type=""[MC_ShortViewForm]"" xmlns=""TableLayout"">
  <Handlers />
  <Table xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" width=""100%"" columns=""50%;*"" cellPadding=""5"">
    <Row>
      <Cell colSpan=""2"" name=""cell_11"">
        <Section uid=""390a6104ae584438870d1db526d883f4"" borderType=""0"" showLabel=""false"" itemIndex=""1"">
          <Labels>
            <Label title=""{Customer:ContactList}"" code=""en-US"" />
          </Labels>
          <Control controlType=""SmartTableLayout"" width=""100%"" uid=""d3ed62d6dd7d4878a699d9077b783ac0"" cellPadding=""5"" columns=""*"">
            <Item rowIndex=""1"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""616bab66b9ab4b2cbdbbbb932e6ed3e9"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""FullName"" readOnly=""False"" uid=""4140514ead234cc0b0adda8ce16ade9a"" />
            </Item>
            <Item rowIndex=""2"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""f5226d47e18b4d619ae3ddc339c537fb"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""UserRole"" readOnly=""False"" uid=""4901febfdd04434499e35bf9db76016c"" />
            </Item>
            <Item rowIndex=""3"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""51c54fe600144147ad12eab4ddd15b2e"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""UserLocation"" readOnly=""False"" uid=""7f1b4a57ee244609a799d208c85b0848"" />
            </Item>
            <Item rowIndex=""4"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""904ca18b9e1d47f289ff0ce165da8697"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Points"" readOnly=""False"" uid=""862bd397062c4ea9a2e014d5b1ac9653"" />
            </Item>
            <Item rowIndex=""5"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""c4879989d0d949d5b2fe0f53be357972"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""NumberOfOrders"" readOnly=""False"" uid=""5876c07ebaae42d791e229f095d5611c"" />
            </Item>
            <Item rowIndex=""6"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""76386a7747cb40f5a419085530959bda"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""NumberOfReviews"" readOnly=""False"" uid=""456347718af5443b9393ba46233fc59e"" />
            </Item>
            <Item rowIndex=""7"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""099aa67b070b4b8d8dbcd3686c5d35de"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Tier"" readOnly=""False"" uid=""64f0328a5a9145a7b2753f7483d55df5"" />
            </Item>
            <Item rowIndex=""8"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""910bf3affbbf442ba8d3a38d210866a8"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""ElevatedRole"" readOnly=""False"" uid=""87b8cdd3bd1d40b0851af3283b073628"" />
            </Item>            
          </Control>
        </Section>
      </Cell>
    </Row>
    <Row>
      <Cell colSpan=""1"" name=""cell_21"" />
      <Cell colSpan=""1"" name=""cell_22"" />
    </Row>
    <Row>
      <Cell colSpan=""2"" name=""cell_31"" />
    </Row>
  </Table>
</FormDocument>");
        }

        private static FormDocument GetContactCreateForm()
        {
            return FormDocument.LoadFromString(@"<?xml version=""1.0""?>
<FormDocument xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" owner=""Contact"" name=""[MC_CreateForm]"" type=""[MC_CreateForm]"" xmlns=""TableLayout"">
  <Handlers />
  <Table xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" width=""100%"" columns=""50%;*"" cellPadding=""5"">
    <Row>
      <Cell colSpan=""2"" name=""cell_11"">
        <Section uid=""231fa1a7878447e885d30dfdb899414b"" borderType=""1"" showLabel=""true"" itemIndex=""1"">
          <Labels>
            <Label title=""{Customer:ContactList}"" code=""en-US"" />
          </Labels>
          <Control controlType=""SmartTableLayout"" width=""100%"" uid=""32ef8364d4254339ad745e11262beeef"" cellPadding=""5"" columns=""*"">
            <Item rowIndex=""1"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""e5d78499bc664cf6a6ab8b6c0ed70428"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""FullName"" readOnly=""False"" uid=""225b6a27c31c418abe90b526f950b835"" />
            </Item>
            <Item rowIndex=""2"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""249937adab3241ddbe70a98cb027d2c2"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""UserRole"" readOnly=""False"" uid=""e30990d47c75492ab557fe47cd468498"" />
            </Item>
            <Item rowIndex=""3"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""9c88a362f77f4f96b39dcc56dd56a81b"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""UserLocation"" readOnly=""False"" uid=""3620a370a8c34c78a035221ef2e81174"" />
            </Item>
            <Item rowIndex=""4"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""b69c57c57bfd456d88294313da71489a"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Points"" readOnly=""False"" uid=""a66cd95df2ce4764875a6c4025a3a0e6"" />
            </Item>
            <Item rowIndex=""5"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""5a859246de474ad3bfcbe73fe50fbc27"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""NumberOfOrders"" readOnly=""False"" uid=""c257538c5a22498785d69c5b036e2d0d"" />
            </Item>
            <Item rowIndex=""6"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""f92d6e7b829945f0b3e3d0627f4940b6"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""NumberOfReviews"" readOnly=""False"" uid=""1759e382036949108d988e4eddf61e36"" />
            </Item>
            <Item rowIndex=""7"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""958b1354838c4e69a55f8826311f8364"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Tier"" readOnly=""False"" uid=""9ba297f8272a454b9545a9c3af6f199c"" />
            </Item>
            <Item rowIndex=""8"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""ccbf65fa417342bd95e99c0c4c06f840"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""ElevatedRole"" readOnly=""False"" uid=""df891316fc9449859ee729ef039d46bc"" />
            </Item>            
          </Control>
        </Section>
      </Cell>
    </Row>
    <Row>
      <Cell colSpan=""1"" name=""cell_21"" />
      <Cell colSpan=""1"" name=""cell_22"" />
    </Row>
    <Row>
      <Cell colSpan=""2"" name=""cell_31"" />
    </Row>
  </Table>
</FormDocument>");
        }

        private static FormDocument GetBudgetBaseForm()
        {
            return FormDocument.LoadFromString(@"<?xml version=""1.0""?>
<FormDocument xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" owner=""Budget"" name=""[MC_BaseForm]"" type=""[MC_BaseForm]"" xmlns=""TableLayout"">
  <Handlers />
  <Table xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" width=""100%"" columns=""50%;*"" cellPadding=""5"">
    <Row>
      <Cell colSpan=""2"" name=""cell_11"">
        <Section uid=""a525ec2f14134b2b8232549655d5518a"" borderType=""1"" showLabel=""true"" itemIndex=""1"">
          <Labels>
            <Label title=""{Customer:Budget}"" code=""en-US"" />
          </Labels>
          <Control controlType=""SmartTableLayout"" width=""100%"" uid=""a771779e0d84446d9eefe6d8976bdc1d"" cellPadding=""5"" columns=""*"">
            <Item rowIndex=""1"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""28e03dc964af45ad82cf30aa0944dc50"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""StartDate"" readOnly=""False"" uid=""d19859de41dd4bafa9ee2acb716d02ba"" />
            </Item>
            <Item rowIndex=""2"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""f36e51fb93bf4309ae80c246580cd91d"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""EndDate"" readOnly=""False"" uid=""06c81333d821482bb539a433db9241d0"" />
            </Item>
            <Item rowIndex=""3"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""67791fb2eb1f4b9e9bdad6f7efdbf37b"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Amount"" readOnly=""False"" uid=""c2f90e4ef125435fb1d5e1c0bdb87b41"" />
            </Item>
            <Item rowIndex=""4"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""313d360f86f24e2ba5cdb8023346dafb"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Currency"" readOnly=""False"" uid=""e52920ac02a84b34ade91793a17c00b4"" />
            </Item>
            <Item rowIndex=""5"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""5e250f1877b14a3da2473cbafeb76a7a"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Status"" readOnly=""False"" uid=""a3dc4e7e437b47b6a0c3fd821af9aa60"" />
            </Item>
            <Item rowIndex=""6"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""02b92ab085844b209571d4d68b294916"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""SpentBudget"" readOnly=""False"" uid=""283abd120bd14814ad13fffafa3574bd"" />
            </Item>
            <Item rowIndex=""7"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""61b24b3e63074e7eb5752b5af13ca72e"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""PurchaserName"" readOnly=""False"" uid=""3f3ac0b66deb4ed7bee069f322f5ba3b"" />
            </Item>
            <Item rowIndex=""8"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""d54007282fe2408a98fd615f19c572eb"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""LockOrganizationAmount"" readOnly=""False"" uid=""c012331b2b5442988b5d3cacef5debd8"" />
            </Item>
            <Item rowIndex=""9"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""0bc0169005d741158829b90a1f046fe3"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""ContactId"" readOnly=""False"" uid=""88f4dd342b20461f8d3ba7609c40ec09"" />
            </Item>
            <Item rowIndex=""10"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""f8c71083ad8745a59320332bd5039dff"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""OrganizationId"" readOnly=""False"" uid=""aae4c9b457ae4babb8a8f2ba30053d54"" />
            </Item>
          </Control>
        </Section>
      </Cell>
    </Row>
    <Row>
      <Cell colSpan=""1"" name=""cell_21"" />
      <Cell colSpan=""1"" name=""cell_22"" />
    </Row>
    <Row>
      <Cell colSpan=""2"" name=""cell_31"" />
    </Row>
  </Table>
</FormDocument>");
        }

        private static FormDocument GetBudgetGeneralForm()
        {
            return FormDocument.LoadFromString(@"<?xml version=""1.0""?>
<FormDocument xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" owner=""Budget"" name=""[MC_GeneralViewForm]"" type=""[MC_GeneralViewForm]"" xmlns=""TableLayout"">
  <Handlers />
  <Table xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" width=""100%"" columns=""50%;*"" cellPadding=""5"">
    <Row>
      <Cell colSpan=""2"" name=""cell_11"">
        <Section uid=""8a4f6cc0991f4d1e8f2540b7d02f1806"" borderType=""1"" showLabel=""true"" itemIndex=""1"">
          <Labels>
            <Label title=""{Customer:Budget}"" code=""en-US"" />
          </Labels>
          <Control controlType=""SmartTableLayout"" width=""100%"" uid=""792863e229e441569d7ed632e19facd9"" cellPadding=""5"" columns=""*"">
            <Item rowIndex=""1"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""e214b55f60554987b7d0ae1b7d910592"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""StartDate"" readOnly=""False"" uid=""acb0fc52dd4749a39650533ef129fe71"" />
            </Item>
            <Item rowIndex=""2"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""eddacce0679447deb0ecb30559eda1e8"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""EndDate"" readOnly=""False"" uid=""2bd1bda8ad024df6beb15bbd58f52095"" />
            </Item>
            <Item rowIndex=""3"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""c8e991178eb54165a841b1cb0a348405"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Amount"" readOnly=""False"" uid=""8ea34a851b4343f9a3e0cc39c6a0932f"" />
            </Item>
            <Item rowIndex=""4"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""6b81eb46265e43f5853dc29b448ebbc7"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Currency"" readOnly=""False"" uid=""bc5d2719137e4ce897eef8c49879155c"" />
            </Item>
            <Item rowIndex=""5"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""ccbf34bb89664ce09f4d753d0736a52e"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Status"" readOnly=""False"" uid=""3644fba0f5a14a5ebb0e50affb381873"" />
            </Item>
            <Item rowIndex=""6"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""95cfe4dac33342f2909a63fdd2d949b4"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""SpentBudget"" readOnly=""False"" uid=""5eb53ad0ed894d4fb5f22ec21f1eb565"" />
            </Item>
            <Item rowIndex=""7"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""f7f5842b65924736a81f0d1d24739e3f"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""PurchaserName"" readOnly=""False"" uid=""807d545fe9a04bd8b9380012d636eddb"" />
            </Item>
            <Item rowIndex=""8"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""0659568ad73d4bcf9ff34b98720cc6a3"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""LockOrganizationAmount"" readOnly=""False"" uid=""05611c4ab09b486989c6e64b0f496aa0"" />
            </Item>
            <Item rowIndex=""9"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""d5c6a21cb39c4b8b9ba951ebf440927e"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""ContactId"" readOnly=""False"" uid=""2f801b111d3e4c46ba5b66f2458982b6"" />
            </Item>
            <Item rowIndex=""10"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""c9ff8a8a320f4f1db5ef087a3e9c652d"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""OrganizationId"" readOnly=""False"" uid=""51e6cb1e4c404c36a537302e453e6e6f"" />
            </Item>
          </Control>
        </Section>
      </Cell>
    </Row>
    <Row>
      <Cell colSpan=""1"" name=""cell_21"" />
      <Cell colSpan=""1"" name=""cell_22"" />
    </Row>
    <Row>
      <Cell colSpan=""2"" name=""cell_31"" />
    </Row>
  </Table>
</FormDocument>");
        }

        private static FormDocument GetBudgetShortForm()
        {
            return FormDocument.LoadFromString(@"<?xml version=""1.0""?>
<FormDocument xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" owner=""Budget"" name=""[MC_ShortViewForm]"" type=""[MC_ShortViewForm]"" xmlns=""TableLayout"">
  <Handlers />
  <Table xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" width=""100%"" columns=""50%;*"" cellPadding=""5"">
    <Row>
      <Cell colSpan=""2"" name=""cell_11"">
        <Section uid=""ecee0de78302411a967bed56ec3221b7"" borderType=""0"" showLabel=""false"" itemIndex=""1"">
          <Labels>
            <Label title=""{Customer:Budget}"" code=""en-US"" />
          </Labels>
          <Control controlType=""SmartTableLayout"" width=""100%"" uid=""fcde128664304ad6b0aeb60ac48724af"" cellPadding=""5"" columns=""*"">
            <Item rowIndex=""1"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""a333c3d4ae1948e0bc559e83205fd304"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""StartDate"" readOnly=""False"" uid=""0b5969d4d480413cada072a9e634067f"" />
            </Item>
            <Item rowIndex=""2"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""2d923c6f68564e048f397a0eafab4477"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""EndDate"" readOnly=""False"" uid=""deeb1cf72d3348b4867cf043cfc1c76f"" />
            </Item>
            <Item rowIndex=""3"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""629bbebb47d442e98548b19faae46f55"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Amount"" readOnly=""False"" uid=""603681e3e69e4c749044e88b5a050f76"" />
            </Item>
            <Item rowIndex=""4"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""4494b2cd939049699f4affa6971b5892"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Currency"" readOnly=""False"" uid=""93f8b8aba86043e4b9c96a329467b5d6"" />
            </Item>
            <Item rowIndex=""5"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""07dd10acc059418789183fae08756a2b"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Status"" readOnly=""False"" uid=""3dd164a50ff54a008650621a00eeb29d"" />
            </Item>
            <Item rowIndex=""6"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""07b64d5397a64678909f18dc4ac0afeb"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""SpentBudget"" readOnly=""False"" uid=""53594df383604454bedb35760506686e"" />
            </Item>
            <Item rowIndex=""7"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""bf9e810ac0684b1b8d1ec57ea3c458d4"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""PurchaserName"" readOnly=""False"" uid=""6e2903bad7ae40e09abdff46c1fcdfe3"" />
            </Item>
            <Item rowIndex=""8"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""c2724934b0b141178300634014269290"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""LockOrganizationAmount"" readOnly=""False"" uid=""6391259a81334648b794a489fa85e588"" />
            </Item>
            <Item rowIndex=""9"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""bf7f404d08d14a06893c3708c4f0bd35"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""ContactId"" readOnly=""False"" uid=""2152a7a351b04edfb1091e7e54d63749"" />
            </Item>
            <Item rowIndex=""10"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""fdf52608807e4734b49f0b6939c45b4d"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""OrganizationId"" readOnly=""False"" uid=""1bd550c0135540a08ec57a5c46facb72"" />
            </Item>
          </Control>
        </Section>
      </Cell>
    </Row>
    <Row>
      <Cell colSpan=""1"" name=""cell_21"" />
      <Cell colSpan=""1"" name=""cell_22"" />
    </Row>
    <Row>
      <Cell colSpan=""2"" name=""cell_31"" />
    </Row>
  </Table>
</FormDocument>");
        }

        private static FormDocument GetBudgetCreateForm()
        {
            return FormDocument.LoadFromString(@"<?xml version=""1.0""?>
<FormDocument xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" owner=""Budget"" name=""[MC_CreateForm]"" type=""[MC_CreateForm]"" xmlns=""TableLayout"">
  <Handlers />
  <Table xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" width=""100%"" columns=""50%;*"" cellPadding=""5"">
    <Row>
      <Cell colSpan=""2"" name=""cell_11"">
        <Section uid=""4f618a4bef23441c9dd45104b468b570"" borderType=""1"" showLabel=""true"" itemIndex=""1"">
          <Labels>
            <Label title=""{Customer:Budget}"" code=""en-US"" />
          </Labels>
          <Control controlType=""SmartTableLayout"" width=""100%"" uid=""4afc834117d4435db8b78232f2e68d27"" cellPadding=""5"" columns=""*"">
            <Item rowIndex=""1"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""e931f98fa4524b7b940cd53bb09be549"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""StartDate"" readOnly=""False"" uid=""dc4cd5eb231a4603ab70f5d0e196bb03"" />
            </Item>
            <Item rowIndex=""2"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""060db2ccdff54832bdc1143db31d2d27"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""EndDate"" readOnly=""False"" uid=""24783a64f89742dd8230a59c94f0be41"" />
            </Item>
            <Item rowIndex=""3"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""0945731ed7274eab801dddb3f859eaf8"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Amount"" readOnly=""False"" uid=""b1a3ec3a022b4c0a95907c6019624960"" />
            </Item>
            <Item rowIndex=""4"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""821b00830d6643aca022f4b33f5f0c90"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Currency"" readOnly=""False"" uid=""74303fa69e6443ee9a271eb3d11852f7"" />
            </Item>
            <Item rowIndex=""5"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""d0d7c971ef40455ca513c02220b61189"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""Status"" readOnly=""False"" uid=""5d60099667604a29a4198991b90da0f6"" />
            </Item>
            <Item rowIndex=""6"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""6c71075bed47450993c44490ec495742"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""SpentBudget"" readOnly=""False"" uid=""c9a8cf99e6414149acf704c8218d6518"" />
            </Item>
            <Item rowIndex=""7"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""8790d72385d64981adc01b64d19d4c28"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""PurchaserName"" readOnly=""False"" uid=""249468ad5da444c896c3c1c6f8d03fff"" />
            </Item>
            <Item rowIndex=""8"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""101c4666e20e44b48e3dd37e4ff9d872"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""LockOrganizationAmount"" readOnly=""False"" uid=""b5bcc285a2234a2899b573ed7b8561d3"" />
            </Item>
            <Item rowIndex=""9"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""a92463f1ff5149b3922a7605d4aac0d5"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""ContactId"" readOnly=""False"" uid=""031fddc3d28448ccb9e8e120da213284"" />
            </Item>
            <Item rowIndex=""10"" cellIndex=""1"" rowSpan=""1"" colSpan=""1"" borderType=""0"" showLabel=""true"" labelWidth=""120px"" uid=""22b320aac8f342f49d778d3aad26aecd"" tabIndex=""0"">
              <Labels>
                <Label title=""[MC_DefaultLabel]"" code=""en-US"" />
              </Labels>
              <Control controlType=""MetaPrimitive"" source=""OrganizationId"" readOnly=""False"" uid=""f88dbeec94d44bc49b6015450ca1e325"" />
            </Item>
          </Control>
        </Section>
      </Cell>
    </Row>
    <Row>
      <Cell colSpan=""1"" name=""cell_21"" />
      <Cell colSpan=""1"" name=""cell_22"" />
    </Row>
    <Row>
      <Cell colSpan=""2"" name=""cell_31"" />
    </Row>
  </Table>
</FormDocument>");
        }
    }
}
