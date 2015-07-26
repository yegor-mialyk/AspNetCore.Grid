using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class StringContainsFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            MethodInfo toUpperMethod = typeof(String).GetMethod("ToUpper", new Type[0]);
            MethodInfo containsMethod = typeof(String).GetMethod("Contains");
            Expression value = Expression.Constant(Value.ToUpper());

            Expression notNull = Expression.NotEqual(expression, Expression.Constant(null));
            Expression toUpper = Expression.Call(expression, toUpperMethod);

            Expression contains = Expression.Call(toUpper, containsMethod, value);

            return Expression.AndAlso(notNull, contains);
        }
    }
}
