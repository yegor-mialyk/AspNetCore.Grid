using Microsoft.AspNetCore.Html;
using System;

namespace NonFactors.Mvc.Grid
{
    public static class GridColumnExtensions
    {
        public static IGridColumn<TModel> RenderedAs<TModel>(this IGridColumn<TModel> column, Func<TModel, Object> value)
        {
            column.RenderValue = value;

            return column;
        }

        public static T MultiFilterable<T>(this T column, Boolean isMultiple) where T : IGridColumn
        {
            column.IsMultiFilterable = isMultiple;

            return column;
        }
        public static T Filterable<T>(this T column, Boolean isFilterable) where T : IGridColumn
        {
            column.IsFilterable = isFilterable;

            return column;
        }
        public static T FilteredAs<T>(this T column, String filterName) where T : IGridColumn
        {
            column.FilterName = filterName;

            return column;
        }

        public static T InitialSort<T>(this T column, GridSortOrder order) where T : IGridColumn
        {
            column.InitialSortOrder = order;

            return column;
        }
        public static T FirstSort<T>(this T column, GridSortOrder order) where T : IGridColumn
        {
            column.FirstSortOrder = order;

            return column;
        }
        public static T Sortable<T>(this T column, Boolean isSortable) where T : IGridColumn
        {
            column.IsSortable = isSortable;

            return column;
        }

        public static T Encoded<T>(this T column, Boolean isEncoded) where T : IGridColumn
        {
            column.IsEncoded = isEncoded;

            return column;
        }
        public static T Formatted<T>(this T column, String format) where T : IGridColumn
        {
            column.Format = format;

            return column;
        }
        public static T Css<T>(this T column, String cssClasses) where T : IGridColumn
        {
            column.CssClasses = cssClasses;

            return column;
        }
        public static T Titled<T>(this T column, Object value) where T : IGridColumn
        {
            column.Title = value as IHtmlContent ?? new HtmlString(value?.ToString());

            return column;
        }
        public static T Named<T>(this T column, String name) where T : IGridColumn
        {
            column.Name = name;

            return column;
        }
    }
}
