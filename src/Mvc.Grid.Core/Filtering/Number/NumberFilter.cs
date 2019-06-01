using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class NumberFilter<T> : BaseGridFilter
    {
        protected override Expression Apply(Expression expression, String value)
        {
            if (String.IsNullOrEmpty(value) && Nullable.GetUnderlyingType(expression.Type) == null)
                expression = Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));

            try
            {
                Object numberValue = String.IsNullOrEmpty(value) ? null : TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);

                switch (Method)
                {
                    case "equals":
                        return Expression.Equal(expression, Expression.Constant(numberValue, expression.Type));
                    case "not-equals":
                        return Expression.NotEqual(expression, Expression.Constant(numberValue, expression.Type));
                    case "less-than":
                        return Expression.LessThan(expression, Expression.Constant(numberValue, expression.Type));
                    case "greater-than":
                        return Expression.GreaterThan(expression, Expression.Constant(numberValue, expression.Type));
                    case "less-than-or-equal":
                        return Expression.LessThanOrEqual(expression, Expression.Constant(numberValue, expression.Type));
                    case "greater-than-or-equal":
                        return Expression.GreaterThanOrEqual(expression, Expression.Constant(numberValue, expression.Type));
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
