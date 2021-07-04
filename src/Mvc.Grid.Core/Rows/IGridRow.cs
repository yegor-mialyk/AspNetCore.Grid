using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridRow<out T>
    {
        T Model { get; }
        Int32 Index { get; }

        GridHtmlAttributes? Attributes { get; set; }
    }
}
