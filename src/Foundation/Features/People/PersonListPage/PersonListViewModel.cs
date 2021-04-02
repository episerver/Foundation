using EPiServer.Find.Cms;
using Foundation.Cms;
using Foundation.Features.People.PersonItemPage;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.People.PersonListPage
{
    public class PersonListViewModel : ContentViewModel<PersonList>
    {
        public PersonListViewModel(PersonList currentPage) : base(currentPage) { }
        public IContentResult<PersonPage> Persons { get; set; }
        public List<SelectionItem> Sectors { get; set; }
        public List<SelectionItem> Locations { get; set; }
        public List<string> Names { get; set; }
    }
}