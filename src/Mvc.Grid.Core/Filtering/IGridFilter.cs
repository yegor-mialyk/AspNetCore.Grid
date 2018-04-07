using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public interface IGridFilter
    {
        String Value { get; set; }
        String Method { get; set; }

        Expression Apply(Expression expression);
    }
}
