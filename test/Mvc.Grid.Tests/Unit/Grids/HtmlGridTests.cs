using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute.ReturnsExtensions;
using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid;

public class HtmlGridTests
{
    private HtmlGrid<GridModel> htmlGrid;

    public HtmlGridTests()
    {
        Grid<GridModel> grid = new(new GridModel[8]);
        IHtmlHelper html = Substitute.For<IHtmlHelper>();
        html.ViewContext.Returns(new ViewContext { HttpContext = new DefaultHttpContext() });

        htmlGrid = new HtmlGrid<GridModel>(html, grid);

        grid.Columns.Add(model => model.Name);
        grid.Columns.Add(model => model.Sum);
    }

    [Fact]
    public void HtmlGrid_DoesNotChangeQuery()
    {
        Object expected = htmlGrid.Grid.Query = new QueryCollection();
        Object? actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.Query;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void HtmlGrid_SetsRequestGridQuery()
    {
        htmlGrid.Grid.Query = null;
        htmlGrid.Html.ViewContext.Returns(new ViewContext());
        htmlGrid.Html.ViewContext.HttpContext = new DefaultHttpContext();

        Object expected = htmlGrid.Html.ViewContext.HttpContext.Request.Query;
        Object? actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.Query;

        Assert.Same(expected, actual);
        Assert.NotNull(actual);
    }

    [Fact]
    public void HtmlGrid_SetsEmptyGridQuery()
    {
        htmlGrid.Grid.Query = null;
        htmlGrid.Html.ViewContext.ReturnsNull();

        Assert.Empty(new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.Query!);
    }

    [Fact]
    public void HtmlGrid_DoesNotChangeHttpContext()
    {
        htmlGrid.Grid.HttpContext = new DefaultHttpContext();
        htmlGrid.Html.ViewContext.Returns(new ViewContext { HttpContext = new DefaultHttpContext() });

        Object? expected = htmlGrid.Grid.HttpContext;
        Object? actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.HttpContext;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void HtmlGrid_SetsHttpContext()
    {
        htmlGrid.Grid.HttpContext = null;
        htmlGrid.Html.ViewContext.Returns(new ViewContext());
        htmlGrid.Html.ViewContext.HttpContext = new DefaultHttpContext();

        Object? actual = new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).Grid.HttpContext;
        Object? expected = htmlGrid.Html.ViewContext.HttpContext;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void HtmlGrid_SetsPartialViewName()
    {
        Assert.Equal("MvcGrid/_Grid", new HtmlGrid<GridModel>(htmlGrid.Html, htmlGrid.Grid).PartialViewName);
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
        StringWriter writer = new();
        Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
        htmlGrid.Html.PartialAsync(htmlGrid.PartialViewName, htmlGrid.Grid, null).Returns(view);

        htmlGrid.WriteTo(writer, HtmlEncoder.Default);

        Assert.Equal("Test", writer.ToString());
    }
}
