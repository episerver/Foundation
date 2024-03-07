﻿using Foundation.Features.CatalogContent;

namespace Foundation.Features.MyOrganization.QuickOrderPage
{
    public class QuickOrderPageViewModel : ContentViewModel<QuickOrderPage>
    {
        public List<ProductTileViewModel> ProductsList { get; set; }
        public List<string> ReturnedMessages { get; set; }
    }
}