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
        private HtmlGrid<GridModel> htmlGrid;

        public HtmlGridTests()
        {
            IGrid<GridModel> grid = new Grid<GridModel>(new GridModel[8]);
            IHtmlHelper html = Substitute.For<IHtmlHelper>();

            html.ViewContext.Returns(new ViewContext());
            html.ViewContext.HttpContext = new DefaultHttpContext();

            htmlGrid = new HtmlGrid<GridModel>(html, grid);

            grid.Columns.Add(model => model.Name);
            grid.Columns.Add(model => model.Sum);
        }

        #region HtmlGrid(HtmlHelper html, IGrid<T> grid)

        [Fact]
        public void HtmlGrid_DoesNotChangeQuery()
        {
            IQueryCollection query = htmlGrid.Grid.Query = new QueryCollection();

            Object actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.Query;
            Object expected = query;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGridQuery()
        {
            htmlGrid.Grid.Query = null;

            IQueryCollection expected = htmlGrid.Html.ViewContext.HttpContext.Request.Query;
            IQueryCollection actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.Query;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_DoesNotChangeViewContext()
        {
            ViewContext viewContext = htmlGrid.Grid.ViewContext = new ViewContext();

            Object actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.ViewContext;
            Object expected = viewContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsViewContext()
        {
            htmlGrid.Grid.ViewContext = null;

            ViewContext actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.ViewContext;
            ViewContext expected = htmlGrid.Html.ViewContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsPartialViewName()
        {
            String actual = new HtmlGrid<GridModel>(null, htmlGrid.Grid).PartialViewName;
            String expected = "MvcGrid/_Grid";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsHtml()
        {
            IHtmlHelper actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Html;
            IHtmlHelper expected = htmlGrid.Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGrid()
        {
            IGrid<GridModel> actual = new HtmlGrid<GridModel>(null, htmlGrid.Grid).Grid;
            IGrid<GridModel> expected = htmlGrid.Grid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region WriteTo(TextWriter writer, IHtmlEncoder encoder)

        [Fact]
        public void WriteTo_WritesPartialView()
        {
            Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
            htmlGrid.Html.PartialAsync(htmlGrid.PartialViewName, htmlGrid.Grid, null).Returns(view);
            StringWriter writer = new StringWriter();

            htmlGrid.WriteTo(writer, HtmlEncoder.Default);

            String actual = writer.GetStringBuilder().ToString();
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
