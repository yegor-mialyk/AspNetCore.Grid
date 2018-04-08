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

        public static IHtmlContent AjaxGrid(this IHtmlHelper html, String dataSource, Object htmlAttributes = null)
        {
            GridHtmlAttributes attributes = new GridHtmlAttributes(htmlAttributes);
            attributes["data-source-url"] = dataSource;

            if (attributes.ContainsKey("class"))
                attributes["class"] += " mvc-grid";
            else
                attributes["class"] = "mvc-grid";

            return html.Partial("MvcGrid/_AjaxGrid", attributes);
        }

        public static IServiceCollection AddMvcGrid(this IServiceCollection services)
        {
            return services.AddMvcGrid(filters => { });
        }
        public static IServiceCollection AddMvcGrid(this IServiceCollection services, Action<GridFilters> configure)
        {
            GridFilters filters = new GridFilters();
            configure(filters);

            return services.AddSingleton<IGridFilters>(filters);
        }
    }
}
