using System;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumnSort
    {
        Int32? Index { get; }
        GridSortOrder? Order { get; }
        Boolean? IsEnabled { get; set; }
        GridSortOrder FirstOrder { get; set; }
    }
    public interface IGridColumnSort<T> : IGridColumnSort
    {
        IQueryable<T> By(IQueryable<T> items);
        IQueryable<T> ThenBy(IOrderedQueryable<T> items);
    }
    public interface IGridColumnSort<T, TValue> : IGridColumnSort<T>
    {
        IGridColumn<T, TValue> Column { get; set; }
    }
}
