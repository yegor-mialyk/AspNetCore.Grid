using System;

namespace NonFactors.Mvc.Grid
{
    public class GridRow<T> : IGridRow<T>
    {
        public String CssClasses { get; set; }
        public T Model { get; set; }

        public GridRow(T model)
        {
            Model = model;
        }
    }
}
