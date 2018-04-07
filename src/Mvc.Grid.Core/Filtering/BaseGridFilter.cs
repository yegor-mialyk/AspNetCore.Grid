using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public abstract class BaseGridFilter : IGridFilter
    {
        public String Value { get; set; }
        public String Method { get; set; }

        public abstract Expression Apply(Expression expression);
    }
}
