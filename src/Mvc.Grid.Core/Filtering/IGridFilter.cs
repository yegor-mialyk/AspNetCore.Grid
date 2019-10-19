using Microsoft.Extensions.Primitives;
using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public interface IGridFilter
    {
        String? Method { get; set; }
        StringValues Values { get; set; }

        Expression? Apply(Expression expression);
    }
}
