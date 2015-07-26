﻿using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class BooleanFilter : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            Object value = GetBooleanValue();
            if (value == null) return null;

            return Expression.Equal(expression, Expression.Constant(value, expression.Type));
        }

        private Object GetBooleanValue()
        {
            if (String.Equals(Value, "true", StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (String.Equals(Value, "false", StringComparison.InvariantCultureIgnoreCase))
                return false;

            return null;
        }
    }
}
