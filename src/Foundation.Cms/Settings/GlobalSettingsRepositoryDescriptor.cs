using EPiServer.Cms.Shell.UI.CompositeViews.Internal;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using System;
using System.Collections.Generic;

namespace Foundation.Cms.Settings
{
    [ServiceConfiguration(typeof(IContentRepositoryDescriptor))]
    public class GlobalSettingsRepositoryDescriptor : ContentRepositoryDescriptorBase
    {
        public static string RepositoryKey => "globalsettings";

        public override IEnumerable<Type> ContainedTypes => new[] { typeof(SettingsBase), typeof(SettingsFolder) };

        public override IEnumerable<Type> CreatableTypes => new[] { typeof(SettingsBase), typeof(SettingsFolder) };

        public override string CustomNavigationWidget => "epi-cms/component/ContentNavigationTree";

        public override string CustomSelectTitle => LocalizationService.Current.GetString("/contentrepositories/globalsettings/customselecttitle");

        public override string Key => RepositoryKey;

        public override IEnumerable<Type> MainNavigationTypes => new[]
        {
            typeof(SettingsBase), typeof(SettingsFolder)
        };

        public override IEnumerable<string> MainViews => new string[1] { HomeView.ViewName };

        public override string Name => LocalizationService.Current.GetString("/contentrepositories/globalsettings/name");


        public override IEnumerable<ContentReference> Roots => new[] { Settings.Service.GlobalSettingsRoot };

        public override string SearchArea => GlobalSettingsSearchProvider.SearchArea;

        public override int SortOrder => 1000;

        private Injected<ISettingsService> Settings { get; set; }
    }
}