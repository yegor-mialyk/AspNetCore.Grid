using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class BooleanFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            if ("false".Equals(Value, StringComparison.OrdinalIgnoreCase))
                return Expression.Equal(expression, Expression.Constant(false, expression.Type));

            if ("true".Equals(Value, StringComparison.OrdinalIgnoreCase))
                return Expression.Equal(expression, Expression.Constant(true, expression.Type));

            return null;
        }
    }
}
