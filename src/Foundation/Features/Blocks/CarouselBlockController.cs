using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Cms.Blocks;
using Foundation.Cms.Media;
using Foundation.Cms.ViewModels;
using Foundation.Cms.ViewModels.Blocks;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class CarouselBlockController : BlockController<CarouselBlock>
    {
        private readonly IContentLoader _contentLoader;

        public CarouselBlockController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public override ActionResult Index(CarouselBlock currentBlock)
        {
            var model = new CarouselBlockViewModel(currentBlock);

            if (currentBlock.CarouselItems != null)
            {
                foreach (var contentAreaItem in currentBlock.CarouselItems.FilteredItems)
                {
                    var carouselItem = _contentLoader.Get<IContentData>(contentAreaItem.ContentLink);

                    if (carouselItem is ImageMediaData)
                    {
                        var carouselImage = new CarouselImage()
                        {
                            Heading = ((ImageMediaData)carouselItem).Title,
                            Description = ((ImageMediaData)carouselItem).Description,
                            Image = ((ImageMediaData)carouselItem).ContentLink
                        };

                        model.Items.Add(new CarouselItem() { CarouselImage = carouselImage });
                    }
                    else if (carouselItem is HeroBlock)
                    {
                        model.Items.Add(new CarouselItem() { HeroBlock = new BlockViewModel<HeroBlock>((HeroBlock)carouselItem) });
                    }
                }
            }

            return PartialView("~/Features/Blocks/Views/CarouselBlock.cshtml", model);
        }
    }
}
