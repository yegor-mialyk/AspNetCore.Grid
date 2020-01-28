using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class StringStartsWithFilter : StringFilter
    {
        public override Expression? Apply(Expression expression)
        {
            if (Values.Count == 0 || Values.Any(String.IsNullOrEmpty))
                return null;

            return base.Apply(expression);
        }

        protected override Expression? Apply(Expression expression, String? value)
        {
            return Expression.AndAlso(
                Expression.NotEqual(expression, Null),
                Expression.Call(ConvertCase(expression), StartsWith, ConvertCase(value)));
        }
    }
}
