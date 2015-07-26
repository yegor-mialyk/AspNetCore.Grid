using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class StringEndsWithFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            MethodInfo endsWithMethod = typeof(String).GetMethod("EndsWith", new[] { typeof(String) });
            MethodInfo toUpperMethod = typeof(String).GetMethod("ToUpper", new Type[0]);
            Expression value = Expression.Constant(Value.ToUpper());

            Expression notNull = Expression.NotEqual(expression, Expression.Constant(null));
            Expression toUpper = Expression.Call(expression, toUpperMethod);

            Expression endsWith = Expression.Call(toUpper, endsWithMethod, value);

            return Expression.AndAlso(notNull, endsWith);
        }
    }
}
