namespace NonFactors.Mvc.Grid;

public interface IGridColumns<out T> : IEnumerable<T> where T : IGridColumn
{
}
