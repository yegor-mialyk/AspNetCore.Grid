using System;

namespace NonFactors.Mvc.Grid
{
    public interface ISortableColumn
    {
        Boolean? IsSortable { get; set; }
        GridSortOrder? SortOrder { get; set; }
        GridSortOrder? FirstSortOrder { get; set; }
        GridSortOrder? InitialSortOrder { get; set; }
    }

    public interface ISortableColumn<T> : IGridProcessor<T>
    {
    }
}
