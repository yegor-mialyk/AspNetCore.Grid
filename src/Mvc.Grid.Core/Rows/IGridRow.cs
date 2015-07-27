using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRow<out T>
    {
        String CssClasses { get; set; }
        T Model { get; }
    }
}
