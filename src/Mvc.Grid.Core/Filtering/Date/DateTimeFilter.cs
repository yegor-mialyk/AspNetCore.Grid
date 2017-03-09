using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class DateTimeFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            if (!DateTime.TryParse(Value, out DateTime value))
                return null;

            switch (Type)
            {
                case "Equals":
                    return Expression.Equal(expression, Expression.Constant(value, expression.Type));
                case "NotEquals":
                    return Expression.NotEqual(expression, Expression.Constant(value, expression.Type));
                case "LessThan":
                    return Expression.LessThan(expression, Expression.Constant(value, expression.Type));
                case "GreaterThan":
                    return Expression.GreaterThan(expression, Expression.Constant(value, expression.Type));
                case "LessThanOrEqual":
                    return Expression.LessThanOrEqual(expression, Expression.Constant(value, expression.Type));
                case "GreaterThanOrEqual":
                    return Expression.GreaterThanOrEqual(expression, Expression.Constant(value, expression.Type));
                default:
                    return null;
            }
        }
    }
}
