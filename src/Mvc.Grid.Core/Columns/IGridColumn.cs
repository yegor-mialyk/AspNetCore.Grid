using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumn : IFilterableColumn, ISortableColumn
    {
        String Name { get; set; }
        String Title { get; set; }
        String Format { get; set; }
        String CssClasses { get; set; }
        Boolean IsEncoded { get; set; }

        HtmlString ValueFor(IGridRow<Object> row);
    }

    public interface IGridColumn<T> : IFilterableColumn<T>, ISortableColumn<T>, IGridColumn
    {
        IGrid<T> Grid { get; }
        LambdaExpression Expression { get; }

        IGridColumn<T> RenderedAs(Func<T, Object> value);

        IGridColumn<T> MultiFilterable(Boolean isMultiple);
        IGridColumn<T> Filterable(Boolean isFilterable);
        IGridColumn<T> FilteredAs(String filterName);

        IGridColumn<T> InitialSort(GridSortOrder order);
        IGridColumn<T> FirstSort(GridSortOrder order);
        IGridColumn<T> Sortable(Boolean isSortable);

        IGridColumn<T> Encoded(Boolean isEncoded);
        IGridColumn<T> Formatted(String format);
        IGridColumn<T> Css(String cssClasses);
        IGridColumn<T> Titled(String title);
        IGridColumn<T> Named(String name);
    }
}
