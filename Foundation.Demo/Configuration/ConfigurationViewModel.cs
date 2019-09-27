using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.Web;
using Foundation.Demo.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Foundation.Demo.Configuration
{
    public class ConfigurationViewModel
    {
        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Local Site Name")]
        public string LocalSiteName { get; set; }

        [Display(Name = "Site Domain Name")]
        public string SiteDomainName { get; set; }

        [Display(Name = "Choose remote site")]
        public List<AzureBlob> Sites { get; set; }

        [Display(Name = "Choose site")]
        public List<SiteDefinition> LocalSites { get; set; }

        public string SelectedSiteName { get; set; }
        public string LocalSelectedSiteName { get; set; }
        public string SiteImportLocation { get; set; }
        public string SiteExportLocation { get; set; }
        public int SitePage { get; set; } = 1;
        public int SitePageSize { get; set; } = 100;
        public List<HttpPostedFileBase> SiteImportFile { get; set; }


        [Display(Name = "Choose catalog")]
        public List<AzureBlob> Catalogs { get; set; }

        [Display(Name = "Choose catalog")]
        public List<CatalogContent> LocalCatalogs { get; set; }

        public string SelectedCatalogName { get; set; }

        [Display(Name = "Catalog Name")]
        public string LocalCatalogName { get; set; }

        public string LocalSelectedCatalogName { get; set; }

        public int CatalogPage { get; set; } = 1;
        public int CatalogPageSize { get; set; } = 100;
        public string CatalogImportLocation { get; set; }
        public string CatalogExportLocation { get; set; }

        [Display(Name = "Choose catalog asset folder")]
        public string MediaFolder { get; set; }
        public List<HttpPostedFileBase> CatalogImportFile { get; set; }


        [Display(Name = "List of local VisitorGroups")]
        public List<VisitorGroup> LocalVisitorGroups { get; set; }

        [Display(Name = "List of remote VisitorGroups")]
        public List<AzureBlob> VisitorGroups { get; set; }

        public List<string> SelectedVisitorGroupItems { get; set; }
        public string SelectedRemoteVisitorGroup { get; set; }

        [Display(Name = "Visitor Group Name")]
        public string VisitorGroupName { get; set; }
        public int VisitorGroupPage { get; set; } = 1;
        public int VisitorGroupPageSize { get; set; } = 100;
        public string VisitorGroupImportLocation { get; set; }
        public string VisitorGroupExportLocation { get; set; }
        public List<HttpPostedFileBase> VisitorGroupImportFile { get; set; }
    }
}
