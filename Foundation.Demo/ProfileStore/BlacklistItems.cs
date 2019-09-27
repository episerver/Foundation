using Newtonsoft.Json;
using System.Collections.Generic;

namespace Foundation.Demo.ProfileStore
{
    public class BlacklistItems
    {
        [JsonProperty("items")]
        public List<BlacklistModel> BlacklistList { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
    }

    public class BlacklistModel
    {
        public string BlacklistId { get; set; }
        public string Email { get; set; }
        public string Scope { get; set; }
    }
}