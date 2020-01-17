using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public interface IGrid
    {
        String Url { get; set; }
        String? Id { get; set; }
        String Name { get; set; }
        String? EmptyText { get; set; }

        IQueryCollection? Query { get; set; }
        GridProcessingMode Mode { get; set; }
        ViewContext? ViewContext { get; set; }
        GridFilterMode FilterMode { get; set; }
        String FooterPartialViewName { get; set; }
        GridHtmlAttributes Attributes { get; set; }

        IGridColumns<IGridColumn> Columns { get; }
        IGridRows<Object> Rows { get; }
        IGridPager? Pager { get; }
    }

    public interface IGrid<T> : IGrid
    {
        IGridSort<T> Sort { get; set; }
        IQueryable<T> Source { get; set; }
        HashSet<IGridProcessor<T>> Processors { get; set; }

        new IGridColumnsOf<T> Columns { get; }
        new IGridRowsOf<T> Rows { get; }
        new IGridPager<T>? Pager { get; set; }
    }
}
