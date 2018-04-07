using System;

namespace NonFactors.Mvc.Grid
{
    public static class HtmlGridExtensions
    {
        public static IHtmlGrid<T> Build<T>(this IHtmlGrid<T> html, Action<IGridColumnsOf<T>> builder)
        {
            builder(html.Grid.Columns);

            return html;
        }
        public static IHtmlGrid<T> ProcessWith<T>(this IHtmlGrid<T> html, IGridProcessor<T> processor)
        {
            html.Grid.Processors.Add(processor);

            return html;
        }
        public static IHtmlGrid<T> WithSourceUrl<T>(this IHtmlGrid<T> html, String url)
        {
            html.Grid.SourceUrl = url;

            return html;
        }

        public static IHtmlGrid<T> MultiFilterable<T>(this IHtmlGrid<T> html)
        {
            foreach (IGridColumn column in html.Grid.Columns)
            {
                if (column.Filter.IsEnabled == null)
                    column.Filter.IsEnabled = true;

                if (column.Filter.IsMulti == null)
                    column.Filter.IsMulti = true;
            }

            return html;
        }
        public static IHtmlGrid<T> Filterable<T>(this IHtmlGrid<T> html)
        {
            foreach (IGridColumn column in html.Grid.Columns)
                if (column.Filter.IsEnabled == null)
                    column.Filter.IsEnabled = true;

            return html;
        }
        public static IHtmlGrid<T> Sortable<T>(this IHtmlGrid<T> html)
        {
            foreach (IGridColumn column in html.Grid.Columns)
                if (column.Sort.IsEnabled == null)
                    column.Sort.IsEnabled = true;

            return html;
        }

        public static IHtmlGrid<T> RowAttributed<T>(this IHtmlGrid<T> html, Func<T, Object> htmlAttributes)
        {
            html.Grid.Rows.Attributes = htmlAttributes;

            return html;
        }
        public static IHtmlGrid<T> RowCss<T>(this IHtmlGrid<T> html, Func<T, String> cssClasses)
        {
            html.Grid.Rows.CssClasses = cssClasses;

            return html;
        }
        public static IHtmlGrid<T> Attributed<T>(this IHtmlGrid<T> html, Object htmlAttributes)
        {
            html.Grid.Attributes = new GridHtmlAttributes(htmlAttributes);

            return html;
        }
        public static IHtmlGrid<T> Css<T>(this IHtmlGrid<T> html, String cssClasses)
        {
            html.Grid.CssClasses = cssClasses;

            return html;
        }
        public static IHtmlGrid<T> Empty<T>(this IHtmlGrid<T> html, String text)
        {
            html.Grid.EmptyText = text;

            return html;
        }
        public static IHtmlGrid<T> Named<T>(this IHtmlGrid<T> html, String name)
        {
            html.Grid.Name = name;

            return html;
        }

        public static IHtmlGrid<T> WithFilterMode<T>(this IHtmlGrid<T> html, GridFilterMode mode)
        {
            html.Grid.FilterMode = mode;

            return html;
        }

        public static IHtmlGrid<T> WithFooter<T>(this IHtmlGrid<T> html, String partialViewName)
        {
            html.Grid.FooterPartialViewName = partialViewName;

            return html;
        }

        public static IHtmlGrid<T> Pageable<T>(this IHtmlGrid<T> html, Action<IGridPager<T>> builder)
        {
            builder(html.Grid.Pager = html.Grid.Pager ?? new GridPager<T>(html.Grid));

            html.Grid.Processors.Add(html.Grid.Pager);

            return html;
        }
        public static IHtmlGrid<T> Pageable<T>(this IHtmlGrid<T> html)
        {
            return html.Pageable(builder => { });
        }
    }
}
