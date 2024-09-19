namespace NonFactors.Mvc.Grid;

public interface IGridColumn<T> : IGridColumn
{
    IGrid<T> Grid { get; }

    new IGridColumnSort<T> Sort { get; }
    new IGridColumnFilter<T> Filter { get; }
}
