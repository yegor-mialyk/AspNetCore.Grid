using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public static class GridColumnExtensions
    {
        public static IGridColumn<T, TValue> RenderedAs<T, TValue>(this IGridColumn<T, TValue> column, Func<T, Object> value)
        {
            column.RenderValue = value;

            return column;
        }

        public static IGridColumn<T, TValue> WithFilterOptions<T, TValue>(this IGridColumn<T, TValue> column, IEnumerable<SelectListItem> options)
        {
            column.Filter.Options = options;

            return column;
        }
        public static IGridColumn<T, TValue> MultiFilterable<T, TValue>(this IGridColumn<T, TValue> column, Boolean isMultiple)
        {
            if (isMultiple && column.Filter.IsEnabled == null)
                column.Filter.IsEnabled = true;

            column.Filter.IsMulti = isMultiple;

            return column;
        }
        public static IGridColumn<T, TValue> Filterable<T, TValue>(this IGridColumn<T, TValue> column, Boolean isFilterable)
        {
            column.Filter.IsEnabled = isFilterable;

            return column;
        }
        public static IGridColumn<T, TValue> FilteredAs<T, TValue>(this IGridColumn<T, TValue> column, String filterName)
        {
            column.Filter.Name = filterName;

            return column;
        }

        public static IGridColumn<T, TValue> InitialSort<T, TValue>(this IGridColumn<T, TValue> column, GridSortOrder order)
        {
            column.Sort.InitialOrder = order;

            return column;
        }
        public static IGridColumn<T, TValue> FirstSort<T, TValue>(this IGridColumn<T, TValue> column, GridSortOrder order)
        {
            column.Sort.FirstOrder = order;

            return column;
        }
        public static IGridColumn<T, TValue> Sortable<T, TValue>(this IGridColumn<T, TValue> column, Boolean isSortable)
        {
            column.Sort.IsEnabled = isSortable;

            return column;
        }

        public static IGridColumn<T, TValue> AppendCss<T, TValue>(this IGridColumn<T, TValue> column, String cssClasses)
        {
            column.CssClasses = (column.CssClasses + " " + cssClasses?.Trim()).Trim();

            return column;
        }
        public static IGridColumn<T, TValue> Encoded<T, TValue>(this IGridColumn<T, TValue> column, Boolean isEncoded)
        {
            column.IsEncoded = isEncoded;

            return column;
        }
        public static IGridColumn<T, TValue> Formatted<T, TValue>(this IGridColumn<T, TValue> column, String format)
        {
            column.Format = format;

            return column;
        }
        public static IGridColumn<T, TValue> Css<T, TValue>(this IGridColumn<T, TValue> column, String cssClasses)
        {
            column.CssClasses = cssClasses?.Trim();

            return column;
        }
        public static IGridColumn<T, TValue> Titled<T, TValue>(this IGridColumn<T, TValue> column, Object value)
        {
            column.Title = value as IHtmlContent ?? new HtmlString(value?.ToString());

            return column;
        }
        public static IGridColumn<T, TValue> Named<T, TValue>(this IGridColumn<T, TValue> column, String name)
        {
            column.Name = name;

            return column;
        }
    }
}
