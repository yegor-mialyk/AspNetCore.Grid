namespace NonFactors.Mvc.Grid;

public interface IGridColumnsOf<T> : IList<IGridColumn<T>>, IGridColumns<IGridColumn<T>>
{
    IGrid<T> Grid { get; set; }

    IGridColumn<T, Object> Add();
    IGridColumn<T, TValue> Add<TValue>(Expression<Func<T, TValue>> expression);

    IGridColumn<T, Object> Insert(Int32 index);
    IGridColumn<T, TValue> Insert<TValue>(Int32 index, Expression<Func<T, TValue>> expression);
}
