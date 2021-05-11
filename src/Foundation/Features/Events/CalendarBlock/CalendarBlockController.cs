using EPiServer;
using EPiServer.Core;
using Foundation.Infrastructure.Cms.Extensions;
using Foundation.Features.Events.CalendarEvent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Events.CalendarBlock
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarBlockController : ControllerBase
    {
        private readonly IContentLoader _contentLoader;

        public CalendarBlockController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        private IEnumerable<CalendarEventPage> GetEvents(int blockId)
        {
            var contentRef = new ContentReference(blockId);
            var currentBlock = _contentLoader.Get<CalendarBlock>(contentRef);
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

            events.Take(currentBlock.Count);

            return events;
        }

        [HttpPost]
        [Route("CalendarEvents")]
        public ContentResult CalendarEvents(CalendarBlockData calendarBlockData)
        {
            var blockId = calendarBlockData.BlockId;
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
        [Route("UpcomingEvents")]
        public ContentResult UpcomingEvents(CalendarBlockData calendarBlockData)
        {
            var blockId = calendarBlockData.BlockId;
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