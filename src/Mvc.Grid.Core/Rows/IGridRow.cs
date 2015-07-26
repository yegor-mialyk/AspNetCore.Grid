using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRow
    {
        Object Model { get; }
        String CssClasses { get; set; }
    }
}
