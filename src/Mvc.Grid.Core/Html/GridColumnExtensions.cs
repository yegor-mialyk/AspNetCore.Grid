using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public static class GridColumnExtensions
    {
        public static IGridColumn<T, TValue> RenderedAs<T, TValue>(this IGridColumn<T, TValue> column, Func<T, Int32, Object?> value)
        {
            column.RenderValue = value;

            return column;
        }
        public static IGridColumn<T, TValue> RenderedAs<T, TValue>(this IGridColumn<T, TValue> column, Func<T, Object?> value)
        {
            column.RenderValue = (t, i) => value(t);

            return column;
        }

        public static IGridColumn<T, TValue> UsingFilterOptions<T, TValue>(this IGridColumn<T, TValue> column, IEnumerable<SelectListItem>? options = null)
        {
            if (String.IsNullOrEmpty(column.Filter.DefaultMethod))
                column.Filter.DefaultMethod = "equals";
            column.Filter.IsEnabled = true;

            column.Filter.Options = options ?? new[] { new SelectListItem() }
                .Concat(column
                .Grid
                .Source
                .OrderBy(column.Expression)
                .Select(column.Expression)
                .Distinct()
                .ToArray()
                .Where(value => value != null)
                .Select(value => value!.ToString())
                .Select(value => new SelectListItem
                {
                    Value = value,
                    Text = value
                }).ToArray());

            return column;
        }

        public static IGridColumn<T, TValue> UsingDefaultFilterMethod<T, TValue>(this IGridColumn<T, TValue> column, String method)
        {
            column.Filter.DefaultMethod = method;

            return column;
        }
        public static IGridColumn<T, TValue> Filterable<T, TValue>(this IGridColumn<T, TValue> column, Boolean isFilterable)
        {
            column.Filter.IsEnabled = isFilterable;

            return column;
        }
        public static IGridColumn<T, TValue> Filterable<T, TValue>(this IGridColumn<T, TValue> column, GridFilterType type)
        {
            column.Filter.IsEnabled ??= true;
            column.Filter.Type = type;

            return column;
        }
        public static IGridColumn<T, TValue> FilteredAs<T, TValue>(this IGridColumn<T, TValue> column, String filterName)
        {
            column.Filter.Name = filterName;

            return column;
        }

        public static IGridColumn<T, TValue> Sortable<T, TValue>(this IGridColumn<T, TValue> column, GridSortOrder firstOrder)
        {
            column.Sort.FirstOrder = firstOrder;
            column.Sort.IsEnabled ??= true;

            return column;
        }
        public static IGridColumn<T, TValue> Sortable<T, TValue>(this IGridColumn<T, TValue> column, Boolean isSortable)
        {
            column.Sort.IsEnabled = isSortable;

            return column;
        }

        public static IGridColumn<T, TValue> Encoded<T, TValue>(this IGridColumn<T, TValue> column, Boolean isEncoded)
        {
            column.IsEncoded = isEncoded;

            return column;
        }
        public static IGridColumn<T, TValue> Formatted<T, TValue>(this IGridColumn<T, TValue> column, String? format)
        {
            column.Format = format;

            return column;
        }
        public static IGridColumn<T, TValue> AppendCss<T, TValue>(this IGridColumn<T, TValue> column, String css)
        {
            column.CssClasses = (column.CssClasses + " " + css?.Trim()).Trim();

            return column;
        }
        public static IGridColumn<T, TValue> Titled<T, TValue>(this IGridColumn<T, TValue> column, Object title)
        {
            column.Title = title ?? "";

            return column;
        }
        public static IGridColumn<T, TValue> Named<T, TValue>(this IGridColumn<T, TValue> column, String name)
        {
            column.Name = name;

            return column;
        }
        public static IGridColumn<T, TValue> Css<T, TValue>(this IGridColumn<T, TValue> column, String css)
        {
            column.CssClasses = css?.Trim() ?? "";

            return column;
        }

        public static IGridColumn<T, TValue> Hidden<T, TValue>(this IGridColumn<T, TValue> column)
        {
            column.IsHidden = true;

            return column;
        }
    }
}
