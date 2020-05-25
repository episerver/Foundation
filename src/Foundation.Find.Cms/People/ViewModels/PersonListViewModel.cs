using EPiServer.Find.Cms;
using Foundation.Cms;
using Foundation.Cms.ViewModels;
using Foundation.Find.Cms.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Find.Cms.People.ViewModels
{
    public class PersonListViewModel : ContentViewModel<PersonListPage>
    {
        public PersonListViewModel(PersonListPage currentPage) : base(currentPage) { }
        public IContentResult<PersonItemPage> Persons { get; set; }
        public List<SelectionItem> Sectors { get; set; }
        public List<SelectionItem> Locations { get; set; }
        public List<string> Names { get; set; }
    }
}