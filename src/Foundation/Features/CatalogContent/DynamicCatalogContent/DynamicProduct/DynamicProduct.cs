﻿using EPiServer.Commerce.Catalog.DataAnnotations;
using Foundation.Features.CatalogContent.Product;

namespace Foundation.Features.CatalogContent.DynamicCatalogContent.DynamicProduct
{
    [CatalogContentType(
        GUID = "80f12d6d-4e98-4dcf-8135-fb262ec4eb65",
        MetaClassName = "DynamicProduct",
        DisplayName = "Dynamic Product",
        Description = "Dynamic product supports mutiple options")]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-23.png")]
    public class DynamicProduct : GenericProduct
    {
    }
}