using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.DataAbstraction;
using Foundation.Cms.Pages;

namespace Foundation.Cms.ViewModels
{
    public class ExtendedStandardPageViewModel : ContentViewModel<ExtendedStandardPage>
    {
        public string CategoryName { get; set; }

        public ExtendedStandardPageViewModel(ExtendedStandardPage currentPage) : base(currentPage)
        {

        }


        public static ExtendedStandardPageViewModel Create(ExtendedStandardPage currentPage,
            CategoryRepository categoryRepository)
        {
            var model = new ExtendedStandardPageViewModel(currentPage);
            if (currentPage.Category.Any())
            {
                model.CategoryName = categoryRepository.Get(currentPage.Category.FirstOrDefault()).Description;
            }

            return model;
        }
    }
}
