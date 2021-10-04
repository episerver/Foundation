using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Inherited = true)]
    public class DefaultBlockController : AsyncBlockComponent<FoundationBlockData>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(FoundationBlockData currentBlock)
        {
            var model = CreateModel(currentBlock);
            var blockName = currentBlock.GetOriginalType().Name;
            return await Task.FromResult(View(string.Format("~/Features/Blocks/{0}/{1}.cshtml", blockName, blockName), model));
        }

        private static IBlockViewModel<BlockData> CreateModel(BlockData currentBlock)
        {
            var type = typeof(BlockViewModel<>).MakeGenericType(currentBlock.GetOriginalType());
            return Activator.CreateInstance(type, currentBlock) as IBlockViewModel<BlockData>;
        }
    }
}