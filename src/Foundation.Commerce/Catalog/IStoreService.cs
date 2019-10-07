using Foundation.Commerce.Catalog.ViewModels;
using System.Collections.Generic;

namespace Foundation.Commerce.Catalog
{
    public interface IStoreService
    {
        List<StoreItemViewModel> GetEntryStoresViewModels(string entryCode);
        List<StoreItemViewModel> GetAllStoreViewModels();
        StoreItemViewModel GetCurrentStoreViewModel();
        bool SetCurrentStore(string storeCode);
    }
}
