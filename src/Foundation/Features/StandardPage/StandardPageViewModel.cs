using EPiServer.DataAbstraction;
using Foundation.Features.Shared;
using System.Linq;

namespace Foundation.Features.StandardPage
{
    public class StandardPageViewModel : ContentViewModel<StandardPage>
    {
        public string CategoryName { get; set; }

        public StandardPageViewModel(StandardPage currentPage) : base(currentPage)
        {
        }

        public static StandardPageViewModel Create(StandardPage currentPage, CategoryRepository categoryRepository)
        {
            var model = new StandardPageViewModel(currentPage);
            if (currentPage.Category.Any())
            {
                model.CategoryName = categoryRepository.Get(currentPage.Category.FirstOrDefault()).Description;
            }
            return model;
        }
    }
}