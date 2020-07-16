using Foundation.Features.Shared;

namespace Foundation.Features.Category
{
    public class StandardCategoryViewModel : ContentViewModel<StandardCategory>
    {
        public StandardCategoryViewModel() { }
        public StandardCategoryViewModel(StandardCategory category) : base(category) { }
    }
}
