using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Cms;
using EPiServer.Personalization;
using Foundation.Cms.ViewModels;
using Foundation.Find.Cms.Models.Pages;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Foundation.Find.Cms.Persons.ViewModels
{
    public class PersonListViewModel : ContentViewModel<PersonListPage>
    {
        public PersonListViewModel(PersonListPage currentPage) : base(currentPage) { }
        public IContentResult<PersonItemPage> Persons { get; set; }
        public List<string> Sectors { get; set; }
        public List<string> Locations { get; set; }
        public List<string> Names { get; set; }
    }
}