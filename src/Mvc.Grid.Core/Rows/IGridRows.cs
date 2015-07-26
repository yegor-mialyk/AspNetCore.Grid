using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRows : IEnumerable<IGridRow>
    {
    }

    public interface IGridRows<T> : IGridRows
    {
        IGrid<T> Grid { get; }
        Func<T, String> CssClasses { get; set; }
    }
}
