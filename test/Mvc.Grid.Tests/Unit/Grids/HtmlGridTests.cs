using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests
{
    public class HtmlGridTests
    {
        private HtmlGrid<GridModel> htmlGrid;

        public HtmlGridTests()
        {
            IHtmlHelper html = Substitute.For<IHtmlHelper>();
            IGrid<GridModel> grid = new Grid<GridModel>(new GridModel[8]);
            html.ViewContext.Returns(new ViewContext { HttpContext = new DefaultHttpContext() });

            htmlGrid = new HtmlGrid<GridModel>(html, grid);

            grid.Columns.Add(model => model.Name);
            grid.Columns.Add(model => model.Sum);
        }

        [Fact]
        public void HtmlGrid_DoesNotChangeQuery()
        {
            Object? expected = htmlGrid.Grid.Query = new QueryCollection();
            Object? actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.Query;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsRequestGridQuery()
        {
            htmlGrid.Grid.Query = null;
            htmlGrid.Html.ViewContext.Returns(new ViewContext());
            htmlGrid.Html.ViewContext.HttpContext = new DefaultHttpContext();

            Object? expected = htmlGrid.Html.ViewContext.HttpContext.Request.Query;
            Object? actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.Query;

            Assert.Same(expected, actual);
            Assert.NotNull(actual);
        }

        [Fact]
        public void HtmlGrid_SetsEmptyGridQuery()
        {
            htmlGrid.Grid.Query = null;
            htmlGrid.Html.ViewContext.ReturnsNull();

            Assert.Empty(new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.Query);
        }

        [Fact]
        public void HtmlGrid_DoesNotChangeViewContext()
        {
            htmlGrid.Grid.ViewContext = new ViewContext { HttpContext = new DefaultHttpContext() };
            htmlGrid.Html.ViewContext.Returns(new ViewContext { HttpContext = new DefaultHttpContext() });

            Object? expected = htmlGrid.Grid.ViewContext;
            Object? actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.ViewContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsViewContext()
        {
            htmlGrid.Grid.ViewContext = null;
            htmlGrid.Html.ViewContext.Returns(new ViewContext());
            htmlGrid.Html.ViewContext.HttpContext = new DefaultHttpContext();

            Object? actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.ViewContext;
            Object? expected = htmlGrid.Html.ViewContext;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsPartialViewName()
        {
            String actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).PartialViewName;
            String expected = "MvcGrid/_Grid";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsHtml()
        {
            Object actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Html;
            Object expected = htmlGrid.Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void HtmlGrid_SetsGrid()
        {
            Object actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid;
            Object expected = htmlGrid.Grid;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void WriteTo_WritesPartialView()
        {
            StringWriter writer = new StringWriter();
            Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
            htmlGrid.Html.PartialAsync(htmlGrid.PartialViewName, htmlGrid.Grid, null).Returns(view);

            htmlGrid.WriteTo(writer, HtmlEncoder.Default);

            String actual = writer.GetStringBuilder().ToString();
            String expected = "Test";

            Assert.Equal(expected, actual);
        }
    }
}
