<a href="https://github.com/episerver/Foundation"><img src="http://ux.episerver.com/images/logo.png" title="Foundation" alt="Foundation"></a>

## Foundation Net Core

This preview repository is early access to the latest Episerver packages targeting .NET 5.

---

## Prerequisites

[Net 5](https://dotnet.microsoft.com/download/dotnet/5.0) sdk is required to use with visual studio.  Runtime maybe sufficent to just run the application.

---

## The Solution

`Foundation has a default username and password of admin@example.com / Episerver123!`

---

## Installation

The installation files contain a batch file that will install the Foundation project and create database.

Right-click on the batch file called **setup.cmd** and select **Run as administrator**

---

## Configuration

Most of the configuration has been moved to options classes.  The options classes can be configured through code or the appsettings.json configuration file.  For option classes to be automatically configured from `appsettings.json`, please use the `EPiServer.ServiceLocation.OptionsAttribute`.  There is a configuration section which maps to the leaf node in the JSON.

To utilize legacy configuration sections you can install the `EPiServer.Cms.AspNetCore.Migration` package. This is available to ease migration, however we encourage to update the use options or `appsettings.json` if possible.

---

## Startup extensibility

### Program.cs
EPiServer will by default use the built-in Dependency Injection framework (DI) in .NET 5. To connect the DI framework with EPiServer you need to call extension method `IHostBuilder.ConfigureCmsDefault()` in Program.cs. <br/>
To configure the application (including EPiServer) to use another DI framework you should call the extension method `IHostBuilder.UseServiceProviderFactory`. The example below shows how to configure the application to use Autofac:

```
host.UseServiceProviderFactory(context => new  ServiceLocatorProviderFactoryFacade<ContainerBuilder>(context,
    new AutofacServiceProviderFactory()));
```

### Startup.cs
There are some added extensibility points when interacting with the Startup class.
  1.  `services.AddCms();` - This configures than CMS and needs to be called to function properly.
  2.  `endpoints.MapContent();` - This registers EPiServer content routing with the endpoint routing feature.
  3.  `IEndpointRoutingExtension` - Access to the `IEndpointRouteBuilder` to register routes. Convience method `services.AddEndpointRoutingExtension<T>()`
  4.  `IStartupFilter` - Access to IApplicationBuilder if you need to register middleware for instance.  Convience method `services.AddStartupFilter<T>()`
  5.  `IBlockingFirstRequestInitializer` - Use this if you need to do something before the first request
  6.  `IRedirectingFirstRequestInitializer` - Use this if you need to redirect to a page until some type of initialization takes place.

---

## Compiled Views for Shell Modules

For addon developers, we have added a default location expander that will look for compiled views in a certain location or based on configuration value.
  1.  /{ShellModuleName}/Views/
  2.  The folder defined in the module.config viewFolder attribute on module element.

---

## Preview of documentation
For a preview of the documentation for CMS 12, Commerce 14, and Search & Navigation 14, including Breaking changes, see the [.NET 5.0 preview documentation](https://world.episerver.com/documentation/developer-guides/optimizely-platform/-net-core-preview/) on Optimizely World.