﻿using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public static class GridHtmlExtensions
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

        public static HtmlString AjaxGrid(this IHtmlHelper html, String dataSource)
        {
            return html.Partial("MvcGrid/_AjaxGrid", dataSource);
        }
    }
}
