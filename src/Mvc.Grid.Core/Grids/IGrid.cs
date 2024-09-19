using Microsoft.AspNetCore.Http;

namespace NonFactors.Mvc.Grid;

public interface IGrid
{
    String Url { get; set; }
    String? Id { get; set; }
    String Name { get; set; }
    String? EmptyText { get; set; }

    IQueryCollection? Query { get; set; }
    GridProcessingMode Mode { get; set; }
    HttpContext? HttpContext { get; set; }
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
