using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Calendar Event",
        GUID = "f086fd08-4e54-4eb9-8367-c45630415226",
        Description = "",
        GroupName = "Calendar Event")]
    [ImageUrl("~/assets/icons/cms/pages/calendar.png")]
    public class CalendarEventPage : FoundationPageData
    {

        [CultureSpecific]
        [Display(Name = "Start date", GroupName = SystemTabNames.Content, Order = 200)]
        public virtual DateTime StartDate { get; set; }

        [CultureSpecific]
        [Display(Name = "End date", GroupName = SystemTabNames.Content, Order = 300)]
        public virtual DateTime EndDate { get; set; }

        [CultureSpecific]
        [Display(Name = "Location", GroupName = SystemTabNames.Content, Order = 400)]
        public virtual string Location { get; set; }
    }
}