using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class EnumFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            if (String.IsNullOrEmpty(Value))
                return null;

            try
            {
                Object value = TypeDescriptor.GetConverter(expression.Type).ConvertFrom(Value);

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
            catch
            {
                return null;
            }
        }
    }
}
