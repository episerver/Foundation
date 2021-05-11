using EPiServer;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Foundation.Features.Events.CalendarBlock
{
    public class CalendarBlockComponent : AsyncBlockComponent<CalendarBlock>
    {
        private readonly IContentLoader _contentLoader;

        public CalendarBlockComponent(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public override async Task<IViewComponentResult> InvokeAsync(CalendarBlock currentBlock)
        {
            var model = new CalendarBlockViewModel(currentBlock);

            return await Task.FromResult(View("~/Features/CmsPages/Events/CalendarBlock/Views/index.cshtml", model));
        }
    }
}
