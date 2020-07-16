using EPiServer.Commerce.Order;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.Services
{
    public interface ICartService
    {
        ShippingMethodInfoModel InStorePickupInfoModel { get; }
        AddToCartResult AddToCart(ICart cart, string code, decimal quantity, string deliveryMethod, string warehouseCode);
        Dictionary<ILineItem, List<ValidationIssue>> ChangeCartItem(ICart cart, int shipmentId, string code, decimal quantity, string size, string newSize);
        void SetCartCurrency(ICart cart, Currency currency);
        Dictionary<ILineItem, List<ValidationIssue>> ValidateCart(ICart cart);
        Dictionary<ILineItem, List<ValidationIssue>> RequestInventory(ICart cart);
        string DefaultCartName { get; }
        string DefaultWishListName { get; }
        string DefaultSharedCartName { get; }
        string DefaultOrderPadName { get; }
        CartWithValidationIssues LoadCart(string name, bool validate);
        CartWithValidationIssues LoadCart(string name, string contactId, bool validate);
        ICart LoadOrCreateCart(string name);
        ICart LoadOrCreateCart(string name, string contactId);
        bool AddCouponCode(ICart cart, string couponCode);
        void RemoveCouponCode(ICart cart, string couponCode);
        void RecreateLineItemsBasedOnShipments(ICart cart, IEnumerable<CartItemViewModel> cartItems, IEnumerable<AddressModel> addresses);
        void MergeShipments(ICart cart);
        ICart LoadWishListCardByCustomerId(Guid currentContactId);
        ICart LoadSharedCardByCustomerId(Guid currentContactId);
        Dictionary<ILineItem, List<ValidationIssue>> ChangeQuantity(ICart cart, int shipmentId, string code, decimal quantity);
        Money? GetDiscountedPrice(ICart cart, ILineItem lineItem);
        ICart CreateNewCart();
        void DeleteCart(ICart cart);
        bool PlaceCartForQuote(ICart cart);
        ICart PlaceOrderToCart(IPurchaseOrder purchaseOrder, ICart cart);
        void RemoveQuoteNumber(ICart cart);
        int PlaceCartForQuoteById(int orderId, Guid userId);
        AddToCartResult SeparateShipment(ICart cart, string code, int quantity, int fromShipmentId, int toShipmentId, string deliveryMethodId, string warehouseCode);
    }
}