using EPiServer;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Globalization;
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
        private readonly LanguageResolver _languageResolver;

        public CarouselBlockController(IContentLoader contentLoader, LanguageResolver languageResolver)
        {
            _contentLoader = contentLoader;
            _languageResolver = languageResolver;
        }

        public override ActionResult Index(CarouselBlock currentBlock)
        {
            var model = new CarouselBlockViewModel(currentBlock);

            foreach (var item in _contentLoader.GetItems(currentBlock.CarouselItems, _languageResolver.GetPreferredCulture()))
            {
                if (item.GetOriginalType().Equals(typeof(ImageMediaData)))
                {
                    var image = _contentLoader.Get<ImageMediaData>(item.ContentLink);
                    var carouselImage = new CarouselImage()
                    {
                        Heading = image.Title,
                        Description = image.Description,
                        Image = image.ContentLink
                    };

                    model.Items.Add(new CarouselItem() { CarouselImage = carouselImage });
                }
                else if (item.GetOriginalType().Equals(typeof(HeroBlock)))
                {
                    var heroBlock = _contentLoader.Get<HeroBlock>(item.ContentLink);
                    model.Items.Add(new CarouselItem() { HeroBlock = new BlockViewModel<HeroBlock>(heroBlock) });
                }
            }
            return PartialView("~/Features/Blocks/Views/CarouselBlock.cshtml", model);
        }
    }
}
