using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rcp.Utilities.Extensions
{
    public static class LinqExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source,
                                                                        Expression<Func<TSource, TKey>> keySelector,
                                                                        bool Descending)
        {
            return !Descending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source,
                                                                        Expression<Func<TSource, TKey>> keySelector,
                                                                        IComparer<TKey> comparer, bool Descending)
        {
            return !Descending ? source.OrderBy(keySelector, comparer) : source.OrderByDescending(keySelector, comparer);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string memberName,
                                                                   bool ascending = true)
        {
            // Get the expression parameter type
            var typeParams = new ParameterExpression[] { Expression.Parameter(typeof(T), "") };

            // Determine the property field
            var pi = typeof(T).GetProperty(memberName);

            // Build and return the linq query
            return ascending
                ? (IOrderedQueryable<T>)query.Provider.CreateQuery(
                                                                    Expression.Call(
                                                                                    typeof(Queryable),
                                                                                    "OrderBy",
                                                                                    new Type[]
                                                                                    {typeof (T), pi.PropertyType},
                                                                                    query.Expression,
                                                                                    Expression.Lambda(
                                                                                                      Expression
                                                                                                          .Property(
                                                                                                                    typeParams
                                                                                                                        [
                                                                                                                         0
                                                                                                                        ],
                                                                                                                    pi),
                                                                                                      typeParams)))
                : (IOrderedQueryable<T>)query.Provider.CreateQuery(
                                                                    Expression.Call(
                                                                                    typeof(Queryable),
                                                                                    "OrderByDescending",
                                                                                    new Type[]
                                                                                    {typeof (T), pi.PropertyType},
                                                                                    query.Expression,
                                                                                    Expression.Lambda(
                                                                                                      Expression
                                                                                                          .Property(
                                                                                                                    typeParams
                                                                                                                        [
                                                                                                                         0
                                                                                                                        ],
                                                                                                                    pi),
                                                                                                      typeParams)));
        }
    }
}