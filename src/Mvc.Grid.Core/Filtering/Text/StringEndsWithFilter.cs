using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class StringEndsWithFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            if (String.IsNullOrEmpty(Value))
                return null;

            Expression value = Expression.Constant(Value.ToUpper());
            MethodInfo toUpperMethod = typeof(String).GetMethod("ToUpper", new Type[0]);
            MethodInfo endsWithMethod = typeof(String).GetMethod("EndsWith", new[] { typeof(String) });

            Expression toUpper = Expression.Call(expression, toUpperMethod);
            Expression endsWith = Expression.Call(toUpper, endsWithMethod, value);
            Expression notNull = Expression.NotEqual(expression, Expression.Constant(null));

            return Expression.AndAlso(notNull, endsWith);
        }
    }
}
