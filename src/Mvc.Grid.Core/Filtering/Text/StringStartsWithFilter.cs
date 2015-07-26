using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class StringStartsWithFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            MethodInfo startsWithMethod = typeof(String).GetMethod("StartsWith", new[] { typeof(String) });
            MethodInfo toUpperMethod = typeof(String).GetMethod("ToUpper", new Type[0]);
            Expression value = Expression.Constant(Value.ToUpper());

            Expression notNull = Expression.NotEqual(expression, Expression.Constant(null));
            Expression toUpper = Expression.Call(expression, toUpperMethod);

            Expression startsWith = Expression.Call(toUpper, startsWithMethod, value);

            return Expression.AndAlso(notNull, startsWith);
        }
    }
}
