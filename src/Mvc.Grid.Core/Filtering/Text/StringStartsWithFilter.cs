using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class StringStartsWithFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            if (String.IsNullOrEmpty(Value))
                return null;

            Expression value = Expression.Constant(Value.ToUpper());
            MethodInfo toUpperMethod = typeof(String).GetMethod("ToUpper", new Type[0]);
            MethodInfo startsWithMethod = typeof(String).GetMethod("StartsWith", new[] { typeof(String) });

            Expression toUpper = Expression.Call(expression, toUpperMethod);
            Expression startsWith = Expression.Call(toUpper, startsWithMethod, value);
            Expression notNull = Expression.NotEqual(expression, Expression.Constant(null, expression.Type));

            return Expression.AndAlso(notNull, startsWith);
        }
    }
}
