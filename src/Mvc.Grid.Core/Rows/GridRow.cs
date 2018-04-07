using System;

namespace NonFactors.Mvc.Grid
{
    public class GridRow<T> : IGridRow<T>
    {
        public T Model { get; }
        public String CssClasses { get; set; }
        public GridHtmlAttributes Attributes { get; set; }

        public GridRow(T model)
        {
            Model = model;
        }
    }
}
