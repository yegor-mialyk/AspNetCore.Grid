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
    }
}
