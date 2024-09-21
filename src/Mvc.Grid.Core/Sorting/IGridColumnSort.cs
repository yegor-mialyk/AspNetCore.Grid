namespace NonFactors.Mvc.Grid;

public interface IGridColumnSort
{
    Int32? Index { get; }
    GridSortOrder? Order { get; }
    Boolean? IsEnabled { get; set; }
    bool IsDefault { get; set; }
    GridSortOrder FirstOrder { get; set; }
}
