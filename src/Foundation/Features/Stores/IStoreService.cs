using System.Collections.Generic;

namespace Foundation.Features.Stores
{
    public interface IStoreService
    {
        List<StoreItemViewModel> GetEntryStoresViewModels(string entryCode);
        List<StoreItemViewModel> GetAllStoreViewModels();
        StoreItemViewModel GetCurrentStoreViewModel();
        bool SetCurrentStore(string storeCode);
    }
}
