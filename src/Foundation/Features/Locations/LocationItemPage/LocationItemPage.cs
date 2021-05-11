using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Locations.LocationItemPage
{
    [ContentType(DisplayName = "Location Item Page",
        GUID = "ac26ee4b-104f-4719-8aab-ad6d3fcb0d75",
        Description = "Used to display the details of a location",
        GroupName = TabNames.Location)]
    [ImageUrl("/icons/cms/pages/cms-icon-page-27.png")]
    public class LocationItemPage : FoundationPageData
    {
        [StringLength(5000)]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Intro text", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string MainIntro { get; set; }

        [Required]
        [UIHint(UIHint.Image)]
        [Display(GroupName = SystemTabNames.Content, Order = 110)]
        public virtual ContentReference Image { get; set; }

        [Display(Name = "Left content area", GroupName = SystemTabNames.Content, Order = 220)]
        public virtual ContentArea LeftContentArea { get; set; }

        [Display(Name = "New location", GroupName = SystemTabNames.Content, Order = 230)]
        public virtual bool New { get; set; }

        [Display(Name = "Promoted location",
            Description = "Check this, in order to boost this destination and promote it in suggestions",
            GroupName = SystemTabNames.Content,
            Order = 240)]
        public virtual bool Promoted { get; set; }

        [Required]
        [BackingType(typeof(PropertyString))]
        [Display(GroupName = TabNames.Location, Order = 10)]
        public virtual string Continent { get; set; }

        [Required]
        [BackingType(typeof(PropertyString))]
        [Display(GroupName = TabNames.Location, Order = 20)]
        public virtual string Country { get; set; }

        [Required]
        [Display(GroupName = TabNames.Location, Order = 30)]
        public virtual double Latitude { get; set; }

        [Required]
        [Display(GroupName = TabNames.Location, Order = 40)]
        public virtual double Longitude { get; set; }

        [Display(Name = "Average temperature", GroupName = TabNames.Location, Order = 50)]
        public virtual double? AvgTemp { get; set; }

        [BackingType(typeof(PropertyString))]
        [Display(Name = "Airport initials", GroupName = TabNames.Location, Order = 60)]
        public virtual string AirportInitials { get; set; }

        [Display(Name = "Yearly passengers", GroupName = TabNames.Location, Order = 70)]
        public virtual int YearlyPassengers { get; set; }

        [Ignore]
        public double AvgTempDbl => AvgTemp ?? double.NaN;

        [Ignore]
        public string SearchHitTypeName => "Destination";

        [Ignore]
        public GeoLocation Coordinates => new GeoLocation(Latitude, Longitude);

        public List<string> TagString()
        {
            //var repo = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
            //return Categories?.Select(category => repo.Get<StandardCategory>(category).Name).ToList();
            return new List<string>();
        }

        //public override void SetItem(ItemModel itemModel)
        //{
        //    itemModel.Description = MainIntro;
        //    itemModel.Image = Image;
        //}
    }
}