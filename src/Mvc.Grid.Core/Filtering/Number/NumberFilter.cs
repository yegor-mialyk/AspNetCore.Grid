using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class NumberFilter<T> : BaseGridFilter
    {
        protected override Expression? Apply(Expression expression, String? value)
        {
            if (String.IsNullOrEmpty(value) && Nullable.GetUnderlyingType(expression.Type) == null)
                expression = Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));

            try
            {
                Object? numberValue = String.IsNullOrEmpty(value) ? null : TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);

                return Method switch
                {
                    "greater-than-or-equal" => Expression.GreaterThanOrEqual(expression, Expression.Constant(numberValue, expression.Type)),
                    "less-than-or-equal" => Expression.LessThanOrEqual(expression, Expression.Constant(numberValue, expression.Type)),
                    "greater-than" => Expression.GreaterThan(expression, Expression.Constant(numberValue, expression.Type)),
                    "not-equals" => Expression.NotEqual(expression, Expression.Constant(numberValue, expression.Type)),
                    "less-than" => Expression.LessThan(expression, Expression.Constant(numberValue, expression.Type)),
                    "equals" => Expression.Equal(expression, Expression.Constant(numberValue, expression.Type)),
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
