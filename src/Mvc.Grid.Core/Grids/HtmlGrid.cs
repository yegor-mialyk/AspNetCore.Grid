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

        public virtual IHtmlGrid<T> Build(Action<IGridColumnsOf<T>> builder)
        {
            builder(Grid.Columns);

            return this;
        }
        public virtual IHtmlGrid<T> ProcessWith(IGridProcessor<T> processor)
        {
            Grid.Processors.Add(processor);

            return this;
        }
        public virtual IHtmlGrid<T> WithSourceUrl(String url)
        {
            Grid.SourceUrl = url;

            return this;
        }

        public virtual IHtmlGrid<T> MultiFilterable()
        {
            foreach (IGridColumn<T> column in Grid.Columns)
                if (column.IsMultiFilterable == null)
                    column.IsMultiFilterable = true;

            return this;
        }
        public virtual IHtmlGrid<T> Filterable( )
        {
            foreach (IGridColumn<T> column in Grid.Columns)
                if (column.IsFilterable == null)
                    column.IsFilterable = true;

            return this;
        }
        public virtual IHtmlGrid<T> Sortable()
        {
            foreach (IGridColumn<T> column in Grid.Columns)
                if (column.IsSortable == null)
                    column.IsSortable = true;

            return this;
        }

        public virtual IHtmlGrid<T> RowAttributed(Func<T, Object> htmlAttributes)
        {
            Grid.Rows.Attributes = htmlAttributes;

            return this;
        }
        public virtual IHtmlGrid<T> RowCss(Func<T, String> cssClasses)
        {
            Grid.Rows.CssClasses = cssClasses;

            return this;
        }
        public virtual IHtmlGrid<T> Attributed(Object htmlAttributes)
        {
            Grid.Attributes = new GridHtmlAttributes(htmlAttributes);

            return this;
        }
        public virtual IHtmlGrid<T> Css(String cssClasses)
        {
            Grid.CssClasses = cssClasses;

            return this;
        }
        public virtual IHtmlGrid<T> Empty(String text)
        {
            Grid.EmptyText = text;

            return this;
        }
        public virtual IHtmlGrid<T> Named(String name)
        {
            Grid.Name = name;

            return this;
        }

        public virtual IHtmlGrid<T> WithFooter(String partialViewName)
        {
            Grid.FooterPartialViewName = partialViewName;

            return this;
        }

        public virtual IHtmlGrid<T> Pageable(Action<IGridPager<T>> builder)
        {
            Grid.Pager = Grid.Pager ?? new GridPager<T>(Grid);
            builder(Grid.Pager);

            if (!Grid.Processors.Contains(Grid.Pager))
                Grid.Processors.Add(Grid.Pager);

            return this;
        }
        public virtual IHtmlGrid<T> Pageable()
        {
            return Pageable(builder => { });
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            Html.Partial(PartialViewName, Grid).WriteTo(writer, encoder);
        }
    }
}
