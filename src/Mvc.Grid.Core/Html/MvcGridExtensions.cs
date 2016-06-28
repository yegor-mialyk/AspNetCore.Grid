using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
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
            return new HtmlGrid<T>(html, new Grid<T>(source)) { PartialViewName = partialViewName };
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

            return services.AddSingleton(filters);
        }
    }
}
