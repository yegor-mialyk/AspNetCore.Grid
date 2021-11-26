using Microsoft.Extensions.Primitives;

namespace NonFactors.Mvc.Grid;

public interface IGridFilter
{
    String? Method { get; set; }
    StringValues Values { get; set; }
    GridFilterCase Case { get; set; }

    Expression? Apply(Expression expression);
}
