using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using System;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.Common
{
    [TemplateDescriptor(Default = true, Inherited = true)]
    public class DefaultBlockController : BlockController<FoundationBlockData>
    {
        public override ActionResult Index(FoundationBlockData currentBlock)
        {
            var model = CreateModel(currentBlock);
            var blockName = currentBlock.GetOriginalType().Name;
            return PartialView(string.Format("~/Features/Blocks/{0}/{1}.cshtml", blockName, blockName), model);
        }

        private static IBlockViewModel<BlockData> CreateModel(BlockData currentBlock)
        {
            var type = typeof(BlockViewModel<>).MakeGenericType(currentBlock.GetOriginalType());
            return Activator.CreateInstance(type, currentBlock) as IBlockViewModel<BlockData>;
        }
    }
}