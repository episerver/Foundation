# Optimizely Foundation: CMS 12 → CMS 13 Upgrade

**Last updated:** 2026-09-02 (build 1943)

---

## Phase Status

| Phase | Description | Status |
|---|---|---|
| Phase 1 | Remove incompatible packages, 0 build errors on CMS 12 | ✅ Complete |
| Phase 2 | Upgrade to .NET 10 / CMS 13 / Commerce 15, 0 build errors | ✅ Complete |
| Phase 3 | Runtime fixes — site returns HTTP 200, homepage loads | ✅ Complete |
| Phase 4 | Optimizely Graph + Content Manager, Find stubs replaced | ✅ Complete |
| Phase 5+ | Graph search implementation, QA, identity, data migration | 🔄 In progress |

### Current QA fixes (Phase 5+)
- ✅ Category listing showing 0 products (`d05873a4`)
- ✅ Translation keys rendering verbatim (`e142db2e`)
- ✅ Homepage product block scoped to wrong catalog category (`3bb31e49`)
- ✅ QuickView X button didn't close modal — `data-dismiss` → `data-bs-dismiss` in `_QuickViewModal.cshtml`
- ✅ Coupon Apply did nothing — JS syntax error fixed in `main.min.js`; `[FromQuery]`→`[FromForm]` in `DefaultCartController`
- ✅ Estimate Shipping 405 — `[FromBody]`→`[FromForm]` in `DefaultCartController.EstimateShipping`
- ✅ Credit Card form posts to wrong URL — `EditForm.cshtml` now uses `"CreditCard"` as controller name in `BeginForm`
- ✅ PlaceOrder 500 — address/payment update calls wrapped in try-catch in `CheckoutController.PlaceOrder`
- ✅ Dashboard 500 — null guard on `GetCurrentContact()` in `ProfilePageController.Index`
- ✅ Dashboard 500 "View 'Save' not found" — `return View("Index", viewModel)` explicit name in `ProfilePageController.Index` (see CMS 13 action routing note below)
- ✅ My Organization Users 500 — null guard on `currentOrganization?.SubOrganizations` in `UsersController.Index`
- ✅ My Organization 500 — null guard on `currentOrganization` in `OrganizationController.Index`
- ✅ Homepage search not finding products — three bugs fixed:
  1. **JS**: `search-box.js` used `!= "QuickSearch"` which routed all unconfigured-SearchOption searches through the old EPiServer.Find `/find_v2/_autocomplete` endpoint (removed in CMS 13). Fixed to `=== "AutoSearch"` so QuickSearch AJAX dropdown is the default.
  2. **JS**: typo `style.dsiplay` → `style.display` in `hidePopover()`.
  3. **Settings defaults**: `ShowProductSearchResults`, `ShowContentSearchResults`, `ShowPdfSearchResults` booleans in `SearchSettings.cs` defaulted to `false` (C# default). Added `[DefaultValue(true)]` and `[DefaultValue("QuickSearch")]` for `SearchOption`. **NOTE: existing CMS data is unaffected by this — you must manually enable "Show products in search results" in CMS admin → Global Settings Root → [Site] → Search Settings for the deployed site.**
  4. **JS build**: Removed dead import of `Features/Recommendations/WidgetBlock/product-recommendations` from `foundation.commerce.js` (Recommendations feature removed in upgrade; import was breaking webpack build).
- ✅ Search returning 0 products (in-memory fallback broken) — three additional bugs fixed:
  1. **Graph 0-result bypass**: `SearchService` only fell back to in-memory on `catch (Exception)`. When Graph returns 0 results (products not yet indexed), no exception is thrown so the fallback never triggered. Fixed by checking `result.Any()` / `(result.Total ?? 0) > 0` before returning Graph results across all search methods.
  2. **`GetCatalogEntries` with root link returns empty in Commerce 15**: `_contentLoader.GetDescendents(_referenceConverter.GetRootLink())` returns nothing — the catalog system root is not a traversable content tree node. Fixed in `GetCatalogEntries<T>()` in `SearchService.cs`: when the root link is detected, enumerate each top-level `CatalogContent` child of the root and collect their descendants instead (mirrors the access pattern that category browse already uses correctly).
  3. **AutoSearch path used for all searches**: CMS settings had `SearchOption = "AutoSearch"`, which routed JS to `easyAutocomplete` calling `/find_v2/_autocomplete` (removed EPiServer.Find endpoint). Fixed in `_Navigation.cshtml` — normalise `"AutoSearch"` → `"QuickSearch"` at render time since AutoSearch is non-functional in CMS 13.
- ✅ Loading box spinner always visible on page load — CSS cascade issue: `.loading-box { display: flex }` in `components/**` (loaded after `base/**`) overrode `.display-none { display: none }` since both are single-class selectors with equal specificity and last-rule wins. Fixed by adding `.loading-box.display-none { display: none }` to `_loading-box.scss` — the two-class selector has specificity (0,2,0) which beats either single-class rule (0,1,0) regardless of cascade order.
- ✅ Cloudflare CDN serving stale CSS after deployments — CSS `max-age=14400` meant CDN served old CSS for up to 4 hours after a deploy. Fixed by adding `asp-append-version="true"` to the `<link>` and `<script>` tags for `main.min.css` and `main.min.js` in `_MasterLayout.cshtml`, `_Layout.cshtml`, and `_LoginLayout.cshtml`. ASP.NET Core appends a file-content hash to the URL on each deploy, guaranteeing a CDN cache miss automatically.
- ✅ DXP Application Manager error "Cannot find data type ApplicationHostTable" — CMS 13 TVP types `ApplicationHostTable` and `ApplicationUrlFormatTable` were in the local SQL user's default schema rather than `[dbo]` in the combined bacpac. On DXP Azure SQL the app user's default schema is `dbo`, so the unqualified type lookup failed. Fixed by creating both types in `dbo` in the local combined DB and re-exporting/re-importing the bacpac. See the **ApplicationHostTable schema issue** note in DXP Operations below.
- ✅ DXP homepage 500 — `ICurrentMarket.GetCurrentMarket()` returns null on DXP when no Application is configured (fresh database import, before Application Manager has been set up). Five crashes fixed: (1) `LanguageService.GetCurrentLanguage()` NullRef on `CurrentMarket.DefaultLanguage` — null guard + `"en"` fallback; (2) `CartService.LoadCart()` NullRef inside Commerce's `IOrderRepositoryExtensions.LoadOrCreateCart` — guard on `GetCurrentMarket() != null` before calling; (3) `CurrencyService.GetAvailableCurrencies()` NullRef on `CurrentMarket.Currencies` — `?.Currencies ?? Enumerable.Empty<Currency>()`; (4) `CartViewModelFactory.CreateMiniCartViewModel` line 60 `ArgumentException: The currency is empty` — `CurrencyService.GetCurrentCurrency()` was returning `default(Currency)` (null code) when market is null; changed to return `new Currency("USD")`; (5) `MarketsViewComponent.Invoke` NullRef on `currentMarket.MarketId.Value` at cache key lookup — null guard at top of `Invoke`, returns empty `MarketViewModel` when market is null. **Permanent fix:** configure an Application in Settings → Application Manager with the DXP host URL — Commerce can then resolve the DEFAULT market and all services work correctly.
- ✅ DXP full-site 500 after catalog bacpac deploy — two errors: (1) `ArgumentException: Invalid sql table name = cls_Contact` crashing `AnonymousCartMergingMiddleware` on every request (including the error handler), caused by `cls_Contact` and 7 other `cls_*` Customer/BF MetaClass tables missing from DXP DB; (2) `Could not find stored procedure 'UniqueCoupons_GetByPromotionId'`. Root cause: `EPiServer.Commerce.Core.sql` creates the base Commerce schema but does NOT create: `cls_*` tables (dynamically generated by Commerce's Business Foundation/MetaClass system), Foundation custom stored procs (`UniqueCoupons_*`, `FoundationConfiguration_*`), Commerce MetaDataPlus order extension tables (`OrderGroup_ShoppingCart`, `OrderFormPayment_*`, `LineItemEx`, `ShipmentEx`, etc.), or their associated CRUD stored procs (`mc_cls_*`, `mdpsp_avto_*`). Fix: run `C:\Windows\Temp\merge_missing_objects.sql` which uses cursor + `SELECT INTO` to copy all 45 missing tables and cursor + `sys.sql_modules` extraction to create all 121 missing stored procedures from the local Commerce DB into the DXP DB. Also required creating `udttUniqueCoupons` user-defined table type in DXP DB before `UniqueCoupons_Save` would compile. Re-exported as `foundation.cms.sqldb.2026.05.11d.bacpac`.
- ✅ DXP 500 — Commerce Business Foundation (BF) `MetaClass.GetJoinedTableList()` NullRef on `cls_*` tables present but created via `SELECT INTO` (no PKs/constraints). BF considers them incompletely initialised and crashes whenever any code path calls `CustomerContext.CurrentContact`, `CurrentContactId`, or any API that internally calls `GetContactByUserId`. Six crash sites fixed across five Foundation files — see the **BF MetaClass NullRef** section in Completed Fixes for full details and the fix pattern. Files fixed (in order of discovery):
  1. `CartService.LoadCart(string, string, bool)` — wrapped `LoadOrCreateCart` in try-catch; also null-guarded `DefaultCartName` and siblings (build 1920)
  2. `PriceCalculationService.GetSalePrice` — wrapped `CustomerContext.Current.CurrentContact` in try-catch (build 1922)
  3. `BookmarksService.Add/Get/Remove` — wrapped `CustomerContext.Current.CurrentContact` in try-catch in all three methods (build 1923)
  4. `CartService.LoadCart(string, bool)` — 2-arg overload was a one-liner that called `CurrentContactId` before reaching the try-catch in the 3-arg overload; expanded to multi-line with its own try-catch (build 1924)
  5. `CustomerService.GetCurrentContact` — wrapped `_customerContext.CurrentContact` in try-catch (build 1925)
- ✅ DXP 500 — Commerce Admin UI pages (`/ui/Commerce/dashboard`, `/ui/Commerce/catalog`, `/ui/Commerce/marketing`) crashing via `EPiServer.Commerce.Security.Internal.BaseController.Tracking()` → `PrincipalExtensions.GetContactEmail()` → `CustomerContext.GetContactByUserId()` → `MetaClass.GetJoinedTableList()` NullRef. These are Commerce **package** controllers — Foundation code has no try-catch influence over them. Root cause: the DXP combined bacpac's `mcmd_MetaFieldType` table only contains rows 1–25 (standard types from `EPiServer.Commerce.Core.sql`); six custom BF types (IDs 26–31: `OrganizationType`, `BusinessCategory`, `AddressType`, `ContactGroup`, `CreditCardType`, `DemoUserMenu`) that Commerce adds during first-run metadata initialization are absent. Seven fields across four MetaClasses reference these missing types. When BF resolves a field's type → null → NullRef inside `GetJoinedTableList()`. Fix: added `EnsureBFMetaFieldTypes()` to `Program.cs` startup which INSERTs the six missing rows by name if absent. Also added `EnsureBFMetaClassPKs()` (build 1926) which adds `PRIMARY KEY CLUSTERED` to all 8 `cls_*` tables for correct BF query generation (build 1927).
- ✅ Opal Chat not available on DXP — `AddOpalChat()` was gated behind `_configuration["Optimizely:OpalChat:InstanceId"]` which is always empty because `OpalChatOptionsConfigurer` resolves the InstanceId from the DXP platform context at runtime, not from appsettings. The guard prevented registration entirely. Fixed by calling `services.AddOpalChat()` unconditionally (`Startup.cs`). Safe on local dev — `InstanceId` is not `[Required]` so `ValidateOnStart` does not fail; widget is simply hidden with no platform context. Deployed as build 1930.
- ✅ Edit mode preview not updating on content change — `SectionsVisibility.OnPageEditing` defaults to `false` in CMS 13 (Visual Builder is the new default). Fixed by adding `services.Configure<CmsFeatureOptions>(o => o.SectionsVisibility.OnPageEditing = true)` in `Startup.cs`. Class: `EPiServer.Cms.Shell.UI.Configurations.CmsFeatureOptions`. Pre-existing build error also fixed: `PreviewController.cs` was missing `using EPiServer.Framework.Web.Mvc;` (required for `[RequireClientResources]` attribute). Deployed as build 1929.
- ✅ Commerce upgraded to 15.0.0 GA — removed `EPiServer.Commerce 15.0.0-preview1` + `EPiServer.Commerce.ODP 14.45.3` (with `<NoWarn>NU1605</NoWarn>` workaround). Replaced with `EPiServer.Commerce 15.0.0` + `EPiServer.Commerce.ODP 15.0.0` (clean, no suppression needed). Dependency tree identical to preview1; all transitive DLLs already present. Deployed as build 1931.
- ✅ Package upgrade: CMS 13.0.2 → 13.1.0, Commerce 15.0.0 → 15.0.1, Commerce.ODP 15.0.0 → 15.0.1, Optimizely.Cmp.Client 1.1.0 → 1.2.0, all Graph/ContentManager/Forms/Identity packages to 13.1.0. No code changes required — no breaking API changes in either release. `Optimizely.Cms.Opal.Tools` remains at 13.0.0 (no 13.1.0 available). Deployed as build 1935.
- ✅ Machine-specific `david_cms13-upgrade` references removed from source — `Program.cs` comment, `publish.ps1` (`$appPoolName` now reads from `$env:IIS_APP_POOL_NAME`, falls back to `cms13-upgrade`), `teardown.cmd` and `resetup.cmd` parameterized via `%SITENAME%` env var, `CLAUDE.md` updated to use `{sitename}` placeholders. `appsettings.Development.json` and `publish/` output were already gitignored. `.claude/settings.local.json` is machine-specific by design and left as-is.
- ✅ Customer login "Something Went Wrong" + profile/order/address pages broken on DXP — root cause: `AddCmsAspNetIdentity<SiteUser>` was only called in Development; non-Development registered null-returning stubs for `ServiceAccessor<ApplicationSignInManager>` and `ServiceAccessor<ApplicationUserManager>`, causing NullReferenceExceptions on any customer-facing auth path. Fixed by calling `services.AddCmsAspNetIdentity<SiteUser>` unconditionally in all environments (`Startup.cs`). On DXP, `AddOptimizelyIdentity(useAsDefault: false)` still intercepts auth **only for CMS UI paths** (protected modules, edit/preview context) so editors use Opti ID while site visitors keep using ASP.NET Identity cookies. Deployed as build 1936.
- ✅ Projects feature not enabled in CMS editor — `ProjectUIOptions.ProjectModeEnabled` defaults to `null` and `ProjectGadgetEnabled` defaults to `false` in CMS 13; both must be set explicitly. Fixed by adding `services.Configure<ProjectUIOptions>(o => { o.ProjectModeEnabled = true; o.ProjectGadgetEnabled = true; })` in `Startup.cs` (namespace: `EPiServer.Cms.Shell.UI.Rest.Projects`). Note: this is the built-in CMS 13 Projects feature — `EPiServer.Labs.ProjectEnhancements` (the old add-on) has no CMS 13 version and remains removed. Deployed as build 1934.
- ✅ `EPiServer.Labs.LanguageManager` 6.0.0 installed — first version with CMS 13 support (requires `EPiServer.CMS.UI >= 13.0.1`). `services.AddLanguageManager()` added to `Startup.cs`. No extra configuration required; defaults are suitable. `AddLanguageManager` is in `EPiServer.DependencyInjection` namespace (already imported). Deployed as build 1943.
- ⬆️ Order confirmation crash for credit-card payments — `_GenericCreditCardConfirmation.cshtml` partial was typed to `ICreditCardPayment`, an interface removed in Commerce 15 (only a stub exists in `CommerceTypeStubs.cs`). Razor model binding throws `InvalidOperationException` on the Order Confirmation page and My Account Order Details. Fix: change partial model to `IPayment` (aligning with `_GiftCardPaymentConfirmation.cshtml`); remove card detail rows (number, expiry, CVV) — Commerce 15 no longer stores this on payment objects; display only payment method + `CustomerName`. Also remove dead cast to `ICreditCardPayment` in `GenericCreditCardPaymentGateway.cs`. **Upstream fix in [episerver/Foundation#982](https://github.com/episerver/Foundation/pull/982), not yet pulled into this branch.**
- ⬆️ Visual Builder experience/section/element types added upstream — PR #982 adds `BlankExperience`, `BlankSection`, and five element content types (`ButtonElement`, `HeadingElement`, `ImageElement`, `ParagraphElement`, `ProductElement`) for headless Visual Builder composition. Six existing blocks (`CallToActionBlock`, `CarouselBlock`, `HeroBlock`, `ProductHeroBlock`, `TeaserBlock`, `TextBlock`) gain `CompositionBehaviors = new[] { "SectionEnabled" }`. `DisplayTemplatesInit.cs` seeds style definitions (heading sizes, button styles, product card variants). **Not yet pulled — requires a `dotnet pull` + build + deploy when ready.**
- 🔄 New Arrivals page product ordering differs from reference (data difference, low priority)

---

## Project Overview

Upgrading the Optimizely Foundation reference implementation from **CMS 12.31.2** to **CMS 13.0.0**.

| Component | Before | After |
|---|---|---|
| Optimizely CMS | 12.31.2 | **13.1.0** |
| Optimizely Commerce | 14.28.0 | **15.0.1** |
| .NET Target Framework | net6.0 | **net10.0** |
| Search | EPiServer.Find 16.3.0 | **Optimizely.Graph.Cms 13.0.1** |

Pre-upgrade additions already in the working tree: CMP integration, AdaptiveImages, TinymceDamPicker,
ODP blocks, new infrastructure files (ContentApiPermissionsInit, CmsCmpPublishingPermissionsInit,
GroupNamesCustom, OdpStory).

---

## Local Environment

> ⚠️ These paths are machine-specific — do not rely on them in code.

- **Build:** `dotnet build src/Foundation/Foundation.csproj -c Release --nologo -v q`
- **Publish output:** `publish/` (relative to working directory)
- **Deploy:** stop IIS app pool → `dotnet publish ... -o publish` → start app pool
- **⚠️ App pool stop takes ~20s** — `Stop-WebAppPool` returns immediately but the process keeps Serilog's log file locked until fully exited. Always poll `Get-WebAppPoolState` until `Stopped` before `Compress-Archive`, otherwise the zip fails. See `C:\Windows\Temp\deploy_1929.ps1` for the correct pattern.
- **IIS site:** app pool and site name match (machine-specific — set `$env:IIS_APP_POOL_NAME` or edit `publish.ps1` / `teardown.cmd` to match your local setup)


### ⚠️ Credentials

Real credentials live in `src/Foundation/appsettings.Development.json` (gitignored — never committed).
`appsettings.json` contains only empty strings/placeholders; Optimizely DXP injects real values on hosted environments.
`ASPNETCORE_ENVIRONMENT=Development` is set on the local IIS app pool so `appsettings.Development.json` is loaded locally.

See `src/Foundation/appsettings.example.json` for the expected structure.

If SQL credentials need to be reset or re-entered, update `src/Foundation/appsettings.Development.json` directly.
Do **not** commit `appsettings.Development.json` — it is in `.gitignore`.

### Deployment failure checklist

If the site returns 500 after a publish, check in this order:

1. **Event Viewer → Application log** — look for the most recent .NET Runtime or IIS AspNetCore Module V2 error
2. **SQL login failure** (`Login failed for user`) — real credentials must be in `src/Foundation/appsettings.Development.json` (gitignored). Ensure it exists with correct connection strings before publishing.
3. **Graph Base-64 error** (`not a valid Base-64 string` in `EnsureSchemaInitializationModule` / `JwtPreviewTokenService`) — the Graph `Secret` in appsettings is a placeholder or malformed. Must be a valid Base-64 string.
4. **App pool keeps stopping** — check Event Viewer for the crash exception, fix the root cause, then restart the app pool (`Start-WebAppPool '<your-site-name>'`).

---

## DXP Operations

### Deploying code to DXP Integration

```powershell
Import-Module 'C:/Users/david/Documents/WindowsPowerShell/Modules/EpiCloud/1.10.0/EpiCloud.psd1' -Force
# Credentials are in C:\Windows\Temp\deploy_*.ps1 scripts — ask the user for projectId/apiKey/apiSecret
$projectId = '<projectId>'
$apiKey    = '<clientKey>'
$apiSecret = '<clientSecret>'

Connect-EpiCloud -ProjectId $projectId -ClientKey $apiKey -ClientSecret $apiSecret | Out-Null
$sasUrl = Get-EpiDeploymentPackageLocation -ProjectId $projectId
Add-EpiDeploymentPackage -SasUrl $sasUrl -Path 'cms.app.YYYY.MM.DD.HHmm.zip'
Start-EpiDeployment -ProjectId $projectId -DeploymentPackage @('cms.app.YYYY.MM.DD.HHmm.zip') `
    -TargetEnvironment Integration -DirectDeploy -Wait
```

Code zip name must match: `{name}.app.{version}.zip` (or `.nupkg`).

**Latest code deploy:** `cms.app.2026.09.02.1943.zip` — EPiServer.Labs.LanguageManager 6.0.0. Script: `C:\Windows\Temp\deploy_1943.ps1`.

### Importing a database to DXP Integration

The DXP portal UI does **not** have a database import option. Use the EpiCloud API instead.

**Bacpac naming is mandatory** — `Add-EpiDeploymentPackage` validates the filename against a regex. The required format is:

```
{name}.cms.sqldb.{version}.bacpac      ← for the CMS/combined database
{name}.commerce.sqldb.{version}.bacpac ← for a Commerce-only database
```

Example: `foundation.cms.sqldb.2026.05.11.bacpac`

```powershell
Import-Module 'C:/Users/david/Documents/WindowsPowerShell/Modules/EpiCloud/1.10.0/EpiCloud.psd1' -Force
# Ask user for projectId/apiKey/apiSecret — do not hardcode here
Connect-EpiCloud -ProjectId $projectId -ClientKey $apiKey -ClientSecret $apiSecret | Out-Null
$sasUrl = Get-EpiDeploymentPackageLocation -ProjectId $projectId
Add-EpiDeploymentPackage -SasUrl $sasUrl -Path 'F:\path\to\foundation.cms.sqldb.YYYY.MM.DD.bacpac'
Start-EpiDeployment -ProjectId $projectId `
    -DeploymentPackage @('foundation.cms.sqldb.YYYY.MM.DD.bacpac') `
    -TargetEnvironment Integration -DirectDeploy -Wait
```

Database imports are only supported for Integration (and ADE) environments, not Preproduction or Production.

You can deploy code and a database together by passing both in `-DeploymentPackage`.

### Preparing a combined CMS+Commerce bacpac for DXP

DXP uses a **single database** for both CMS and Commerce tables. The local dev setup uses two separate databases:
- `{sitename}.Cms` — CMS content (EPiServerDB)
- `{sitename}.Commerce` — Commerce catalog data (EcfSqlConnection)

To build a combined bacpac:

1. Create a fresh local SQL DB (`{sitename}.DXP`) with `sqlcmd -S . -E`
2. Import the CMS bacpac: `sqlpackage /Action:Import /SourceFile:cms.bacpac /TargetDatabaseName:{sitename}.DXP ...`
3. Run Commerce SQL init: `sqlcmd -S . -d {sitename}.DXP -i EPiServer.Commerce.Core.sql`
4. **Merge Commerce data**: copy all non-empty Commerce tables from `{sitename}.Commerce` into `{sitename}.DXP` (see warning below)
5. Export: `sqlpackage /Action:Export /SourceDatabaseName:{sitename}.DXP /TargetFile:foundation.cms.sqldb.....bacpac`
6. Rename to DXP naming convention before uploading

The Commerce SQL file is at `src/Foundation/App_Data/EPiServer.Commerce.Core.sql` (copied from NuGet cache).

⚠️ **Critical: Commerce catalog data must be merged separately.** Running `EPiServer.Commerce.Core.sql` creates the Commerce schema with empty tables (plus some lookup data). The actual catalog (products, prices, inventory) lives only in the local Commerce DB (`{sitename}.Commerce`) and must be copied across before export. Key tables: `Catalog`, `CatalogEntry`, `CatalogNode`, `NodeEntryRelation`, `CatalogItemAsset`, `CatalogItemSeo`, `CatalogContentProperty`, `ecfVersion`, `PriceDetail`, `PriceGroup`, `PriceValue`, `InventoryService`. Also replace the stub reference tables: `Market`, `MarketCountries`, `MarketCurrencies`, `MarketLanguages`, `MarketPaymentMethods`, `MarketShippingMethods`, `Warehouse`, `JurisdictionGroup`, `JurisdictionRelation`, `AssociationType` — Commerce Core.sql creates these with single default rows that don't match the full multi-market dataset in the Commerce DB.

⚠️ **Critical: 45 additional tables and 121 stored procedures are NOT in Commerce Core.sql.** These are dynamically generated by Commerce's Business Foundation/MetaClass system and by Foundation customizations when Commerce first runs against a live DB. They must be copied from the local Commerce DB using `C:\Windows\Temp\merge_missing_objects.sql`. Key missing objects: all `cls_*` customer tables (8), all `Order*` MetaDataPlus extension tables (26 including `OrderGroup_ShoppingCart`, `OrderFormPayment_*`, `LineItemEx`, `ShipmentEx`), `UniqueCoupons`, `FoundationConfiguration`, `CatalogEntryChange`, and all their CRUD stored procs (`mc_cls_*`, `mdpsp_avto_*`, `UniqueCoupons_*`, `FoundationConfiguration_*`). The `udttUniqueCoupons` user-defined table type must also be created manually before `UniqueCoupons_Save` will compile. **Missing `cls_Contact` crashes the entire request pipeline** (including the error handler) via `AnonymousCartMergingMiddleware` on every request.

⚠️ **Critical: `mcmd_MetaFieldType` will be missing 6 custom BF types after a script-built bacpac.** `EPiServer.Commerce.Core.sql` only creates `mcmd_MetaFieldType` rows 1–25. Rows 26–31 (`OrganizationType`, `BusinessCategory`, `AddressType`, `ContactGroup`, `CreditCardType`, `DemoUserMenu`) are only added by Commerce's runtime initialization. A bacpac built purely from scripts + `SELECT INTO` will have 25 rows instead of 31. Missing types cause `MetaClass.GetJoinedTableList()` → NullRef on any `CustomerContext` access. The `EnsureBFMetaFieldTypes()` startup method in `Program.cs` self-heals this at runtime, but if building a new bacpac you should copy the full `mcmd_MetaFieldType` content from the local Commerce DB to avoid the issue entirely.

Merge scripts are in `C:\Windows\Temp\merge_commerce*.sql`, `merge_fix_*.sql`, and `merge_missing_objects.sql`.

### ApplicationHostTable schema issue (DXP bacpac)

**Symptom:** SQL error 351 "Cannot find data type ApplicationHostTable" when saving an Application in Settings → Application Manager. Does not reproduce locally.

**Root cause:** `ApplicationHostTable` and `ApplicationUrlFormatTable` are CMS 13 user-defined table types (TVPs) used as inline parameters by `EPiServer.Applications.ApplicationDB.SaveAsync` (embedded SQL resource `netApplicationSave.sql` in `EPiServer.dll` — not a stored proc). The CMS creates these types without a `dbo.` schema prefix, so they land in the default schema of the local SQL user rather than `[dbo]`. On DXP Azure SQL, the app user's default schema is `dbo`, so the unqualified lookup fails.

**Fix (confirmed working):** Create both types in `dbo` in the local combined DB, then re-export and re-import the bacpac:

```sql
CREATE TYPE [dbo].[ApplicationHostTable] AS TABLE (
    [Authority]           NVARCHAR(MAX) NULL,
    [Type]                INT           NULL,
    [Locale]              NVARCHAR(255) NULL,
    [UseSecureConnection] BIT           NULL)

CREATE TYPE [dbo].[ApplicationUrlFormatTable] AS TABLE (
    [fkContentTypeGUID] UNIQUEIDENTIFIER NULL,
    [Base]              NVARCHAR(50)     NULL,
    [Type]              INT              NULL,
    [Format]            NVARCHAR(MAX)    NULL)
```

Run against `{sitename}.DXP`, then re-export with `sqlpackage /Action:Export` and re-import via EpiCloud (see "Importing a database" above). Verify the new `model.xml` contains `[dbo].[ApplicationHostTable]` entries before deploying.

**Note:** `Program.cs` also contains `EnsureCmsTypes()` which attempts the same `CREATE TYPE` at startup, but this approach did not work on DXP — the Azure SQL app user likely lacks `CREATE TYPE` permission. The bacpac approach is the definitive fix. `EnsureCmsTypes()` is retained as a belt-and-suspenders guard but should not be relied upon alone.

### EnsureCommerceSchema startup code

`Program.cs` contains `EnsureCommerceSchema(connectionString)` which runs Commerce SQL init at app startup when Commerce tables are missing. Key facts:

- **Sentinel**: checks `SELECT COUNT(*) FROM [dbo].[ReportingDates]` — this table is populated by the **last batch** of `EPiServer.Commerce.Core.sql` (`EXEC ecf_GenerateReportingDates` at line 26860 of 26864). Do NOT use any other stored proc or table as the sentinel — `GetContentSchemaVersionNumber` is at line 10653 (40% through) and gives a false positive if a previous startup timed out mid-script.
- **Startup time limit**: `web.config` sets `startupTimeLimit="600"` (600s) to allow the SQL init to complete. Default is 120s which is insufficient.
- **stdout logging**: `web.config` sets `stdoutLogEnabled="true"` — logs go to `.\logs\stdout_*.log` in wwwroot. DXP SCM endpoint is `https://epsa02paast3b2svinte-bef6acc8fdd8bfe0.scm.canadacentral-01.azurewebsites.net/` but requires Microsoft auth to access.
- **Connection string env vars**: checked in order `SQLCONNSTR_EPiServerDB` → `CUSTOMCONNSTR_EPiServerDB` → `SQLAZURECONNSTR_EPiServerDB`.

### DXP blob storage

- Container: `mysitemedia` at `https://epsa02paast3b2svinte.blob.core.windows.net/mysitemedia`
- Get a writable SAS link via: `Get-EpiStorageContainerSasLink -ProjectId $projectId -Environment Integration -StorageContainerName mysitemedia`
- Copy local blobs with AzCopy: `azcopy.exe copy 'publish/App_Data/blobs/*' '$sasUrl' --recursive`
- AzCopy is at `C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\ModelBuilder\azcopy.exe`

---

## Key Platform Changes (CMS 12 → CMS 13)

1. **.NET 10 required** — jump from net6.0 to net10.0
2. **Optimizely Graph mandatory** — EPiServer.Find fully removed; replaced by Graph SDK
3. **Opti ID mandatory** (PaaS) — replaces OpenID Connect / EPiServer identity stack
4. **Commerce 15 required** — CMS 13 is NOT compatible with Commerce 14
5. **Site Definitions removed** — replaced by Application model (`IApplicationResolver`)
6. **On-Page Editing opt-in** — Visual Builder is new default; OPE requires `SectionsVisibility.OnPageEditing = true` via `services.Configure<CmsFeatureOptions>(o => o.SectionsVisibility.OnPageEditing = true)` in `Startup.cs`. Without this, the preview iframe does NOT update when content is changed in edit mode. Class: `EPiServer.Cms.Shell.UI.Configurations.CmsFeatureOptions`.
7. **Plugin Manager removed** — `EPiServer.PlugIn` system gone from Admin
8. **Module URL namespace** — changed from `/EPiServer/` to `/Optimizely/`
9. **New built-in Content Manager** — powered by Graph; EPiServer.Labs.ContentManager superseded
10. **New navigation tag helper** — `<platform-navigation />` replaces `@Html.CreatePlatformNavigationMenu()`

### API Replacement Map

| Deprecated (v12) | Replacement (v13) |
|---|---|
| `PageReference` | `ContentReference` |
| `PageData.PageLink` | `ContentLink` |
| `IContentTypeRepository<PageType>` | `IContentTypeRepository` (non-generic) |
| `SiteDefinition.Current` | Inject `IApplicationResolver` |
| `context.Locate.Advanced` / service locator | Constructor injection via `IServiceProvider` |
| `@Html.CreatePlatformNavigationMenu()` | `<platform-navigation />` tag helper |
| EPiServer.Find LINQ queries | `Optimizely.Graph.Cms.Query` fluent API |

---

## Package Changes

### Removed — no CMS 13 version
| Package | Reason |
|---|---|
| `EPiServer.Find.Cms` / `EPiServer.Find.Commerce` | Replaced by Optimizely Graph |
| `EPiServer.ContentDeliveryApi.*` (all) | Requires CMS UI Core < 13.0.0 |
| `EPiServer.ContentDefinitionsApi` | Requires CMS UI Core < 13.0.0 |
| `EPiServer.ContentManagementApi` | Requires CMS UI Core < 13.0.0 |
| `EPiServer.ServiceApi.Commerce` | Requires Commerce.Core < 15.0.0 |
| `EPiServer.Session` | Targets net5/net6 only |
| `EPiServer.Telemetry.UI` | CMS 12 only |
| `EPiServer.Marketing.Testing` | Explicitly requires CMS < 13.0.0 |
| `EPiServer.Personalization.Commerce` / `MaxMindGeolocation` | No CMS 13 version |
| `UNRVLD.ODP.VisitorGroups` | No CMS 13 version |
| `Advanced.CMS.BulkEdit` / `GroupingHeader` | No CMS 13 version |
| `PowerSlice` | No CMS 13 version |
| `Geta.Optimizely.Categories` / `.Find` | No CMS 13 version |
| `Geta.NotFoundHandler.Optimizely` | No CMS 13 version |
| `Baaijte.Optimizely.ImageSharp.Web` | No CMS 13 version — use `EPiServer.ImageLibrary.ImageSharp` |
| `EPiServer.Labs.LanguageManager` / `ProjectEnhancements` | No CMS 13 version |
| `Optimizely.Labs.MarketingAutomationIntegration.ODP` | No CMS 13 version |
| `EPiServer.CMS.WelcomeIntegration.UI` | No CMS 13 version |
| `EPiServer.GoogleAnalytics` | No CMS 13 version — replace with direct GA4 tag |
| `EPiServer.PdfPreview` | No CMS 13 version |
| `TinymceDamPicker` | No CMS 13 version |
| `EPiServer.Forms.Samples` | Requires Forms < 6.0.0 and CMS < 13.0.0 |
| `EPiServer.Social.Framework` | Deprecated; removed by decision |
| `EPiServer.MarketingAutomationIntegration.ExactTarget` | Removed by decision |
| `EPiServer.Marketing.Automation.Forms` | Depends on ExactTarget |
| `EPiServer.Labs.ContentManager` | Superseded by built-in Content Manager |
| `AdaptiveImages` / `AdaptiveImages.Unsplash` | No CMS 13 version |

### Added — new in CMS 13
| Package | Version | Purpose |
|---|---|---|
| `Optimizely.Graph.Cms` | 13.0.1 | Core Graph integration |
| `Optimizely.Graph.Cms.Query` | 13.0.1 | C# fluent query API |
| `EPiServer.Cms.UI.ContentManager` | 13.0.1 | Built-in Content Manager UI |
| `EPiServer.Cms.UI.AspNetIdentity` | 13.0.1 | Decoupled identity |
| `EPiServer.OptimizelyIdentity` | 13.0.1 | Opti ID integration |

### Upgraded
| Package | From | To |
|---|---|---|
| `EPiServer.CMS` | 12.31.2 | 13.0.1 |
| `EPiServer.Commerce` | 14.28.0 | 15.0.0 |
| `EPiServer.Hosting` | 12.21.7 | 13.0.1 |
| `EPiServer.CMS.TinyMce` | 4.8.0 | 13.0.1 |
| `EPiServer.Forms` | 5.10.7 | 6.0.0 |
| `Advanced.CMS.AdvancedReviews` | 1.3.7 | 2.0.0 |
| `EPiServer.ImageLibrary.ImageSharp` | — | 13.0.1 |
| `SixLabors.ImageSharp` | 2.x | 3.1.12 |
| `System.Configuration.ConfigurationManager` | 6.0.1 | 10.0.2 |
| `System.Linq.Async` | 6.0.1 | 7.0.0 |

---

## Files Excluded from Compilation (Find-dependent)

```xml
<Compile Remove="Infrastructure\Find\**" />
<Compile Remove="Features\Locations\**" />
<Compile Remove="Features\Blocks\ProductFilterBlocks\**" />
```

Non-Find files within `Infrastructure\Find\` are individually re-included — see Foundation.csproj.
Find stubs remain in `Features\Search\FindTypeStubs.cs`.

---

## Optimizely Graph Configuration

Credentials are read from `appsettings.json` — do not hardcode.

```json
"Optimizely": {
  "ContentGraph": {
    "GatewayAddress": "https://cg.optimizely.com",
    "AppKey": "<AppKey>",
    "Secret": "<Secret>",
    "SingleKey": "<SingleKey>"
  }
}
```

Startup.cs registration order is critical:
```csharp
services.AddContentGraph(_ => { });   // MUST be before ContentManager
services.AddContentManager();
```

---

## Completed Fixes & Key Learnings

### Phase 3: Runtime fixes

#### SettingsService
- `GetSiteSettings<T>()`: do NOT bail on `siteId == Guid.Empty` — a site may legitimately use an empty GUID as its ID
- `UpdateSettings()`: add GUID fallback when `contentRootService.List()` returns empty (`tblContentSource` may be empty in a freshly migrated DB)
- Call `InitializeSettings()` early in the `Initialize.Initialize()` module initializer

#### .NET 10 ValidationAttribute breaking change
`Localized*Attribute` classes (`LocalizedRequired`, `LocalizedCompare`, etc.) broke:
setting `this.ErrorMessage` then calling `base.FormatErrorMessage()` throws
_"Either ErrorMessageString or ErrorMessageResourceName must be set, but not both."_

Root cause: .NET 10's `ValidationAttribute` uses a `DefaultMessageFactory` Func internally.
**Fix:** return the localized string directly from `FormatErrorMessage()` — do not set `this.ErrorMessage`.

#### IVisitorGroupRoleRepository
`EPiServer.Personalization.Commerce` was removed. `CampaignVisitorGroupFilter` (in Commerce.Marketing)
depends on `IVisitorGroupRoleRepository`. Register a `NoOpVisitorGroupRoleRepository` stub via
`services.TryAddSingleton<>` in Startup.cs **after** `AddVisitorGroupsUI()` so the real implementation
wins if ever provided.

#### ContentAreaItem.ContentLink type change
In CMS 13, `ContentAreaItem.ContentLink` is `ContentReference`, **not** `PageReference`.
`as PageReference` silently returns null. Use `new PageReference(contentLink.ID, contentLink.WorkID)`.
Fixed in: `PageListBlockComponent.cs`, `HeaderViewModelFactory.cs`.

### Phase 4: Find stubs & Graph

#### EPiServer.Forms 6.0.0
Requires an explicit `services.AddForms()` call — no longer auto-registered by the CMS.

#### Graph + Content Manager registration order
`services.AddContentGraph()` **must** be called before `services.AddContentManager()`.

### DXP-specific: Commerce Business Foundation (BF) MetaClass NullRef

This crash pattern appeared repeatedly on DXP Integration after the bacpac import. It does **not** reproduce locally because the local Commerce DB has been fully initialised by Commerce running against a live DB.

#### Root cause

Two DB deficiencies caused this crash, both stemming from the bacpac being built by script + `SELECT INTO` rather than by running Commerce end-to-end:

**1. Missing `mcmd_MetaFieldType` rows (primary cause of `GetJoinedTableList()` NullRef):**
`EPiServer.Commerce.Core.sql` creates `mcmd_MetaFieldType` with rows 1–25 (standard types). Six custom BF types are only added by Commerce's first-run metadata initialization and are absent from a merged bacpac: `OrganizationType` (26), `BusinessCategory` (27), `AddressType` (28), `ContactGroup` (29), `CreditCardType` (30), `DemoUserMenu` (31). Seven fields across four MetaClasses (`Contact`, `Organization`, `Address`, `CreditCard`) have `TypeName` values that reference these missing types. When BF calls `GetJoinedTableList()` and looks up a field's type by name → null → `NullReferenceException`. Fixed by `EnsureBFMetaFieldTypes()` in `Program.cs` (build 1927).

**2. Missing PKs on `cls_*` tables (secondary, needed for correct BF query generation):**
The `cls_*` Customer/BF tables (`cls_Contact`, `cls_Address`, etc.) were copied via `SELECT INTO` which preserves data but drops all constraints. BF requires primary key constraints for correct SQL query building. Fixed by `EnsureBFMetaClassPKs()` in `Program.cs` (build 1926).

**The permanent fix** is to export the bacpac from a DB where Commerce has run its full startup initialization — which populates `mcmd_MetaFieldType` with all 31 types and creates `cls_*` tables with proper PKs. Both `EnsureXxx()` startup methods in `Program.cs` serve as self-healing runtime fixes until a clean bacpac is produced. Foundation code paths are also individually guarded with try-catch as defence-in-depth.

#### How to diagnose missing MetaFieldTypes

Compare row counts between local Commerce DB and DXP DB:

```sql
-- Run against local Commerce DB — should return 31
SELECT COUNT(*) FROM mcmd_MetaFieldType

-- Run against DXP combined DB — returns 25 (missing 6 custom types)
SELECT COUNT(*) FROM mcmd_MetaFieldType

-- Find fields whose TypeName has no matching MetaFieldType entry:
SELECT mf.MetaFieldId, mc.Name AS MetaClass, mf.Name AS Field, mf.TypeName
FROM mcmd_MetaField mf
JOIN mcmd_MetaClass mc ON mf.MetaClassId = mc.MetaClassId
WHERE mf.TypeName NOT IN (SELECT Name FROM mcmd_MetaFieldType)
ORDER BY mf.MetaClassId, mf.MetaFieldId
```

Any rows returned by the last query are fields that will cause `GetJoinedTableList()` to NullRef at runtime.

#### Stack trace signature

Look for this pattern in the DXP Application Event Log:

```
System.NullReferenceException: Object reference not set to an instance of an object.
   at Mediachase.BusinessFoundation.Data.Meta.Management.MetaClass.GetJoinedTableList()
   at Mediachase.Commerce.Customers.CustomerContext.GetContactByUserId(String userId)
   at Mediachase.Commerce.Customers.CustomerContext.get_CurrentContact()   ← or get_CurrentContactId()
   at <your Foundation code>
```

Any frame between `GetContactByUserId` and your code is irrelevant — the crash is always in BF.

#### Diagnosing which call site is crashing

Each new crash site reveals itself via a different stack frame above `GetContactByUserId`. Read the DXP Event Log (Application log), find the most recent `.NET Runtime` or `ASP.NET Core` error entry, and look for the first Foundation frame above `GetContactByUserId` — that is the unguarded call site.

#### The fix pattern

Wrap every unguarded access to `CustomerContext.CurrentContact` or `CurrentContactId` in a try-catch with a null/default fallback:

```csharp
// For CurrentContact:
CustomerContact contact = null;
try { contact = _customerContext.CurrentContact; } catch { /* BF MetaClass not initialised */ }

// For CurrentContactId (e.g. in CartService):
string contactId;
try { contactId = _customerContext.CurrentContactId.ToString(); }
catch { contactId = null; } // BF MetaClass NullRef
```

Key rule: **the try-catch must wrap the property access itself**, not just the downstream call. A one-liner like `=> LoadCart(name, _customerContext.CurrentContactId.ToString(), validate)` crashes before any downstream try-catch can fire.

#### All fixed call sites (in order of discovery)

| Build | File | Method | What was guarded |
|---|---|---|---|
| 1920 | `CartService.cs` | `LoadCart(string, string, bool)` | `LoadOrCreateCart` call wrapped; `DefaultCartName` null-guarded separately |
| 1922 | `PriceCalculationService.cs` | `GetSalePrice` | `CustomerContext.Current.CurrentContact` access |
| 1923 | `BookmarksService.cs` | `Add`, `Get`, `Remove` | `CustomerContext.Current.CurrentContact` in all three methods |
| 1924 | `CartService.cs` | `LoadCart(string, bool)` | `_customerContext.CurrentContactId` — 2-arg overload expanded from one-liner |
| 1925 | `CustomerService.cs` | `GetCurrentContact` | `_customerContext.CurrentContact` access |
| 1926 | `Program.cs` | `EnsureBFMetaClassPKs()` | Adds PKs to all 8 `cls_*` tables at startup (needed for correct BF SQL generation) |
| 1927 | `Program.cs` | `EnsureBFMetaFieldTypes()` | **Root fix for GetJoinedTableList() NullRef** — inserts 6 missing `mcmd_MetaFieldType` rows; fixes Commerce package controllers (Dashboard/Marketing/Catalog) |

#### Why startup DB fixes were needed in addition to try-catch guards

Commerce's own Shell controllers (`DashboardController`, `MarketingController`, `CatalogController`) call `BaseController.Tracking()` which calls `PrincipalExtensions.GetContactEmail()` → BF crash. These are Commerce package classes — Foundation code cannot add try-catch around them. The only options are:
1. Fix the DB (done via startup methods in `Program.cs`)
2. Override the Commerce Shell controllers (fragile, not done)

**Fix 1 — `EnsureBFMetaFieldTypes()` (build 1927, root fix):**
The DXP combined bacpac contains `mcmd_MetaFieldType` rows 1–25 (standard types from `EPiServer.Commerce.Core.sql`). Six custom BF types are MISSING because they are only added by Commerce's first-run metadata initialization, not by the SQL script. `GetJoinedTableList()` loads each field's type from `mcmd_MetaFieldType` by the `TypeName` string. When the type doesn't exist, it returns null → NullRef.

Missing types inserted by `EnsureBFMetaFieldTypes()` (all `McDataType=8`, `Owner='System'`, `AccessLevel=1`):

| Name | FriendlyName | XSAttributes |
|---|---|---|
| `OrganizationType` | OrganizationType | NULL |
| `BusinessCategory` | BusinessCategory | NULL |
| `AddressType` | AddressType | MultiValue=true XML |
| `ContactGroup` | ContactGroup | NULL |
| `CreditCardType` | CreditCardType | NULL |
| `DemoUserMenu` | Show in Demo User Menu | NULL |

Fields affected (TypeName not in `mcmd_MetaFieldType` without this fix):

| MetaClass | Field | TypeName |
|---|---|---|
| Contact | CustomerGroup | ContactGroup |
| Contact | ShowInDemoUserMenu | DemoUserMenu |
| Organization | OrganizationType | OrganizationType |
| Organization | OrgCustomerGroup | ContactGroup |
| Organization | BusinessCategory | BusinessCategory |
| Address | AddressType | AddressType |
| CreditCard | CardType | CreditCardType |

**Fix 2 — `EnsureBFMetaClassPKs()` (build 1926, secondary fix):**
`EnsureBFMetaClassPKs()` adds `PRIMARY KEY CLUSTERED` to each `cls_*` table that's missing one (tables were created via SELECT INTO without constraints). PKs are needed for correct BF SQL query generation. Queries `INFORMATION_SCHEMA.TABLE_CONSTRAINTS` for `cls_Contact` as sentinel, then alters all 8 tables:

| Table | PK Column | Type |
|---|---|---|
| `cls_Address` | `AddressId` | `uniqueidentifier NOT NULL` |
| `cls_Budget` | `BudgetId` | `int NOT NULL` |
| `cls_Contact` | `ContactId` | `uniqueidentifier NOT NULL` |
| `cls_ContactNote` | `ContactNoteId` | `uniqueidentifier NOT NULL` |
| `cls_CreditCard` | `CreditCardId` | `uniqueidentifier NOT NULL` |
| `cls_GiftCard` | `GiftCardId` | `uniqueidentifier NOT NULL` |
| `cls_Organization` | `OrganizationId` | `uniqueidentifier NOT NULL` |
| `cls_RecentReferenceHistory` | `RecentReferenceHistoryId` | `uniqueidentifier NOT NULL` |

#### Debugging tip — temporary dev exception page

When a DXP 500 gives no useful detail in the response body, temporarily enable the developer exception page unconditionally to see the full stack trace in the browser:

```csharp
// TEMP DEBUG — revert before next deploy
app.UseDeveloperExceptionPage();
// if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
```

Deploy, reproduce the error, read the stack trace, revert immediately. Never leave this in for a production-bound build.

---

### Post-Phase 4: QA fixes

#### CMS 13 — LinkItem properties are read-only when sourced from content
`LinkItem` objects returned from a `LinkItemCollection` content property are backed by a
`ReadOnlyDictionary`. Setting **any** property (`.Title`, `.Text`, `.Href`, `.Target`) on them throws
`System.NotSupportedException: Collection is read-only`.

This caused a 500 on every page render for authenticated users — the layout called
`HeaderViewModelFactory.AddMyAccountMenu`, which iterated `layoutSettings.MyAccountMenu` and set
`linkItem.Title = linkItem.Text` on the retrieved items.

**Fix:** never mutate a `LinkItem` from a content property — clone it first:
```csharp
// WRONG — throws NotSupportedException in CMS 13
linkItem.Title = linkItem.Text;
menuItems.Add(linkItem);

// CORRECT — create a new LinkItem
var item = new LinkItem
{
    Href = linkItem.Href,
    Text = linkItem.Text,
    Title = linkItem.Text,
    Target = linkItem.Target
};
menuItems.Add(item);
```

File: `Features/Header/HeaderViewModelFactory.cs` → `AddMyAccountMenu()`.

#### CMS 13 — content routing leaks action name into ViewResult view resolution

In Optimizely CMS 13, `Html.BeginForm("ActionName", "ControllerName", ...)` resolves the form action
URL using content routing. For content pages this generates a URL like `/en/page-url/ActionName/`.
During this URL generation, the Optimizely routing infrastructure temporarily modifies route state with
`action = "ActionName"`. This leaks into subsequent view name resolution: `return View(model)` (no
explicit view name) uses `ActionDescriptor.RouteValues["action"]` as the view name — which may now be
"ActionName" instead of the action method's own name.

**Symptom:** `System.InvalidOperationException: The view 'Save' was not found` on a page that has
`Html.BeginForm("Save", "SomeController", ...)` in its view.

**Fix:** always use an explicit view name in `return View()` on any controller whose view calls
`Html.BeginForm` with a different action name:
```csharp
// WRONG — view name inferred from (potentially leaked) route data
return View(viewModel);

// CORRECT — explicit view name, immune to route data leakage
return View("Index", viewModel);
```

Applied in: `Features/MyAccount/ProfilePage/ProfilePageController.cs` → `Index()`.

**Side effect:** the canonical `<link>` tag on the rendered page will include the action segment
(`/en/my-account/dashboard/Save`). This is cosmetic and harmless for authenticated pages, but should
be addressed if the page needs correct SEO canonicalization.

#### Scheduled job referencing a removed assembly
A stale scheduled job record for `EPiServer.Marketing.Testing.Web` remains in the DB (`tblScheduledItem`).
Every scheduler tick logs `FileNotFoundException` to the Event Log but does **not** affect page rendering.
To silence it permanently: delete the row from `tblScheduledItem` and `tblScheduledItemLog` where
`Name = 'Marketing Test Monitor'`.

#### Commerce 15 — catalog content loading (CRITICAL)
`_contentLoader.Get<IContent>(r) as T` **silently fails** for catalog entries in Commerce 15.
Commerce 15's content provider requires an entry-specific type; `Get<IContent>` takes a different
internal code path and returns a non-castable proxy.

```csharp
// WRONG — returns null for ProductContent / VariationContent in Commerce 15
_contentLoader.Get<IContent>(r) as T

// CORRECT
_contentLoader.Get<EntryContentBase>(r) as T
```

File: `Features/Search/SearchService.cs` → `GetCatalogEntries<T>()`.

#### CMS 13 localization — translation key paths visible in rendered HTML
Two root causes:
1. `services.AddEmbeddedLocalization<Startup>()` is **still required** in CMS 13 — do not remove it.
2. Optimizely's localization provider **auto-scans the `lang/` directory on disk** at startup.
   `EmbeddedResource` alone is not enough; files must also be physically deployed.

**Fix:**
```xml
<!-- Foundation.csproj — both entries required -->
<EmbeddedResource Include="lang\**\*" />
<Content Include="lang\**\*" CopyToPublishDirectory="PreserveNewest" />
```

#### ProductSearchBlock — products from wrong catalog category
`ProductSearchBlockComponent` passed `null` as `currentContent` to `SearchService.Search()`,
causing it to fall back to the catalog root and return all products (in tree order).

**Fix:** inject `IContentLoader`, extract the first `NodeContent` from `currentBlock.Nodes.Items`,
and pass it as `searchRoot`. `NodeContent` extends `CatalogContentBase`, so `SearchService` uses its
`ContentLink` as the search root — scoping results to the configured category.

File: `Features/Search/ProductSearchBlock/ProductSearchBlockComponent.cs`.

### Phase 5: Graph search

#### Graph search implementation
`IGraphContentClient` (from `Optimizely.Graph.Cms.Query`) injected into `SearchService`.
`services.AddGraphContentClient()` added to `Startup.cs` (namespace: `Optimizely.Cms.DependencyInjection`).
`OrderDirection` enum is in `Optimizely.Graph.Cms.Query.Implementation` (unusual but correct).

**Behaviour:**
- `SearchContent()`, `SearchPdf()` — Graph full-text on `PageData`/`MediaData`; no scoping needed.
- `Search()` with text query — Graph full-text on `ProductContent` (no catalog-node scoping yet; see TODO in file).
- `Search()` with empty query — in-memory fallback (catalog browsing; preserves node scoping).
- `QuickSearch()`, `SearchNewProducts()` — Graph with in-memory fallback.
- All Graph calls are wrapped in try/catch → fall back to in-memory if Graph is unavailable/unindexed.

**TODO:** add catalog-node scoping for product text search once `ProductContent.ParentLink`/`Ancestors`
field mapping in the Graph schema is confirmed. `BuildFilter<T>().And(...)` requires a lambda returning
`GraphFilter` (not `bool`), so `==` on `ContentReference.ID` needs an extension method from
`Optimizely.Graph.Cms.Query.Filtering` (e.g. `Match()`, `In()`).

---

## What Remains (Phase 5+)

| Area | Status | Notes |
|---|---|---|
| Graph search implementation | ✅ Done | `IGraphContentClient` injected; Graph primary path for all text searches; in-memory fallback retained |
| Locations feature | ❌ Excluded | `Features/Locations/**` excluded from compilation — needs Find geo-search replaced |
| ProductFilterBlocks | ❌ Excluded | `Features/Blocks/ProductFilterBlocks/**` excluded — Find Filter API has no direct Graph equivalent |
| SearchOnSale / SearchUsers | 🔄 Stub | Returns empty; needs Graph or Commerce API implementation |
| Commerce flow QA | 🔄 Pending | Cart → checkout → order not yet regression tested; order confirmation credit-card crash fix pending pull (PR #982) |
| Visual Builder content types | ⬆️ Not pulled | `BlankExperience`, `BlankSection`, 5 element types, `SectionEnabled` on 6 blocks — from PR #982 (merged 2026-07-13) |
| Opti ID / auth | 🔄 Self-hosted | Currently using `AddCmsAspNetIdentity` (local auth); Opti ID requires DXP cloud |
| Content Graph indexing | 🔄 Pending | Initial sync not yet verified |
| Full regression QA | 🔄 Pending | Deeper crawl + manual commerce/forms/ODP testing |

---

## Risk Register

| Risk | Severity | Mitigation |
|---|---|---|
| Commerce 15 — GA released 2026-05-18 | ✅ Resolved | Upgraded to 15.0.0 GA in build 1931 |
| Many community add-ons have no v13 version | HIGH | Removed; track GitHub for v13 PRs |
| EPiServer.Find replaced — search uses Graph | LOW | Graph queries implemented; in-memory fallback active until indexing verified |
| UNRVLD ODP visitor groups not available | MEDIUM | ODP personalisation temporarily disabled |
| A/B testing (Marketing.Testing) not available | MEDIUM | Feature temporarily disabled |
| Geta NotFoundHandler not available | MEDIUM | No 404 redirect handling currently |
| TinyMCE DAM picker not available | LOW | DAM picker temporarily disabled |

---

## Reference Links

- [CMS 13 Overview](https://docs.developers.optimizely.com/content-management-system/v13.0.0-CMS/docs/cms-13-overview)
- [CMS 13 Release Notes](https://support.optimizely.com/hc/en-us/articles/44734633809037-2026-Optimizely-CMS-13-PaaS-release-notes)
- [CMS 12 → 13 Developer Guide (Alloy)](https://world.optimizely.com/blogs/robert-svallin/dates/2026/1/from-12-to-13-a-developers-guide-to-upgrading-an-optimizely-cms-alloy-site)
- [Commerce 14 → 15 Upgrade Guide](https://world.optimizely.com/blogs/viet-anh-nguyen/dates/2026/3/upgrade-guide-commerce-14-to-commerce-15-preview-/)
- [CMS 13 Graph SDK](https://world.optimizely.com/blogs/jake-minard/dates/2026/3/introducing-optimizely-cms-13-graph-sdk/)
- [CMS 13 Technical Q&A](https://optimizely.blog/2026/03/technical-qa-for-cms-13/)
- [Optimizely NuGet Feed](https://nuget.optimizely.com/)
