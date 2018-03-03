using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class HtmlGridTests
    {
        private IHtmlHelper html;
        private IGrid<GridModel> grid;
        private HtmlGrid<GridModel> htmlGrid;

        public HtmlGridTests()
        {
            html = Substitute.For<IHtmlHelper>();
            html.ViewContext.Returns(new ViewContext());
            html.ViewContext.HttpContext = new DefaultHttpContext();
            grid = new Grid<GridModel>(new GridModel[8]);

            htmlGrid = new HtmlGrid<GridModel>(html, grid);
            grid.Columns.Add(model => model.Name);
            grid.Columns.Add(model => model.Sum);
        }

        #region HtmlGrid(HtmlHelper html, IGrid<T> grid)

        [Fact]
        public void HtmlGrid_DoesNotChangeQuery()
        {
            IQueryCollection query = grid.Query = new QueryCollection();

            Object actual = new HtmlGrid<GridModel>(html, grid).Grid.Query;
            Object expected = query;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGridQuery()
        {
            grid.Query = null;

            IQueryCollection expected = html.ViewContext.HttpContext.Request.Query;
            IQueryCollection actual = new HtmlGrid<GridModel>(html, grid).Grid.Query;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_DoesNotChangeViewContext()
        {
            ViewContext viewContext = grid.ViewContext = new ViewContext();

            Object actual = new HtmlGrid<GridModel>(html, grid).Grid.ViewContext;
            Object expected = viewContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsViewContext()
        {
            grid.ViewContext = null;

            ViewContext actual = new HtmlGrid<GridModel>(html, grid).Grid.ViewContext;
            ViewContext expected = html.ViewContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsPartialViewName()
        {
            String actual = new HtmlGrid<GridModel>(null, grid).PartialViewName;
            String expected = "MvcGrid/_Grid";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsHtml()
        {
            IHtmlHelper actual = new HtmlGrid<GridModel>(html, grid).Html;
            IHtmlHelper expected = html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGrid()
        {
            IGrid<GridModel> actual = new HtmlGrid<GridModel>(null, grid).Grid;
            IGrid<GridModel> expected = grid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region WriteTo(TextWriter writer, IHtmlEncoder encoder)

        [Fact]
        public void WriteTo_WritesPartialView()
        {
            Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
            html.PartialAsync(htmlGrid.PartialViewName, htmlGrid.Grid, null).Returns(view);
            StringWriter writer = new StringWriter();

            htmlGrid.WriteTo(writer, HtmlEncoder.Default);

            String actual = writer.GetStringBuilder().ToString();
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
