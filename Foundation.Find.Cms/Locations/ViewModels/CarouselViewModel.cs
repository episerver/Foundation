using EPiServer.Core;
using Foundation.Find.Cms.Models.Blocks;
using System.Collections.Generic;

namespace Foundation.Find.Cms.Locations.ViewModels
{
    public class CarouselViewModel
    {
        public List<CarouselItemBlock> Items { get; set; }

        public ContentReference BackgroundImage { get; set; }
    }
}
