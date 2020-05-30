using EPiServer.Find.Cms;
using EPiServer.Personalization;
using Foundation.Cms.ViewModels;
using Foundation.Find.Cms.Models.Pages;
using System.Collections.Specialized;

namespace Foundation.Find.Cms.People.ViewModels
{
    public class PersonItemViewModel : ContentViewModel<PersonItemPage>
    {
        public PersonItemViewModel(PersonItemPage currentPage) : base(currentPage) { }

    }
}