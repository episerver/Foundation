using Foundation.Commerce.Customer.ViewModels;
using System;
using System.Collections.Generic;

namespace Foundation.Commerce.Customer.Services
{
    public interface IBookmarksService
    {
        void Add(Guid contentGuid);
        void Remove(Guid contentGuid);
        List<BookmarkModel> Get();
    }
}
