using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Infrastructure.Cms.Extensions
{
    public static class EnumerableExtensions
    {
        public static TType FirstOfType<TType>(this IEnumerable list) => list.OfType<TType>().FirstOrDefault();

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var element in source)
            {
                action(element);
            }
        }
    }
}