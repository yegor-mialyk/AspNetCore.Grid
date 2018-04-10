using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class DateTimeFilter : BaseGridFilter
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
                case "earlier-than":
                    return Expression.LessThan(expression, Expression.Constant(value, expression.Type));
                case "later-than":
                    return Expression.GreaterThan(expression, Expression.Constant(value, expression.Type));
                case "earlier-than-or-equal":
                    return Expression.LessThanOrEqual(expression, Expression.Constant(value, expression.Type));
                case "later-than-or-equal":
                    return Expression.GreaterThanOrEqual(expression, Expression.Constant(value, expression.Type));
                default:
                    return null;
            }
        }

        private Object GetTypedValue()
        {
            if (DateTime.TryParse(Value, out DateTime date))
                return date;

            return null;
        }
    }
}
