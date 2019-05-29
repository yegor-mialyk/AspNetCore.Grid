using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class DateTimeFilter : BaseGridFilter
    {
        protected override Expression Apply(Expression expression, String value)
        {
            if (String.IsNullOrEmpty(value) && Nullable.GetUnderlyingType(expression.Type) == null)
                expression = Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));

            try
            {
                Object dateValue = TypeDescriptor.GetConverter(expression.Type).ConvertFrom(value);

                switch (Method)
                {
                    case "equals":
                        return Expression.Equal(expression, Expression.Constant(dateValue, expression.Type));
                    case "not-equals":
                        return Expression.NotEqual(expression, Expression.Constant(dateValue, expression.Type));
                    case "earlier-than":
                        return Expression.LessThan(expression, Expression.Constant(dateValue, expression.Type));
                    case "later-than":
                        return Expression.GreaterThan(expression, Expression.Constant(dateValue, expression.Type));
                    case "earlier-than-or-equal":
                        return Expression.LessThanOrEqual(expression, Expression.Constant(dateValue, expression.Type));
                    case "later-than-or-equal":
                        return Expression.GreaterThanOrEqual(expression, Expression.Constant(dateValue, expression.Type));
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
