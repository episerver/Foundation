using EPiServer.Core;
using Foundation.Cms.ViewModels;
using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.ViewModels
{
    public abstract class CartViewModelBase<T> : ContentViewModel<T> where T : IContent
    {
        protected CartViewModelBase(T content) : base(content)
        {
        }

        public decimal ItemCount { get; set; }

        public IEnumerable<CartItemViewModel> CartItems { get; set; }

        public Money Total { get; set; }

        public bool HasOrganization { get; set; }
    }
}