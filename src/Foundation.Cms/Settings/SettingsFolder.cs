using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using System;

namespace Foundation.Cms.Settings
{
    [ContentType(GUID = "c709627f-ca9f-4c77-b0fb-8563287ebd93")]
    [AvailableContentTypes(Include = new[] { typeof(SettingsBase), typeof(SettingsFolder) })]
    public class SettingsFolder : ContentFolder
    {
        public const string SettingsRootName = "SettingsRoot";
        public static Guid SettingsRootGuid = new Guid("79611ee5-7ddd-4ac8-b00e-5e8e8d2a57ee");

        private Injected<LocalizationService> _localizationService;
        private static Injected<ContentRootService> _rootService;

        public static ContentReference SettingsRoot => GetSettingsRoot();

        public override string Name
        {
            get
            {
                if (ContentLink.CompareToIgnoreWorkID(SettingsRoot))
                {
                    return _localizationService.Service.GetString("/contentrepositories/globalsettings/Name");
                }
                return base.Name;
            }
            set => base.Name = value;
        }

        private static ContentReference GetSettingsRoot() => _rootService.Service.Get(SettingsRootName);
    }
}
