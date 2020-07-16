using EPiServer.Core;
using Foundation.Features.Shared;
using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.ViewModels
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