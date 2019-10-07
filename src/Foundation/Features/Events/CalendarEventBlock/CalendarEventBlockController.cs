using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Events.CalendarEventBlock
{
    [TemplateDescriptor(Default = true)]
    public class CalendarEventBlockController : BlockController<Cms.Blocks.CalendarEventBlock>
    {
        private readonly IContentLoader _contentLoader;

        public CalendarEventBlockController(IContentLoader contentLoader) => _contentLoader = contentLoader;

        public override ActionResult Index(Cms.Blocks.CalendarEventBlock currentBlock)
        {
            var events = FindEvents(currentBlock);

            if (currentBlock.ViewMode.Equals("List"))
            {
                events = events.Where(x => DateTime.Parse(x.GetPropertyValue("StartDate")) >= DateTime.Now).OrderBy(x => x.GetPropertyValue("StartDate")).Take(currentBlock.Count);
            }

            var model = new CalendarEventBlockViewModel(currentBlock)
            {
                Events = events
            };

            ViewData.GetEditHints<CalendarEventBlockViewModel, Cms.Blocks.CalendarEventBlock>()
                .AddConnection(x => x.ViewMode, x => x.ViewMode);

            if (currentBlock.ViewMode.Equals("List"))
            {
                return PartialView("~/Features/Events/CalendarEventBlock/Agenda.cshtml", model);
            }
            else
            {
                return PartialView(model);
            }
        }

        private IEnumerable<CalendarEventPage> FindEvents(Cms.Blocks.CalendarEventBlock currentBlock)
        {
            IEnumerable<CalendarEventPage> events;
            var root = currentBlock.Root;
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