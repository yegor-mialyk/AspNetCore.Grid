using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class BooleanFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            Object value = GetBooleanValue();

            return value == null ? null : Expression.Equal(expression, Expression.Constant(value, expression.Type));
        }

        private Object GetBooleanValue()
        {
            if (String.Equals(Value, "true", StringComparison.OrdinalIgnoreCase))
                return true;

            if (String.Equals(Value, "false", StringComparison.OrdinalIgnoreCase))
                return false;

            return null;
        }
    }
}
