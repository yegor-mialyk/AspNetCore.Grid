using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class StringNotEqualsFilter : StringFilter
    {
        protected override Expression? Apply(Expression expression, String? value)
        {
            if (String.IsNullOrEmpty(value))
                return Expression.AndAlso(
                    Expression.NotEqual(expression, Null),
                    Expression.NotEqual(expression, Empty));

            return Expression.OrElse(
                Expression.Equal(expression, Null),
                Expression.NotEqual(ConvertCase(expression), ConvertCase(value)));
        }
    }
}
