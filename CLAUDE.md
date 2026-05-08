# Optimizely Foundation: CMS 12 → CMS 13 Upgrade

**Last updated:** 2026-05-07

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
- 🔄 New Arrivals page product ordering differs from reference (data difference, low priority)

---

## Project Overview

Upgrading the Optimizely Foundation reference implementation from **CMS 12.31.2** to **CMS 13.0.0**.

| Component | Before | After |
|---|---|---|
| Optimizely CMS | 12.31.2 | **13.0.1** |
| Optimizely Commerce | 14.28.0 | **15.0.0-preview1** |
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
- **IIS site:** `david_cms13-upgrade` — app pool name matches site name
- **URL:** `https://cms13-upgrade.opti-demo.online`

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
4. **App pool keeps stopping** — check Event Viewer for the crash exception, fix the root cause, then `Start-WebAppPool 'david_cms13-upgrade'`.

---

## Key Platform Changes (CMS 12 → CMS 13)

1. **.NET 10 required** — jump from net6.0 to net10.0
2. **Optimizely Graph mandatory** — EPiServer.Find fully removed; replaced by Graph SDK
3. **Opti ID mandatory** (PaaS) — replaces OpenID Connect / EPiServer identity stack
4. **Commerce 15 required** — CMS 13 is NOT compatible with Commerce 14
5. **Site Definitions removed** — replaced by Application model (`IApplicationResolver`)
6. **On-Page Editing opt-in** — Visual Builder is new default; OPE requires `OnPageEditing = true`
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
| `EPiServer.Commerce` | 14.28.0 | 15.0.0-preview1 |
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
| Commerce flow QA | 🔄 Pending | Cart → checkout → order not yet regression tested |
| Opti ID / auth | 🔄 Self-hosted | Currently using `AddCmsAspNetIdentity` (local auth); Opti ID requires DXP cloud |
| Content Graph indexing | 🔄 Pending | Initial sync not yet verified |
| Full regression QA | 🔄 Pending | Deeper crawl + manual commerce/forms/ODP testing |

---

## Risk Register

| Risk | Severity | Mitigation |
|---|---|---|
| Commerce 15 is preview only | HIGH | Accepted; monitor for GA release |
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
