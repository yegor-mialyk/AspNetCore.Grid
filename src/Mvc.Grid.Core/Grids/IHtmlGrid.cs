using Microsoft.AspNetCore.Html;
using System;

namespace NonFactors.Mvc.Grid
{
    public interface IHtmlGrid<T> : IHtmlContent
    {
        IGrid<T> Grid { get; }
        String PartialViewName { get; set; }

        IHtmlGrid<T> Build(Action<IGridColumnsOf<T>> builder);
        IHtmlGrid<T> ProcessWith(IGridProcessor<T> processor);
        IHtmlGrid<T> WithSourceUrl(String url);
        
        IHtmlGrid<T> MultiFilterable();
        IHtmlGrid<T> Filterable();
        IHtmlGrid<T> Sortable();

        IHtmlGrid<T> RowAttributed(Func<T, Object> htmlAttributes);
        IHtmlGrid<T> RowCss(Func<T, String> cssClasses);
        IHtmlGrid<T> Attributed(Object htmlAttributes);
        IHtmlGrid<T> Css(String cssClasses);
        IHtmlGrid<T> Empty(String text);
        IHtmlGrid<T> Named(String name);

        IHtmlGrid<T> WithFooter(String partialViewName);

        IHtmlGrid<T> Pageable(Action<IGridPager<T>> builder);
        IHtmlGrid<T> Pageable();
    }
}
