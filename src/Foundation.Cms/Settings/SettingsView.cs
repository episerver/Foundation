using EPiServer.Framework.Localization;
using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;
using EPiServer.Shell.ViewComposition.Containers;
using EPiServer.Web.Routing;
using System.Collections.Generic;

namespace Foundation.Cms.Settings
{

    [CompositeView]
    public class SettingsView : ICompositeView,
                                IRoutable,
                                IRestrictedComponentCategoryDefinition
    {
        public static readonly string ViewName = "/episerver/cms/settings";
        private readonly LocalizationService _localizationService;
        private readonly ISettingsService _settingsService;
        private IContainer _rootContainer;

        private string routeSegment;

        public SettingsView(LocalizationService localizationService, ISettingsService settingsService)
        {
            _localizationService = localizationService;
            _settingsService = settingsService;
        }

        public string DefaultContext
        {
            get
            {
                var defaultContext = _settingsService.GlobalSettingsRoot;
                return defaultContext.GetUri(false).ToString();
            }
        }

        public string Name => ViewName;

        public IContainer RootContainer
        {
            get
            {
                if (_rootContainer != null)
                {
                    return _rootContainer;
                }

                var navigation = new PinnablePane().Add(
                    new ComponentPaneContainer { ContainerType = ContainerType.System }.Add(
                        new GlobalSettingsComponent().CreateComponent()));

                var tools = new PinnablePane().Add(
                        new ComponentPaneContainer
                        {
                            PlugInArea = PlugInArea.Assets,
                            ContainerType = ContainerType.User
                        }
                            .Add(new ComponentGroup() { PlugInArea = PlugInArea.AssetsDefaultGroup, ContainerType = ContainerType.System, SortOrder = 10, HeadingLocalizationKey = "/episerver/cms/homeview/assets" })
                    );

                var content = new BorderContainer()
                    .Add(
                        new ContentPane { PlugInArea = "/episerver/cms/action" },
                        new BorderSettingsDictionary(region: BorderContainerRegion.Top)).Add(
                        new ContentPane { PlugInArea = "/episerver/cms/maincontent" },
                        new BorderSettingsDictionary(region: BorderContainerRegion.Center));

                _rootContainer = new BorderContainer()
                    .Add(navigation, new BorderSettingsDictionary(
                        BorderContainerRegion.Leading,
                        new Setting("minSize", 305),
                        new Setting("splitter", "true"),
                        new Setting("liveSplitters", "false"),
                        new Setting("id", "navigation")))
                    .Add(content, new BorderSettingsDictionary(region: BorderContainerRegion.Center))
                    .Add(tools, new BorderSettingsDictionary(
                        BorderContainerRegion.Trailing,
                        400,
                        305,
                        null,
                        new Setting("splitter", "true"),
                        new Setting("liveSplitters", "false"),
                        new Setting("id", "tools")));

                _rootContainer.Settings["id"] = Name + "_rootContainer";
                return _rootContainer;
            }
        }

        public string RouteSegment
        {
            get => routeSegment ?? (routeSegment = "settings");

            set => routeSegment = value;
        }

        public string Title => "Settings";

        public ICompositeView CreateView()
        {
            return new SettingsView(
                localizationService: _localizationService,
                settingsService: _settingsService);
        }

        public IEnumerable<string> GetComponentCategories() => new string[] { };
    }
}