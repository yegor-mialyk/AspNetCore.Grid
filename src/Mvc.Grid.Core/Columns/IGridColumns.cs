using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumns : IEnumerable<IGridColumn>
    {
    }

    public interface IGridColumns<T> : IGridColumns
    {
        IGrid<T> Grid { get; set; }

        IGridColumn<T> Add<TValue>(Expression<Func<T, TValue>> constraint);
        IGridColumn<T> Insert<TValue>(Int32 index, Expression<Func<T, TValue>> constraint);
    }
}
