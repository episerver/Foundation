using EPiServer;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Events.CalendarBlock
{
    [TemplateDescriptor(Default = true)]
    public class CalendarBlockController : BlockController<Cms.Blocks.CalendarBlock>
    {
        private readonly IContentLoader _contentLoader;

        public CalendarBlockController(IContentLoader contentLoader) => _contentLoader = contentLoader;

        public override ActionResult Index(Cms.Blocks.CalendarBlock currentBlock)
        {
            var events = FindEvents(currentBlock);

            if (currentBlock.ViewMode.Equals("List"))
            {
                events = events.Where(x => x.EventStartDate >= DateTime.Now).OrderBy(x => x.EventStartDate).Take(currentBlock.Count == 0 ? 5 : currentBlock.Count);
            }

            var model = new CalendarBlockViewModel(currentBlock)
            {
                Events = events
            };

            ViewData.GetEditHints<CalendarBlockViewModel, Cms.Blocks.CalendarBlock>()
                .AddConnection(x => x.ViewMode, x => x.ViewMode);

            if (currentBlock.ViewMode.Equals("List"))
            {
                return PartialView("~/Features/Events/CalendarBlock/Agenda.cshtml", model);
            }
            else
            {
                return PartialView(model);
            }
        }

        private IEnumerable<CalendarEventPage> FindEvents(Cms.Blocks.CalendarBlock currentBlock)
        {
            IEnumerable<CalendarEventPage> events;
            var root = currentBlock.EventsRoot;
            if (currentBlock.Recursive)
            {
                events = root.GetAllRecursively<CalendarEventPage>();
            }
            else
            {
                events = _contentLoader.GetChildren<CalendarEventPage>(root);
            }

            if (currentBlock.CategoryFilter != null && currentBlock.CategoryFilter.Any())
            {
                events = events.Where(x => x.Category.Intersect(currentBlock.CategoryFilter).Any());
            }
            return events;
        }
    }
}