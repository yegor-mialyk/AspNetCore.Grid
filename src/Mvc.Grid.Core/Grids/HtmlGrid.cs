using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid
{
    public class HtmlGrid<T> : IHtmlGrid<T>
    {
        public IGrid<T> Grid { get; set; }
        public IHtmlHelper Html { get; set; }
        public String PartialViewName { get; set; }

        public HtmlGrid(IHtmlHelper html, IGrid<T> grid)
        {
            grid.Query = grid.Query ?? html.ViewContext.HttpContext.Request.Query;
            grid.ViewContext = grid.ViewContext ?? html.ViewContext;
            PartialViewName = "MvcGrid/_Grid";
            Html = html;
            Grid = grid;
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            Html.Partial(PartialViewName, Grid).WriteTo(writer, encoder);
        }
    }
}
