using Microsoft.AspNetCore.Html;
using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public interface IGridColumn
    {
        String Name { get; set; }
        Object Title { get; set; }
        String? Format { get; set; }
        Boolean IsHidden { get; set; }
        String CssClasses { get; set; }
        Boolean IsEncoded { get; set; }

        IGridColumnSort Sort { get; }
        IGridColumnFilter Filter { get; }

        IHtmlContent ValueFor(IGridRow<Object> row);
    }
    public interface IGridColumn<T> : IGridColumn
    {
        IGrid<T> Grid { get; }

        new IGridColumnSort<T> Sort { get; }
    }
    public interface IGridColumn<T, TValue> : IGridProcessor<T>, IGridColumn<T>
    {
        Func<T, Int32, Object?>? RenderValue { get; set; }
        Expression<Func<T, TValue>> Expression { get; set; }

        new IGridColumnSort<T, TValue> Sort { get; set; }
        new IGridColumnFilter<T, TValue> Filter { get; set; }
    }
}
