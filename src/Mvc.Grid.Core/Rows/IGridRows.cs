using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRows<out T> : IEnumerable<IGridRow<T>>
    {
    }

    public interface IGridRowsOf<T> : IGridRows<T>
    {
        Func<T, Object> Attributes { get; set; }
        Func<T, String> CssClasses { get; set; }
        IGrid<T> Grid { get; }
    }
}
