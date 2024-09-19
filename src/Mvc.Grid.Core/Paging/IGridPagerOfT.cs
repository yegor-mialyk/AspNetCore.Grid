namespace NonFactors.Mvc.Grid;

public interface IGridPager<T> : IGridProcessor<T>, IGridPager
{
    IGrid<T> Grid { get; }
}
