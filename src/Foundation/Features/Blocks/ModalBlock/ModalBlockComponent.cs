using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Foundation.Features.Blocks.ModalBlock
{
    public class ModalBlockComponent : AsyncBlockComponent<ModalBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(ModalBlock currentBlock)
        {
            return await Task.FromResult(View("~/Features/Blocks/ModalBlock/ModalBlock.cshtml", currentBlock));
        }
    }
}