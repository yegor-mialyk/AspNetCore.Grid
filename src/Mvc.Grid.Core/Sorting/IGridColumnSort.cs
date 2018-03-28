using System;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumnSort
    {
        Boolean? IsEnabled { get; set; }

        GridSortOrder? Order { get; set; }
        GridSortOrder? FirstOrder { get; set; }
        GridSortOrder? InitialOrder { get; set; }
    }
    public interface IGridColumnSort<T, TValue> : IGridColumnSort
    {
        IGridColumn<T, TValue> Column { get; set; }

        IQueryable<T> Apply(IQueryable<T> items);
    }
}
