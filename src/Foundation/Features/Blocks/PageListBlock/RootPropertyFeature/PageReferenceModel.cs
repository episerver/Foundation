using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.Web;

namespace Foundation.Features.Blocks.PageListBlock.RootPropertyFeature
{
    public class PageReferenceModel
    {
        public PageReference RootPages { get; set; }
        // [UIHint(UIHint.Image)]
        // public ContentReference Image { get; set; }
    }
}