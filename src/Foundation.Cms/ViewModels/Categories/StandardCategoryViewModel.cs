using Foundation.Cms.Categories;

namespace Foundation.Cms.ViewModels.Categories
{
    public class StandardCategoryViewModel : ContentViewModel<StandardCategory>
    {
        public StandardCategoryViewModel() { }
        public StandardCategoryViewModel(StandardCategory category) : base(category) { }
    }
}
