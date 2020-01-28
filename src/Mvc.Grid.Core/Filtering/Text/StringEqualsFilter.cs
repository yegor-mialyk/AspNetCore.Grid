using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class StringEqualsFilter : StringFilter
    {
        protected override Expression? Apply(Expression expression, String? value)
        {
            if (String.IsNullOrEmpty(value))
                return Expression.OrElse(
                    Expression.Equal(expression, Null),
                    Expression.Equal(expression, Empty));

            return Expression.AndAlso(
                Expression.NotEqual(expression, Null),
                Expression.Equal(ConvertCase(expression), ConvertCase(value)));
        }
    }
}
