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

        protected override async Task<IViewComponentResult> InvokeComponentAsync(CalendarBlock currentBlock)
        {
            var model = new CalendarBlockViewModel(currentBlock);

            return await Task.FromResult(View("~/Features/CmsPages/Events/CalendarBlock/Views/index.cshtml", model));
        }
    }
}
