using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumns<out T> : IEnumerable<T> where T : IGridColumn
    {
    }

    public interface IGridColumnsOf<T> : IGridColumns<IGridColumn<T>>
    {
        IGrid<T> Grid { get; set; }

        IGridColumn<T> Add();
        IGridColumn<T> Add<TValue>(Expression<Func<T, TValue>> constraint);

        IGridColumn<T> Insert(Int32 index);
        IGridColumn<T> Insert<TValue>(Int32 index, Expression<Func<T, TValue>> constraint);
    }
}
