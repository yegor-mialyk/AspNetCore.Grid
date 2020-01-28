using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class DateTimeFilter : GridFilter
    {
        protected override Expression? Apply(Expression expression, String? value)
        {
            if (String.IsNullOrEmpty(value) && Nullable.GetUnderlyingType(expression.Type) == null)
                expression = Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));

            try
            {
                Object dateValue = TypeDescriptor.GetConverter(expression.Type).ConvertFrom(value);

                return Method switch
                {
                    "later-than-or-equal" => Expression.GreaterThanOrEqual(expression, Expression.Constant(dateValue, expression.Type)),
                    "earlier-than-or-equal" => Expression.LessThanOrEqual(expression, Expression.Constant(dateValue, expression.Type)),
                    "later-than" => Expression.GreaterThan(expression, Expression.Constant(dateValue, expression.Type)),
                    "earlier-than" => Expression.LessThan(expression, Expression.Constant(dateValue, expression.Type)),
                    "not-equals" => Expression.NotEqual(expression, Expression.Constant(dateValue, expression.Type)),
                    "equals" => Expression.Equal(expression, Expression.Constant(dateValue, expression.Type)),
                    _ => null
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
