using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.PlugIn;
using Newtonsoft.Json;

namespace Foundation.Features.Blocks.PageListBlock.RootPropertyFeature
{
    [PropertyDefinitionTypePlugIn]
    public class RootProperty : PropertyList<PageReferenceModel>
    {
        protected override PageReferenceModel ParseItem(string value)
        {
            return JsonConvert.DeserializeObject<PageReferenceModel>(value);
        }
    }
}