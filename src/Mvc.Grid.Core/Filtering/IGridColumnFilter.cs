using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumnFilter
    {
        String Name { get; set; }
        Boolean? IsMulti { get; set; }
        Boolean? IsEnabled { get; set; }
        String DefaultMethod { get; set; }
        IEnumerable<SelectListItem> Options { get; set; }

        String Operator { get; set; }
        IGridFilter First { get; set; }
        IGridFilter Second { get; set; }
    }
    public interface IGridColumnFilter<T, TValue> : IGridColumnFilter
    {
        IGridColumn<T, TValue> Column { get; set; }

        IQueryable<T> Apply(IQueryable<T> items);
    }
}
