using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public abstract class NumberFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            Object value = GetNumericValue();
            if (value == null)
                return null;

            switch (Type)
            {
                case "equals":
                    return Expression.Equal(expression, Expression.Constant(value, expression.Type));
                case "not-equals":
                    return Expression.NotEqual(expression, Expression.Constant(value, expression.Type));
                case "less-than":
                    return Expression.LessThan(expression, Expression.Constant(value, expression.Type));
                case "greater-than":
                    return Expression.GreaterThan(expression, Expression.Constant(value, expression.Type));
                case "less-than-or-equal":
                    return Expression.LessThanOrEqual(expression, Expression.Constant(value, expression.Type));
                case "greater-than-or-equal":
                    return Expression.GreaterThanOrEqual(expression, Expression.Constant(value, expression.Type));
                default:
                    return null;
            }
        }

        public abstract Object GetNumericValue();
    }
}
