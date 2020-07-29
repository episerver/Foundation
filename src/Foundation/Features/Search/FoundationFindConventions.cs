using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;
using EPiServer.Find.ClientConventions;
using EPiServer.Find.Cms;
using EPiServer.Find.Cms.Conventions;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework;
using Foundation.Commerce.Extensions;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Infrastructure;

namespace Foundation.Features.Search
{
    public class FoundationFindConventions : CatalogContentClientConventions
    {
        protected override void ApplyProductContentConventions(EPiServer.Find.ClientConventions.TypeConventionBuilder<ProductContent> conventionBuilder)
        {
            base.ApplyProductContentConventions(conventionBuilder);

            conventionBuilder
                .ExcludeField(x => x.Variations())
                .IncludeField(x => x.VariationModels());

            conventionBuilder.IncludeField(x => x.DefaultPrice())
                .IncludeField(x => x.Prices())
                .IncludeField(x => x.Inventories())
                .IncludeField(x => x.Outline())
                .IncludeField(x => x.SortOrder())
                ;
        }

        protected override void ApplyBundleContentConventions(EPiServer.Find.ClientConventions.TypeConventionBuilder<BundleContent> conventionBuilder)
        {
            base.ApplyBundleContentConventions(conventionBuilder);

            conventionBuilder.IncludeField(x => x.DefaultPrice())
                .IncludeField(x => x.Prices())
                .IncludeField(x => x.Inventories())
                .IncludeField(x => x.Outline())
                .IncludeField(x => x.SortOrder());
        }

        protected override void ApplyPackageContentConventions(EPiServer.Find.ClientConventions.TypeConventionBuilder<PackageContent> conventionBuilder)
        {
            base.ApplyPackageContentConventions(conventionBuilder);
            conventionBuilder.ExcludeField(x => IPricingExtensions.DefaultPrice(x));
            conventionBuilder.IncludeField(x => Foundation.Commerce.Extensions.EntryContentBaseExtensions.DefaultPrice(x))
                .IncludeField(x => x.Outline())
                .IncludeField(x => x.SortOrder());
        }

        public override void ApplyConventions(IClientConventions clientConventions)
        {
            base.ApplyConventions(clientConventions);
            ContentIndexer.Instance.Conventions.ForInstancesOf<GenericVariant>().ShouldIndex(x => false);
            SearchClient.Instance.Conventions.ForInstancesOf<GenericProduct>().IncludeField(x => x.AvailableSizes());
            SearchClient.Instance.Conventions.ForInstancesOf<GenericProduct>().IncludeField(x => x.AvailableColors());
            SearchClient.Instance.Conventions.NestedConventions.ForInstancesOf<GenericProduct>().Add(v => v.VariationModels());
        }
    }
}