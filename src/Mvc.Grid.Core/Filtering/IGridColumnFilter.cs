using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumnFilter
    {
        String Name { get; set; }
        Boolean? IsEnabled { get; set; }
        String DefaultMethod { get; set; }
        GridFilterCase? Case { get; set; }
        GridFilterType? Type { get; set; }
        IEnumerable<SelectListItem> Options { get; set; }

        String? Operator { get; set; }
        IGridFilter? First { get; set; }
        IGridFilter? Second { get; set; }
    }
    public interface IGridColumnFilter<T> : IGridColumnFilter
    {
        IQueryable<T> Apply(IQueryable<T> items);
    }
    public interface IGridColumnFilter<T, TValue> : IGridColumnFilter<T>
    {
        IGridColumn<T, TValue> Column { get; set; }
    }
}
