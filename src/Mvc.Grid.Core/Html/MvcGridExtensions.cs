using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public static class MvcGridExtensions
    {
        public static HtmlGrid<T> Grid<T>(this IHtmlHelper html, IEnumerable<T> source) where T : class
        {
            return new HtmlGrid<T>(html, new Grid<T>(source));
        }
        public static HtmlGrid<T> Grid<T>(this IHtmlHelper html, String partialViewName, IEnumerable<T> source) where T : class
        {
            HtmlGrid<T> grid = new HtmlGrid<T>(html, new Grid<T>(source));
            grid.PartialViewName = partialViewName;

            return grid;
        }

        public static IHtmlContent AjaxGrid(this IHtmlHelper html, String dataSource)
        {
            return html.Partial("MvcGrid/_AjaxGrid", dataSource);
        }

        public static IServiceCollection AddMvcGrid(this IServiceCollection services)
        {
            return services.AddMvcGrid(filters => { });
        }
        public static IServiceCollection AddMvcGrid(this IServiceCollection services, Action<IGridFilters> configure)
        {
            IGridFilters filters = new GridFilters();
            configure(filters);

            return services.AddInstance<IGridFilters>(filters);
        }
    }
}
