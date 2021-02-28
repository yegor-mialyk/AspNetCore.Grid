using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid.Tests
{
    public static class IQueryableFilterExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> items, LambdaExpression expression, IGridFilter filter)
        {
            return items.Where(Expression.Lambda<Func<T, Boolean>>(filter.Apply(expression.Body)!, expression.Parameters[0]));
        }
    }
}
