using System;
using System.Linq;
using System.Linq.Expressions;

public static class QueryableExtensions
{
    public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName = "sold", bool descending = false)
    {
        if (string.IsNullOrEmpty(propertyName)) return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var selector = Expression.Lambda(property, parameter);

        var method = descending ? "OrderByDescending" : "OrderBy";
        var types = new Type[] { source.ElementType, property.Type };
        var methodCallExpression = Expression.Call(typeof(Queryable), method, types, source.Expression, selector);

        return source.Provider.CreateQuery<T>(methodCallExpression);
    }
}
