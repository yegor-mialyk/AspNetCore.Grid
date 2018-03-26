using Microsoft.AspNetCore.Html;
using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumn
    {
        String Name { get; set; }
        String Format { get; set; }
        String CssClasses { get; set; }
        Boolean IsEncoded { get; set; }
        IHtmlContent Title { get; set; }
        LambdaExpression Expression { get; }

        Boolean? IsSortable { get; set; }
        GridSortOrder? SortOrder { get; set; }
        GridSortOrder? FirstSortOrder { get; set; }
        GridSortOrder? InitialSortOrder { get; set; }

        IGridColumnFilter Filter { get; }

        IHtmlContent ValueFor(IGridRow<Object> row);
    }

    public interface IGridColumn<T> : IGridProcessor<T>, IGridColumn
    {
        IGrid<T> Grid { get; }

        Func<T, Object> RenderValue { get; set; }

        new IGridColumnFilter<T> Filter { get; set; }
    }
}
