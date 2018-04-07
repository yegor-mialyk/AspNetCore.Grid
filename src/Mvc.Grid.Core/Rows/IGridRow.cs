using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRow<out T>
    {
        T Model { get; }
        String CssClasses { get; set; }
        GridHtmlAttributes Attributes { get; set; }
    }
}
