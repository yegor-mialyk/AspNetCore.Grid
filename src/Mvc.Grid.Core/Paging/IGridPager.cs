using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridPager
    {
        Int32 TotalRows { get; }
        Int32 TotalPages { get; }

        Int32 CurrentPage { get; set; }
        Int32 RowsPerPage { get; set; }

        Int32 FirstDisplayPage { get; }
        Int32 PagesToDisplay { get; set; }

        String CssClasses { get; set; }
        String PartialViewName { get; set; }
    }

    public interface IGridPager<T> : IGridProcessor<T>, IGridPager
    {
        IGrid<T> Grid { get; }
    }
}
