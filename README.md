<a href="https://github.com/episerver/Foundation"><img src="https://www.optimizely.com/globalassets/02.-global-images/navigation/optimizely_logo_navigation.svg" title="Foundation" alt="Foundation"></a>

# Optimizely Foundation — CMS 13 Upgrade

This repository is the Optimizely Foundation reference implementation, upgraded from **CMS 12 / .NET 6** to **CMS 13 / .NET 10**.

---

## Platform versions

| Component | Version |
|---|---|
| Optimizely CMS | 13.0.1 |
| Optimizely Commerce | 15.0.0-preview1 |
| .NET target framework | net10.0 |
| Optimizely Graph (replaces Find) | 13.0.1 |
| EPiServer.Forms | 6.0.0 |
| Node.js (front-end build) | 16+ |

---

## Key dependencies

| Package | Version | Notes |
|---|---|---|
| `EPiServer.CMS` | 13.0.1 | Core CMS |
| `EPiServer.Cms.UI.AspNetIdentity` | 13.0.1 | Identity / auth |
| `EPiServer.CMS.TinyMce` | 13.0.1 | Rich text editor |
| `EPiServer.Cms.UI.VisitorGroups` | 13.0.1 | Visitor groups |
| `EPiServer.Commerce` | 15.0.0-preview1 | Commerce |
| `EPiServer.Forms` | 6.0.0 | Forms (requires explicit `AddForms()` in Startup) |
| `EPiServer.Hosting` | 13.0.1 | Hosting infrastructure |
| `EPiServer.ImageLibrary.ImageSharp` | 13.0.1 | Image processing |
| `EPiServer.Cms.UI.ContentManager` | 13.0.1 | Built-in Content Manager UI |
| `Optimizely.Graph.Cms` | 13.0.1 | Search / indexing (replaces EPiServer.Find) |
| `Optimizely.Graph.Cms.Query` | 13.0.1 | C# fluent query API for Graph |
| `Advanced.CMS.AdvancedReviews` | 2.0.0 | Editorial reviews |
| `SixLabors.ImageSharp` | 3.1.12 | Image processing library |
| `Wangkanai.Detection` | 6.11.3 | Device detection |
| `Serilog.AspNetCore` | 6.1.0 | Structured logging |

### Notable packages removed in this upgrade

| Removed package | Reason |
|---|---|
| `EPiServer.Find.Cms` / `.Commerce` | No CMS 13 version — replaced by Optimizely Graph |
| `EPiServer.ContentDeliveryApi.*` | Requires CMS UI Core < 13.0.0 |
| `EPiServer.Marketing.Testing` | Explicitly requires CMS < 13.0.0 |
| `EPiServer.Personalization.Commerce` | No CMS 13 version |
| `Geta.NotFoundHandler.Optimizely` | No CMS 13 version |
| `Geta.Optimizely.Categories` | No CMS 13 version |
| `EPiServer.Social.Framework` | Deprecated |
| `UNRVLD.ODP.VisitorGroups` | No CMS 13 version |
| `Advanced.CMS.BulkEdit` / `GroupingHeader` | No CMS 13 version |
| `EPiServer.Forms.Samples` | Requires Forms < 6.0.0 |

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Node.js 16+](https://nodejs.org/en/download/)
- SQL Server (local instance at `.`)
- IIS with ASP.NET Core Module v2

---

## Local setup

### 1. Configure credentials

Copy `src/Foundation/appsettings.example.json` to `src/Foundation/appsettings.Development.json` and fill in real values:

```json
{
  "ConnectionStrings": {
    "EPiServerDB": "Server=.;Database=<cms-db>;User Id=<user>;Password=<password>;TrustServerCertificate=True",
    "EcfSqlConnection": "Server=.;Database=<commerce-db>;User Id=<user>;Password=<password>;TrustServerCertificate=True"
  },
  "Optimizely": {
    "ContentGraph": {
      "GatewayAddress": "https://cg.optimizely.com",
      "AppKey": "<graph-app-key>",
      "Secret": "<graph-secret-base64>",
      "SingleKey": "<graph-single-key>"
    }
  }
}
```

> `appsettings.Development.json` is gitignored — never commit it.

### 2. Set environment variable

Set `ASPNETCORE_ENVIRONMENT=Development` on your IIS app pool so that `appsettings.Development.json` is loaded:

```powershell
Import-Module WebAdministration
Set-ItemProperty 'IIS:\AppPools\<your-app-pool>' -Name 'environmentVariables' `
  -Value @{name='ASPNETCORE_ENVIRONMENT';value='Development'}
```

### 3. Build and publish

```
dotnet build src/Foundation/Foundation.csproj -c Release
dotnet publish src/Foundation/Foundation.csproj -c Release -o publish
```

Point your IIS site at the `publish/` directory.

### 4. Database setup (first-time)

Run `setup.cmd` (Windows) or `setup.sh` (Mac/Linux) to create and seed the SQL databases.

```
setup.cmd
```

Default CMS login: **admin@example.com / Episerver123!**

---

## Optimizely DXP hosting

Connection strings and Graph credentials are injected automatically by the DXP platform — no values need to be set in `appsettings.json` for hosted environments.

---

## About Foundation

Foundation offers a starting point that is intuitive, well-structured and modular, allowing developers to explore CMS, Commerce, Personalization, Search and Navigation, Data Platform and Experimentation on the Optimizely platform.

Based on the public [episerver/Foundation](https://github.com/episerver/Foundation) reference implementation.
