using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Api.Querying.Queries;
using EPiServer.Find.Helpers;
using Foundation.Find.Facets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Find
{
    public static class SearchExtensions
    {
        public static Expression<Func<T, object>> GetTermFacetForResult<T>(string fieldName)
        {
            var paramX = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(paramX, fieldName);
            Expression conversion = Expression.Convert(property, typeof(object));
            var expr = Expression.Lambda<Func<T, object>>(conversion, paramX);
            return expr;
        }

        public static ITypeSearch<TSource> NumericRangeFacetFor<TSource>(this ITypeSearch<TSource> search, string name,
            IEnumerable<NumericRange> range, Type backingType)
        {
            return search.RangeFacetFor(GetTermFacetForResult<TSource>(name),
                NumericRangfeFacetRequestAction(search.Client, name, range, backingType));
        }

        public static ITypeSearch<TSource> NumericRangeFacetFor<TSource>(this ITypeSearch<TSource> search,
            string name,
            IEnumerable<NumericRange> range)
        {
            return search.RangeFacetFor(GetTermFacetForResult<TSource>(name),
                NumericRangfeFacetRequestAction(search.Client, name, range, typeof(double)));
        }

        public static ITypeSearch<TSource> NumericRangeFacetFor<TSource>(this ITypeSearch<TSource> search,
            string name,
            double from,
            double to)
        {
            return search.RangeFacetFor(GetTermFacetForResult<TSource>(name),
                NumericRangfeFacetRequestAction(search.Client, name, from, to, typeof(double)));
        }

        public static ITypeSearch<TSource> TermsFacetFor<TSource>(this ITypeSearch<TSource> search,
            string name,
            int size) => search.TermsFacetFor(name, FacetRequestAction(search.Client, name, size));

        public static ITypeSearch<TSource> TermsFacetForArray<TSource>(this ITypeSearch<TSource> search,
            string name,
            int size) => search.TermsFacetFor(name, FacetRequestActionForField(name, size));

        public static ITypeSearch<TSource> RangeFacetFor<TSource>(this ITypeSearch<TSource> search,
            string name,
            IEnumerable<NumericRange> range,
            Type backingType)
        {
            var fieldName = search.Client.GetFullFieldName(name, backingType);
            var action = NumericRangfeFacetRequestAction(search.Client, name, range, backingType);
            return new Search<TSource, IQuery>(search, context =>
            {
                var facetRequest = new NumericRangeFacetRequest(name)
                {
                    Field = fieldName
                };
                action(facetRequest);
                context.RequestBody.Facets.Add(facetRequest);
            });
        }

        private static Action<NumericRangeFacetRequest> NumericRangfeFacetRequestAction(IClient searchClient,
            string fieldName,
            IEnumerable<NumericRange> range,
            Type type)
        {
            var fullFieldName = GetFullFieldName(searchClient, fieldName, type);

            return x =>
            {
                x.Field = fullFieldName;
                x.Ranges.AddRange(range);
            };
        }

        private static Action<NumericRangeFacetRequest> NumericRangfeFacetRequestAction(IClient searchClient,
            string fieldName,
            double from,
            double to,
            Type type)
        {
            var range = new List<NumericRange>
            {
                new NumericRange(from, to)
            };
            return NumericRangfeFacetRequestAction(searchClient, fieldName, range, type);
        }

        private static Action<TermsFacetRequest> FacetRequestAction(IClient searchClient,
            string fieldName,
            int size)
        {
            var fullFieldName = GetFullFieldName(searchClient, fieldName);
            return FacetRequestActionForField(fullFieldName, size);
        }

        private static Action<TermsFacetRequest> FacetRequestActionForField(string fieldName,
            int size)
        {
            return x =>
            {
                x.Field = fieldName;
                x.Size = size;
            };
        }

        public static string GetFullFieldName(this IClient searchClient,
            string fieldName) => GetFullFieldName(searchClient, fieldName, typeof(string));

        public static string GetFullFieldName(this IClient searchClient,
            string fieldName,
            Type type)
        {
            if (type != null)
            {
                return fieldName + searchClient.Conventions.FieldNameConvention.GetFieldName(
                           Expression.Variable(type, fieldName));
            }

            return fieldName;
        }

        public static ITypeSearch<T> AddStringFilter<T>(this ITypeSearch<T> query,
            string stringFieldValue,
            string fieldName)
        {
            if (stringFieldValue == null)
            {
                throw new ArgumentNullException(nameof(stringFieldValue));
            }

            var fullFieldName = query.Client.GetFullFieldName(fieldName);
            return query.Filter(GetOrFilterForStringList<T>(new List<string>
            {
                stringFieldValue
            }, query.Client, fullFieldName));
        }

        public static ITypeSearch<T> AddStringFilter<T>(this ITypeSearch<T> query,
            List<string> stringFieldValues,
            string fieldName)
        {
            var fullFieldName = query.Client.GetFullFieldName(fieldName);

            if (stringFieldValues != null && stringFieldValues.Any())
            {
                return query.Filter(GetOrFilterForStringList<T>(stringFieldValues, query.Client, fullFieldName));
            }

            return query;
        }

        public static ITypeSearch<T> AddStringListFilter<T>(this ITypeSearch<T> query,
            List<string> stringFieldValues,
            string fieldName)
        {
            if (stringFieldValues != null && stringFieldValues.Any())
            {
                return query.Filter(GetOrFilterForStringList<T>(stringFieldValues, query.Client, fieldName));
            }

            return query;
        }

        private static FilterBuilder<T> GetOrFilterForStringList<T>(IEnumerable<string> fieldValues,
            IClient client,
            string fieldName)
        {
            var filters = fieldValues.Select(s => new TermFilter(fieldName, s)).Cast<Filter>().ToList();

            if (filters.Count == 1)
            {
                return new FilterBuilder<T>(client, filters[0]);
            }

            var orFilter = new OrFilter(filters);
            var filterBuilder = new FilterBuilder<T>(client, orFilter);
            return filterBuilder;
        }

        public static ITypeSearch<T> AddFilterForNumericRange<T>(this ITypeSearch<T> query,
            IEnumerable<SelectableNumericRange> range,
            string fieldName) => AddFilterForNumericRange(query, range, fieldName, typeof(double));

        public static ITypeSearch<T> AddFilterForNumericRange<T>(this ITypeSearch<T> query,
            IEnumerable<SelectableNumericRange> range,
            string fieldName,
            Type type) => query.Filter(GetOrFilterForNumericRange(query, range, fieldName, type));

        private static FilterBuilder<T> GetOrFilterForNumericRange<T>(ITypeSearch<T> query,
            IEnumerable<SelectableNumericRange> range,
            string fieldName,
            Type type)
        {
            // Appends type convention to field name (like "$$string")
            var client = query.Client;
            var fullFieldName = client.GetFullFieldName(fieldName, type);

            var filters = new List<Filter>();
            foreach (var rangeItem in range)
            {
                var rangeFilter = RangeFilter.Create(fullFieldName,
                    rangeItem.From ?? 0,
                    rangeItem.To ?? double.MaxValue);
                rangeFilter.IncludeUpper = false;
                filters.Add(rangeFilter);
            }

            var orFilter = new OrFilter(filters);
            var filterBuilder = new FilterBuilder<T>(client, orFilter);
            return filterBuilder;
        }

        public static ITypeSearch<T> AddFilterForIntList<T>(this ITypeSearch<T> query,
            List<int> categories,
            string fieldName) => categories.Any() ? query.Filter(GetOrFilterForIntList(query, categories, fieldName, null)) : query;

        public static FilterBuilder<T> GetOrFilterForIntList<T>(this ITypeSearch<T> query,
            IEnumerable<int> values,
            string fieldName,
            Type type)
        {
            var client = query.Client;
            var fullFieldName = client.GetFullFieldName(fieldName, type);

            var filters = values.Select(value => new TermFilter(fullFieldName, value)).Cast<Filter>().ToList();

            FilterBuilder<T> filterBuilder;
            if (filters.Count > 1)
            {
                var orFilter = new OrFilter(filters);
                filterBuilder = new FilterBuilder<T>(client, orFilter);
            }
            else
            {
                filterBuilder = new FilterBuilder<T>(client, filters[0]);
            }

            return filterBuilder;
        }

        public static DelegateFilterBuilder Prefix(this IEnumerable<string> value, string prefix) => new DelegateFilterBuilder(field => new PrefixFilter(field, prefix));

        public static DelegateFilterBuilder PrefixCaseInsensitive(this IEnumerable<string> value, string prefix)
        {
            return new DelegateFilterBuilder(field => new PrefixFilter(field, prefix.ToLowerInvariant()))
            {
                FieldNameMethod =
                    (expression, conventions) =>
                        conventions.FieldNameConvention.GetFieldNameForLowercase(expression)
            };
        }

        public static DelegateFilterBuilder Prefix<T>(this IEnumerable<T> value,
            Expression<Func<T, string>> fieldSelector,
            string prefix) => new DelegateFilterBuilder(field => new PrefixFilter(field, prefix))
            {
                FieldNameMethod = (expression, conventions) =>
                {
                    return string.Format("{0}.{1}",
                                  conventions.FieldNameConvention.GetFieldName(expression),
                                  conventions.FieldNameConvention.GetFieldName(fieldSelector));
                }
            };

        public static DelegateFilterBuilder PrefixCaseInsensitive<T>(this IEnumerable<T> value,
            Expression<Func<T, string>> fieldSelector,
            string prefix)
        {
            return new DelegateFilterBuilder(field => new PrefixFilter(field, prefix.ToLowerInvariant()))
            {
                FieldNameMethod = (expression, conventions) =>
                {
                    return string.Format("{0}.{1}",
                                  conventions.FieldNameConvention.GetFieldName(expression),
                                  conventions.FieldNameConvention.GetFieldNameForLowercase(fieldSelector));
                }
            };
        }

        public static ITypeSearch<TSource> TermsFacetFor<TSource>(this ITypeSearch<TSource> search,
           string name,
           Type type,
           Filter filter,
           Action<FacetFilterRequest> facetRequestAction = null,
           int size = 50)
        {
            var fieldName = name;
            if (type != null)
            {
                fieldName = search.Client.GetFullFieldName(name, type);
            }
            return new Search<TSource, IQuery>(search,
                context =>
                {
                    var facetRequest = new TermsFacetFilterRequest(name, filter)
                    {
                        Field = fieldName,
                        Size = size
                    };
                    if (facetRequestAction.IsNotNull())
                    {
                        facetRequestAction(facetRequest);
                    }
                    context.RequestBody.Facets.Add(facetRequest);
                });
        }

        public static ITypeSearch<TSource> RangeFacetFor<TSource>(this ITypeSearch<TSource> search,
            string name,
            Type type,
            Filter filter,
            IEnumerable<NumericRange> range)
        {
            var fieldName = search.Client.GetFullFieldName(name, type);
            var action = NumericRangeFacetRequestAction(search.Client, name, range, type);
            return new Search<TSource, IQuery>(search,
                context =>
                {
                    var facetRequest = new RangeFacetFilterRequest(name, filter)
                    {
                        Field = fieldName
                    };
                    action(facetRequest);
                    context.RequestBody.Facets.Add(facetRequest);
                });
        }

        private static Action<RangeFacetFilterRequest> NumericRangeFacetRequestAction(IClient searchClient,
            string fieldName,
            IEnumerable<NumericRange> range,
            Type type)
        {
            var name = searchClient.GetFullFieldName(fieldName, type);

            return x =>
            {
                x.Field = name;
                x.Ranges.AddRange(range);
            };
        }

        public static ITypeSearch<T> AddWildCardQuery<T>(this ITypeSearch<T> search,
            string query, Expression<Func<T, string>> fieldSelector)
        {
            var fieldName = search.Client.Conventions.FieldNameConvention
                .GetFieldNameForAnalyzed(fieldSelector);
            var wildcardQuery = new WildcardQuery(fieldName, query.ToLowerInvariant());

            return new Search<T, WildcardQuery>(search, context =>
            {
                if (context.RequestBody.Query != null)
                {
                    var boolQuery = new BoolQuery();
                    boolQuery.Should.Add(context.RequestBody.Query);
                    boolQuery.Should.Add(wildcardQuery);
                    boolQuery.MinimumNumberShouldMatch = 1;
                    context.RequestBody.Query = boolQuery;
                }
                else
                {
                    context.RequestBody.Query = wildcardQuery;
                }
            });
        }
    }
}