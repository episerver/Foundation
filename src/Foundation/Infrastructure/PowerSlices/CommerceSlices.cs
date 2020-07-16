using EPiServer.Commerce.Marketing;
using Foundation.Features.CatalogContent.Bundle;
using Foundation.Features.CatalogContent.Package;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Variation;
using PowerSlice;

namespace Foundation.Infrastructure.PowerSlices
{
    public class ProductsSlice : ContentSliceBase<GenericProduct>
    {
        public override string Name => "Products";

        public override int SortOrder => 100;
    }

    public class PackagesSlice : ContentSliceBase<GenericPackage>
    {
        public override string Name => "Packages";

        public override int SortOrder => 101;
    }

    public class BundlesSlice : ContentSliceBase<GenericBundle>
    {
        public override string Name => "Bundles";

        public override int SortOrder => 102;
    }

    public class VariantsSlice : ContentSliceBase<GenericVariant>
    {
        public override string Name => "Variants";

        public override int SortOrder => 103;
    }

    public class OrderPromotionsSlice : ContentSliceBase<OrderPromotion>
    {
        public override string Name => "Order discounts";

        public override int SortOrder => 111;
    }

    public class ShippingPromotionsSlice : ContentSliceBase<ShippingPromotion>
    {
        public override string Name => "Shipping discounts";

        public override int SortOrder => 112;
    }

    public class EntryPromotionsSlice : ContentSliceBase<EntryPromotion>
    {
        public override string Name => "Item discounts";

        public override int SortOrder => 113;
    }
}
