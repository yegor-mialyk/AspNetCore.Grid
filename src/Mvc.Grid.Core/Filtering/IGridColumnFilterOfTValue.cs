namespace NonFactors.Mvc.Grid;

public interface IGridColumnFilter<T, TValue> : IGridColumnFilter<T>
{
    IGridColumn<T, TValue> Column { get; set; }
}
