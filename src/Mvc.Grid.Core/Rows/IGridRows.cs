using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRows<out T> : IEnumerable<IGridRow<T>>
    {
    }

    public interface IGridRowsOf<T> : IGridRows<T>
    {
        IGrid<T> Grid { get; }

        Func<T, Object> Attributes { get; set; }
    }
}
