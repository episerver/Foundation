using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Foundation.Features.Blocks.HtmlBlock
{
    public class HtmlBlockBlockComponent : AsyncBlockComponent<HtmlBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(HtmlBlock currentBlock)
        {
            return await Task.FromResult(View("~/Features/Blocks/HtmlBlock/HtmlBlock.cshtml", currentBlock));
        }
    }
}