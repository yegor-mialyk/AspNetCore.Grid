using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class BooleanFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            Object value = null;
            if (String.IsNullOrEmpty(Value))
            {
                if (Nullable.GetUnderlyingType(expression.Type) == null)
                    expression = Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));
            }
            else if ((value = GetTypedValue()) == null)
            {
                return null;
            }

            switch (Method)
            {
                case "equals":
                    return Expression.Equal(expression, Expression.Constant(value, expression.Type));
                case "not-equals":
                    return Expression.NotEqual(expression, Expression.Constant(value, expression.Type));
                default:
                    return null;
            }
        }

        private Object GetTypedValue()
        {
            if ("false".Equals(Value, StringComparison.OrdinalIgnoreCase))
                return false;

            if ("true".Equals(Value, StringComparison.OrdinalIgnoreCase))
                return true;

            return null;
        }
    }
}
