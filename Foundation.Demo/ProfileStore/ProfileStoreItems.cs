using Foundation.Cms.Attributes;
using Foundation.Commerce.Customer.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Foundation.Demo.ProfileStore
{
    public class ProfileStoreItems
    {
        [JsonProperty("items")]
        public List<ProfileStoreModel> ProfileStoreList { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
    }

    public class ProfileStoreModel
    {
        public string ProfileId { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/Name")]
        public string Name { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/ProfileManager")]
        public string ProfileManager { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/FirstSeen")]
        public DateTime FirstSeen { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/LastSeen")]
        public DateTime LastSeen { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/Visits")]
        public int Visits { get; set; }

        public ProfileStoreInformation Info { get; set; }
        public IList<string> ContactInformation { get; set; }
        public IList<object> DeviceIds { get; set; }
        public string Scope { get; set; }
        public Dictionary<string, string> Payload { get; set; }
        public string JsonPayload { get; set; }
        public IEnumerable<CountryViewModel> CountryOptions { get; set; }
    }

    public class ProfileStoreInformation
    {
        [LocalizedDisplay("/ProfileStore/Form/Label/Picture")]
        public string Picture { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/Website")]
        public string Website { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/StreetAddress")]
        public string StreetAddress { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/Phone")]
        public string Phone { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/Mobile")]
        public string Mobile { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/City")]
        public string City { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/State")]
        public string State { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/ZipCode")]
        public string ZipCode { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/JobTitle")]
        public string JobTitle { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/Company")]
        public string Company { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/Country")]
        public string Country { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/InferredCountry")]
        public string InferredCountry { get; set; }

        [LocalizedDisplay("/ProfileStore/Form/Label/Email")]
        public string Email { get; set; }
    }
}