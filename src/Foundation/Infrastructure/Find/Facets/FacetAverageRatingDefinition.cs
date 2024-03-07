﻿using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;

namespace Foundation.Infrastructure.Find.Facets
{
    public class FacetAverageRatingDefinition : FacetDefinition
    {
        private readonly ICurrentMarket _currentMarket;

        public Type BackingType = typeof(double);
        public List<SelectableNumericRange> Range;

        public FacetAverageRatingDefinition(ICurrentMarket currentMarket)
        {
            _currentMarket = currentMarket;
            RenderType = GetType().Name;
            Name = "Facet Test";
            DisplayName = "Facet Test Display Name";
            Range = new List<SelectableNumericRange>();
        }

        public ITypeSearch<T> Filter<T>(ITypeSearch<T> query, List<SelectableNumericRange> numericRanges)
        {
            if (numericRanges != null && numericRanges.Any())
            {
                query = query.AddFilterForNumericRange(numericRanges, FieldName, BackingType);
            }

            return query;
        }

        public override ITypeSearch<T> Facet<T>(ITypeSearch<T> query, Filter filter)
        {
            var range = Range.Where(x => x != null).ToList();
            if (!range.Any())
            {
                return query;
            }

            var convertedRangeList = range.Select(selectableNumericRange => selectableNumericRange.ToNumericRange())
                .ToList();
            return query.RangeFacetFor(FieldName, typeof(double), filter, convertedRangeList);
        }

        public override void PopulateFacet(FacetGroupOption facetGroupOption, Facet facet, string selectedFacets)
        {
            var numericRangeFacet = facet as NumericRangeFacet;
            if (numericRangeFacet == null)
            {
                return;
            }

            facetGroupOption.Facets = numericRangeFacet.Ranges.Select(x => new FacetOption
            {
                Count = x.Count,
                Key = $"{facet.Name}:{GetKey(x)}",
                Name = GetDisplayText(x),
                Selected = selectedFacets != null && selectedFacets.Contains($"{facet.Name}:{GetKey(x)}")
            }).ToList();
        }

        private static string GetKey(NumericRangeResult result)
        {
            var from = result.From == null ? "MIN" : result.From.ToString();
            var to = result.To == null ? "MAX" : result.To.ToString();
            return from + "-" + to;
        }

        private string GetDisplayText(NumericRangeResult result)
        {
            var currency = _currentMarket.GetCurrentMarket().DefaultCurrency;

            var from = result.From == null
                ? new Money(0, currency).ToString()
                : new Money(Convert.ToDecimal(result.From.Value), currency).ToString();

            var to = result.To == null
                ? new Money(10000, currency).ToString()
                : new Money(Convert.ToDecimal(result.To.Value), currency).ToString();

            return from + "-" + to;
        }
    }
}