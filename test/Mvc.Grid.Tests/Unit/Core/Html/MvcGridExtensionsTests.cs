using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class MvcGridExtensionsTests
    {
        private static IHtmlHelper html;

        static MvcGridExtensionsTests()
        {
            html = Substitute.For<IHtmlHelper>();
        }

        #region Grid<T>(this HtmlHelper html, IEnumerable<T> source)

        [Fact]
        public void Grid_CreatesHtmlGridWithHtml()
        {
            Object expected = html;
            Object actual = html.Grid(new GridModel[0]).Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_CreatesGridWithSource()
        {
            IEnumerable<GridModel> expected = new GridModel[0].AsQueryable();
            IEnumerable<GridModel> actual = html.Grid(expected).Grid.Source;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Grid<T>(this HtmlHelper html, String partialViewName, IEnumerable<T> source)

        [Fact]
        public void Grid_PartialViewName_CreatesHtmlGridWithHtml()
        {
            Object expected = html;
            Object actual = html.Grid("_Partial", new GridModel[0]).Html;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_PartialViewName_CreatesGridWithSource()
        {
            IEnumerable<GridModel> expected = new GridModel[0].AsQueryable();
            IEnumerable<GridModel> actual = html.Grid("_Partial", expected).Grid.Source;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Grid_PartialViewName_CreatesGridWithPartialViewName()
        {
            String actual = html.Grid("_Partial", new GridModel[0]).PartialViewName;
            String expected = "_Partial";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AjaxGrid(this HtmlHelper, String dataSource, Object htmlAttributes = null)

        [Fact]
        public void AjaxGrid_Div()
        {
            StringWriter writer = new StringWriter();

            html.AjaxGrid("DataSource").WriteTo(writer, HtmlEncoder.Default);

            String expected = "<div class=\"mvc-grid\" data-source-url=\"DataSource\"></div>";
            String actual = writer.GetStringBuilder().ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AjaxGrid_AttributedDiv()
        {
            StringWriter writer = new StringWriter();

            html.AjaxGrid("DataSource", new { @class = "classy", data_source_url = "Test", data_id = 1 }).WriteTo(writer, HtmlEncoder.Default);

            String expected = "<div class=\"mvc-grid classy\" data-id=\"1\" data-source-url=\"DataSource\"></div>";
            String actual = writer.GetStringBuilder().ToString();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AddMvcGrid(this IServiceCollection services)

        [Fact]
        public void AddMvcGrid_FiltersInstance()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddMvcGrid();

            ServiceDescriptor actual = services.Single();

            Assert.Equal(typeof(IGridFilters), actual.ServiceType);
            Assert.IsType<GridFilters>(actual.ImplementationInstance);
        }

        #endregion

        #region AddMvcGrid(this IServiceCollection services, Action<GridFilters> configure)

        [Fact]
        public void AddMvcGrid_ConfiguredFiltersInstance()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddMvcGrid(filters => { });

            ServiceDescriptor actual = services.Single();

            Assert.Equal(typeof(IGridFilters), actual.ServiceType);
            Assert.IsType<GridFilters>(actual.ImplementationInstance);
        }

        [Fact]
        public void AddMvcGrid_ConfiguresFiltersInstance()
        {
            Action<GridFilters> configure = Substitute.For<Action<GridFilters>>();
            IServiceCollection services = new ServiceCollection();

            services.AddMvcGrid(configure);

            ServiceDescriptor actual = services.Single();

            configure.Received()(actual.ImplementationInstance as GridFilters);
        }

        #endregion
    }
}
