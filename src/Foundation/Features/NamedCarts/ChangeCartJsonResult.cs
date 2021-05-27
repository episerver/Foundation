using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Features.NamedCarts
{
    public class ChangeCartJsonResult
    {
        /// <summary>
        /// Status = 0 then return Warning, 1 return Success, -1 return Error (use in Product.js, function addToCart)
        /// </summary>
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int CountItems { get; set; }
        public Money? SubTotal { get; set; }

        // for large cart
        public Money? TotalDiscount { get; set; }
        public Money? Total { get; set; }
        public Money? ShippingTotal { get; set; }
        public Money? TaxTotal { get; set; }
    }

    public class RequestParamsToCart
    {
        public string Code { get; set; }
        public int ShipmentId { get; set; }
        public decimal Quantity { get; set; } = 1;
        public string Size { get; set; } = null;
        public string NewSize { get; set; } = null;

        // for Add to cart
        public string Store { get; set; } = "delivery";
        public string SelectedStore { get; set; } = "";
        public string RequestFrom { get; set; } = "";

        // for SharedCart 
        public string OrganizationId { get; set; }

        // for Checkout Separate shipment
        public int ToShipmentId { get; set; }
        public string DeliveryMethodId { get; set; }

        // for DynamicProduct 
        public List<string> DynamicCodes { get; set; }
    }
}