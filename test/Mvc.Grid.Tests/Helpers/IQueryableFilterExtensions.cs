using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid.Tests
{
    public static class IQueryableFilterExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> items, LambdaExpression expression, IGridFilter filter)
        {
            if (filter.Apply(expression.Body) is Expression filterExpression)
                return items.Where(Expression.Lambda<Func<T, Boolean>>(filterExpression, expression.Parameters[0]));

            return items;
        }
    }
}
