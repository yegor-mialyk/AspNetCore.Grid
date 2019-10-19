using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public abstract class BaseGridFilter : IGridFilter
    {
        public String? Method { get; set; }
        public StringValues Values { get; set; }

        public virtual Expression? Apply(Expression expression)
        {
            Expression? filter = Apply(expression, Values.FirstOrDefault());

            for (Int32 i = 1; i < Values.Count; i++)
                if (Apply(expression, Values[i]) is Expression next)
                    filter = filter == null
                        ? next
                        : Method == "not-equals"
                            ? Expression.AndAlso(filter, next)
                            : Expression.OrElse(filter, next);

            return filter;
        }
        protected abstract Expression? Apply(Expression expression, String? value);
    }
}
