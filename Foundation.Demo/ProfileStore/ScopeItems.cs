using Newtonsoft.Json;
using System.Collections.Generic;

namespace Foundation.Demo.ProfileStore
{
    public class ScopeItems
    {
        [JsonProperty("items")]
        public List<ScopeModel> ScopeList { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
    }

    public class ScopeModel
    {
        public string ScopeId { get; set; }
        public string Description { get; set; }
    }
}