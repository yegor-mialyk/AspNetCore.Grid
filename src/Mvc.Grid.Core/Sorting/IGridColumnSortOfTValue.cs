namespace NonFactors.Mvc.Grid;

public interface IGridColumnSort<T, TValue> : IGridColumnSort<T>
{
    IGridColumn<T, TValue> Column { get; set; }
}
