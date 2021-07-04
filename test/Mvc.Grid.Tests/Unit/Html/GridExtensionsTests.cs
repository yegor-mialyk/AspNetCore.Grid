using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests
{
    public class MvcGridExtensionsTests
    {
        private static IHtmlHelper html;

        static MvcGridExtensionsTests()
        {
            html = Substitute.For<IHtmlHelper>();
            html.ViewContext.Returns(new ViewContext { HttpContext = new DefaultHttpContext() });
        }

        [Fact]
        public void Grid_CreatesHtmlGridWithHtml()
        {
            Object expected = html;
            Object actual = html.Grid(Array.Empty<GridModel>()).Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_CreatesGridWithSource()
        {
            IEnumerable<GridModel> expected = Array.Empty<GridModel>().AsQueryable();
            IEnumerable<GridModel> actual = html.Grid(expected).Grid.Source;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_PartialViewName_CreatesHtmlGridWithHtml()
        {
            Object expected = html;
            Object actual = html.Grid("_Partial", Array.Empty<GridModel>()).Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_PartialViewName_CreatesGridWithSource()
        {
            IEnumerable<GridModel> expected = Array.Empty<GridModel>().AsQueryable();
            IEnumerable<GridModel> actual = html.Grid("_Partial", expected).Grid.Source;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_PartialViewName_CreatesGridWithPartialViewName()
        {
            Assert.Equal("_Partial", html.Grid("_Partial", Array.Empty<GridModel>()).PartialViewName);
        }

        [Fact]
        public void AjaxGrid_Div()
        {
            StringWriter writer = new();

            html.AjaxGrid("DataSource").WriteTo(writer, HtmlEncoder.Default);

            Assert.Equal("<div class=\"mvc-grid\" data-url=\"DataSource\"></div>", writer.ToString());
        }

        [Fact]
        public void AjaxGrid_AttributedDiv()
        {
            StringWriter writer = new();

            html.AjaxGrid("DataSource", new { @class = "classy", data_url = "Test", data_id = 1 }).WriteTo(writer, HtmlEncoder.Default);

            Assert.Equal("<div class=\"mvc-grid classy\" data-id=\"1\" data-url=\"DataSource\"></div>", writer.ToString());
        }

        [Fact]
        public void AddMvcGrid_FiltersInstance()
        {
            ServiceDescriptor actual = new ServiceCollection().AddMvcGrid().Single();

            Assert.Equal(typeof(IGridFilters), actual.ServiceType);
            Assert.IsType<GridFilters>(actual.ImplementationInstance);
        }

        [Fact]
        public void AddMvcGrid_ConfiguresFiltersInstance()
        {
            Action<GridFilters> configure = Substitute.For<Action<GridFilters>>();

            ServiceDescriptor actual = new ServiceCollection().AddMvcGrid(configure).Single();

            configure.Received()((GridFilters)actual.ImplementationInstance!);
        }
    }
}
