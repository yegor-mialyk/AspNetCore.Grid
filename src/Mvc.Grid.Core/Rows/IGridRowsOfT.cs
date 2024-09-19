namespace NonFactors.Mvc.Grid;

public interface IGridRowsOf<T> : IGridRows<T>
{
    IGrid<T> Grid { get; }

    Func<T, Object>? Attributes { get; set; }
}
