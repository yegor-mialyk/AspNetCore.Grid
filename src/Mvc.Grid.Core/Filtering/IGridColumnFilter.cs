using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumnFilter
    {
        String Operator { get; set; }
        IGridFilter First { get; set; }
        IGridFilter Second { get; set; }
    }
    public interface IGridColumnFilter<T> : IGridColumnFilter, IGridProcessor<T>
    {
        IGridColumn<T> Column { get; set; }
    }
}
