using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class StringStartsWithFilter : BaseGridFilter
    {
        public override Expression? Apply(Expression expression)
        {
            if (Values.Count == 0 || Values.Any(String.IsNullOrEmpty))
                return null;

            return base.Apply(expression);
        }

        protected override Expression? Apply(Expression expression, String? value)
        {
            Expression expressionValue = Expression.Constant(value?.ToUpper());
            MethodInfo toUpperMethod = typeof(String).GetMethod(nameof(String.ToUpper), Array.Empty<Type>())!;
            MethodInfo startsWithMethod = typeof(String).GetMethod(nameof(String.StartsWith), new[] { typeof(String) })!;

            Expression toUpper = Expression.Call(expression, toUpperMethod);
            Expression startsWith = Expression.Call(toUpper, startsWithMethod, expressionValue);
            Expression notNull = Expression.NotEqual(expression, Expression.Constant(null, expression.Type));

            return Expression.AndAlso(notNull, startsWith);
        }
    }
}
