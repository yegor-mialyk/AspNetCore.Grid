using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridSort<T> : IGridProcessor<T>
    {
        IGrid<T> Grid { get; set; }

        (Int32 Index, GridSortOrder Order)? this[IGridColumn<T> column] { get; }
    }
}
