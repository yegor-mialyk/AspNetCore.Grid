namespace NonFactors.Mvc.Grid;

public interface IGrid<T> : IGrid
{
    IGridSort<T> Sort { get; set; }
    IQueryable<T> Source { get; set; }
    HashSet<IGridProcessor<T>> Processors { get; set; }

    new IGridColumnsOf<T> Columns { get; }
    new IGridRowsOf<T> Rows { get; }
    new IGridPager<T>? Pager { get; set; }
}
