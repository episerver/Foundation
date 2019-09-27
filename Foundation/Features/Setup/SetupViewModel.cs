using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Features.Setup
{
    public class SetupViewModel
    {
        [Display(Name = "Catalog Name")]
        public string CatalogName { get; set; }

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Choose catalog")]
        public List<SelectListItem> Catalogs { get; set; }

        [Display(Name = "Choose site")]
        public List<SelectListItem> Sites { get; set; }

        public string SiteLocation { get; set; }

        public List<HttpPostedFileBase> SiteFile { get; set; }

        public string CatalogLocation { get; set; }

        public List<HttpPostedFileBase> CatalogFile { get; set; }
    }
}