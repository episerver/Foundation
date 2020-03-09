using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Cms.Pages;
using System;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Header
{
    public class HeaderViewModel
    {
        public virtual CmsHomePage HomePage { get; set; }
        public ContentReference CurrentContentLink { get; set; }
        public Guid CurrentContentGuid { get; set; }
        public LinkItemCollection UserLinks { get; set; }
        public string Name { get; set; }
        public List<MenuItemViewModel> MenuItems { get; set; }
        public bool IsReadonlyMode { get; set; }
    }
}