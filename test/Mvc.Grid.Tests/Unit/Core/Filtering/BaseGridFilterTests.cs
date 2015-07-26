using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class BaseGridFilterTests
    {
        protected IQueryable<T> Filter<T, P>(IQueryable<T> items, Expression filterExpression, Expression<Func<T, P>> property)
        {
            return items.Where(ToLambda(property, filterExpression));
        }
        private Expression<Func<T, Boolean>> ToLambda<T, P>(Expression<Func<T, P>> property, Expression expression)
        {
            if (property.Body.Type.IsGenericType && property.Body.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Expression notNull = Expression.NotEqual(property.Body, Expression.Constant(null));
                expression = Expression.AndAlso(notNull, expression);
            }

            return Expression.Lambda<Func<T, Boolean>>(expression, property.Parameters[0]);
        }
    }
}
