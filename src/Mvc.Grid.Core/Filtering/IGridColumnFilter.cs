using System;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumnFilter
    {
        String Name { get; set; }
        Boolean? IsMulti { get; set; }
        Boolean? IsEnabled { get; set; }

        String Operator { get; set; }
        IGridFilter First { get; set; }
        IGridFilter Second { get; set; }
    }
    public interface IGridColumnFilter<T> : IGridColumnFilter
    {
        IGridColumn<T> Column { get; set; }

        IQueryable<T> Apply(IQueryable<T> items);
    }
}
