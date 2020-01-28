using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class EnumFilter : GridFilter
    {
        protected override Expression? Apply(Expression expression, String? value)
        {
            if (String.IsNullOrEmpty(value) && Nullable.GetUnderlyingType(expression.Type) == null)
                expression = Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));

            try
            {
                Object enumValue = TypeDescriptor.GetConverter(expression.Type).ConvertFrom(value);

                return Method switch
                {
                    "not-equals" => Expression.NotEqual(expression, Expression.Constant(enumValue, expression.Type)),
                    "equals" => Expression.Equal(expression, Expression.Constant(enumValue, expression.Type)),
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
