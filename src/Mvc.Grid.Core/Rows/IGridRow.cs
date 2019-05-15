using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRow<out T>
    {
        T Model { get; }
        Int32 Index { get; }

        String CssClasses { get; set; }
        GridHtmlAttributes Attributes { get; set; }
    }
}
