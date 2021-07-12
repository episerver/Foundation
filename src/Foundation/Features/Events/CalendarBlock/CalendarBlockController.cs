using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Cms.Extensions;
using Foundation.Features.Events.CalendarEvent;
using Geta.EpiCategories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Events.CalendarBlock
{
    [TemplateDescriptor(Default = true)]
    public class CalendarBlockController : BlockController<CalendarBlock>
    {
        private readonly IContentLoader _contentLoader;

        public CalendarBlockController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public override ActionResult Index(CalendarBlock currentBlock)
        {
            var model = new CalendarBlockViewModel(currentBlock);

            return PartialView(model);
        }

        private IEnumerable<CalendarEventPage> GetEvents(int blockId)
        {
            var currentBlock = _contentLoader.Get<CalendarBlock>(new ContentReference(blockId));
            IEnumerable<CalendarEventPage> events;
            var root = currentBlock.EventsRoot;
            if (currentBlock.Recursive)
            {
                events = root.FindPagesByPageType(true, typeof(CalendarEventPage).GetPageType().ID).Select(x => x as CalendarEventPage);
            }
            else
            {
                events = _contentLoader.GetChildren<CalendarEventPage>(root);
            }

            if (currentBlock.CategoryFilter != null && currentBlock.CategoryFilter.Any())
            {
                events = events.Where(x =>
                {
                    var categories = (x as ICategorizableContent)?.Categories;
                    return categories != null &&
                           categories.Intersect(currentBlock.CategoryFilter).Any();
                });
            }

            events.Take(currentBlock.Count);

            return events;
        }

        [HttpPost]
        public ContentResult CalendarEvents(int blockId)
        {
            var events = GetEvents(blockId);
            var result = events.Select(x => new
            {
                title = x.Name,
                start = x.EventStartDate,
                end = x.EventEndDate,
                url = x.LinkURL
            });

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
            };
        }

        [HttpPost]
        public ContentResult UpcomingEvents(int blockId)
        {
            var events = GetEvents(blockId);
            var result = events.Where(x => x.EventStartDate >= DateTime.Now)
                .OrderBy(x => x.EventStartDate)
                .Select(x => new
                {
                    x.Name,
                    x.EventStartDate,
                    x.EventEndDate,
                    x.LinkURL
                });

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
            };
        }
    }
}