using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class StringNotEqualsFilter : BaseGridFilter
    {
        protected override Expression Apply(Expression expression, String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                Expression notNull = Expression.NotEqual(expression, Expression.Constant(null, expression.Type));
                Expression isNotEmpty = Expression.NotEqual(expression, Expression.Constant(""));

                return Expression.AndAlso(notNull, isNotEmpty);
            }

            Expression expressionValue = Expression.Constant(value.ToUpper());
            MethodInfo toUpperMethod = typeof(String).GetMethod("ToUpper", new Type[0]);

            Expression toUpper = Expression.Call(expression, toUpperMethod);
            Expression notEquals = Expression.NotEqual(toUpper, expressionValue);
            Expression equalsNull = Expression.Equal(expression, Expression.Constant(null, expression.Type));

            return Expression.OrElse(equalsNull, notEquals);
        }
    }
}
