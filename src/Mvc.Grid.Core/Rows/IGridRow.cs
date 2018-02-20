using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRow<out T>
    {
        GridHtmlAttributes Attributes { get; set; }
        String CssClasses { get; set; }
        T Model { get; }
    }
}
