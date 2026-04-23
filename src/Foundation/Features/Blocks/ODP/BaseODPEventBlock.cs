using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.ODP
{
    public class BaseODPEventBlock : BlockData
    {
        [CultureSpecific]
        [Display(Name = "Show snippet on in edit mode",
         Description = "Check this box to display the sample snippet when editing page",
         Order = 100,
         GroupName = SystemTabNames.Content)]
        public virtual bool ShowSnippetEditMode { get; set; }

        [CultureSpecific]
        [Display(Name = "Show snippet on frond end",
            Description = "Check this box to display the sample snippet in front end",
            Order = 110,
            GroupName = SystemTabNames.Content)]
        public virtual bool ShowSnippetFrontEnd { get; set; }

        [Display(Name = "Event Time Stamp",
            Description = "No need to set this. This is only used with a Journey Container Block. ",
            Order = 120,
            GroupName = SystemTabNames.Content)]
        public virtual DateTime EventTime { get; set; }

        public virtual string ConvertToEpoch(string timestamp)
        {
            DateTime epochTime = DateTime.Parse("1970-01-01");
            DateTime date = DateTime.Parse(timestamp);
            return date.Subtract(epochTime).TotalSeconds.ToString();
        }

        public virtual bool HasEventDate(DateTime eventtime)
        {
            //if the event year is 1, then the date field is likely not set
            return eventtime.Year != 1;
        }

    }

    
}
