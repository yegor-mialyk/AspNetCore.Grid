using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public static class HtmlGridExtensions
    {
        public static IHtmlGrid<T> Build<T>(this IHtmlGrid<T> html, Action<IGridColumnsOf<T>> builder)
        {
            builder(html.Grid.Columns);

            html.Grid.Processors.Add(html.Grid.Sort);

            return html;
        }
        public static IHtmlGrid<T> UsingUrl<T>(this IHtmlGrid<T> html, String url)
        {
            html.Grid.Url = url;

            return html;
        }

        public static IHtmlGrid<T> Filterable<T>(this IHtmlGrid<T> html, GridFilterType? type = null)
        {
            foreach (IGridColumn column in html.Grid.Columns)
            {
                if (column.Filter.IsEnabled == null)
                    column.Filter.IsEnabled = true;

                if (column.Filter.Type == null)
                    column.Filter.Type = type;
            }

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
        public static IHtmlGrid<T> Attributed<T>(this IHtmlGrid<T> html, Object htmlAttributes)
        {
            foreach (KeyValuePair<String, Object> attribute in HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
                html.Grid.Attributes[attribute.Key] = attribute.Value;

            return html;
        }
        public static IHtmlGrid<T> AppendCss<T>(this IHtmlGrid<T> html, String cssClasses)
        {
            if (html.Grid.Attributes.ContainsKey("class"))
                html.Grid.Attributes["class"] = (html.Grid.Attributes["class"] + " " + cssClasses?.TrimStart()).Trim();
            else
                html.Grid.Attributes["class"] = cssClasses?.Trim();

            return html;
        }
        public static IHtmlGrid<T> Css<T>(this IHtmlGrid<T> html, String cssClasses)
        {
            html.Grid.Attributes["class"] = cssClasses?.Trim();

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
        public static IHtmlGrid<T> Id<T>(this IHtmlGrid<T> html, String? id)
        {
            html.Grid.Id = id;

            return html;
        }

        public static IHtmlGrid<T> UsingProcessingMode<T>(this IHtmlGrid<T> html, GridProcessingMode mode)
        {
            html.Grid.Mode = mode;

            return html;
        }
        public static IHtmlGrid<T> UsingProcessor<T>(this IHtmlGrid<T> html, IGridProcessor<T> processor)
        {
            html.Grid.Processors.Add(processor);

            return html;
        }
        public static IHtmlGrid<T> UsingFilterMode<T>(this IHtmlGrid<T> html, GridFilterMode mode)
        {
            html.Grid.FilterMode = mode;

            return html;
        }

        public static IHtmlGrid<T> UsingFooter<T>(this IHtmlGrid<T> html, String partialViewName)
        {
            html.Grid.FooterPartialViewName = partialViewName;

            return html;
        }

        public static IHtmlGrid<T> Pageable<T>(this IHtmlGrid<T> html, Action<IGridPager<T>>? builder = null)
        {
            html.Grid.Pager ??= new GridPager<T>(html.Grid);
            html.Grid.Processors.Add(html.Grid.Pager);

            builder?.Invoke(html.Grid.Pager);

            return html;
        }
    }
}
