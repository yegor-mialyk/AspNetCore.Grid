using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public abstract class NumberFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            Object value = GetNumericValue();
            if (value == null) return null;

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

        public abstract Object GetNumericValue();
    }
}
