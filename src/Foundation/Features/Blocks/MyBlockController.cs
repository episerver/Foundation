using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Foundation.Cms.Blocks;

namespace Foundation.Features.Blocks
{
    public class MyBlockController : BlockController<MyBlock>
    {
        public override ActionResult Index(MyBlock currentblock)
        {
            return PartialView("~/Features/Blocks/Views/MyBlock.cshtml", currentblock);
        }
    }
}