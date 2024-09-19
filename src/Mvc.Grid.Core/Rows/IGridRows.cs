namespace NonFactors.Mvc.Grid;

public interface IGridRows<out T> : IEnumerable<IGridRow<T>>
{
}
