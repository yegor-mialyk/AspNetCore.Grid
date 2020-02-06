using Microsoft.Extensions.Primitives;
using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public abstract class GridFilter : IGridFilter
    {
        public String? Method { get; set; }
        public StringValues Values { get; set; }
        public GridFilterCase Case { get; set; }

        protected GridFilter()
        {
            Case = GridFilterCase.Original;
        }

        public virtual Expression? Apply(Expression expression)
        {
            Expression? filter = null;

            foreach (String value in Values)
                if (Apply(expression, value) is Expression next)
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
