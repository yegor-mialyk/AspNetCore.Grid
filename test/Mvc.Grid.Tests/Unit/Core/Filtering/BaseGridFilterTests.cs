using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class BaseGridFilterTests
    {
        protected IQueryable<T> Filter<T, P>(IQueryable<T> items, Expression expression, Expression<Func<T, P>> property)
        {
            return items.Where(Expression.Lambda<Func<T, Boolean>>(expression, property.Parameters[0]));
        }
    }
}
