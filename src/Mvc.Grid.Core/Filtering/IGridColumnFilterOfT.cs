namespace NonFactors.Mvc.Grid;

public interface IGridColumnFilter<T> : IGridColumnFilter
{
    IQueryable<T> Apply(IQueryable<T> items);
}
