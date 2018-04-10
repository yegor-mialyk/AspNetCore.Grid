using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class StringEqualsFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            if (String.IsNullOrEmpty(Value))
            {
                Expression equalsNull = Expression.Equal(expression, Expression.Constant(null, expression.Type));
                Expression isEmpty = Expression.Equal(expression, Expression.Constant(""));

                return Expression.OrElse(equalsNull, isEmpty);
            }

            MethodInfo toUpperMethod = typeof(String).GetMethod("ToUpper", new Type[0]);
            Expression value = Expression.Constant(Value.ToUpper());

            Expression notNull = Expression.NotEqual(expression, Expression.Constant(null, expression.Type));
            Expression toUpper = Expression.Call(expression, toUpperMethod);
            Expression equals = Expression.Equal(toUpper, value);

            return Expression.AndAlso(notNull, equals);
        }
    }
}
