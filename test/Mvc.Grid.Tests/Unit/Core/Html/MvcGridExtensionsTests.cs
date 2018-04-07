using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public void AjaxGrid_RendersPartial()
        {
            Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
            html.PartialAsync("MvcGrid/_AjaxGrid", Arg.Is<GridHtmlAttributes>(attributes =>
                attributes.Count == 2 && attributes["data-source-url"].ToString() == "DataSource" && attributes["class"].ToString() == "mvc-grid"), null).Returns(view);

            IHtmlContent actual = html.AjaxGrid("DataSource");
            IHtmlContent expected = view.Result;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AjaxGrid_RendersPartialWithHtmlAttributes()
        {
            Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
            html.PartialAsync("MvcGrid/_AjaxGrid", Arg.Is<GridHtmlAttributes>(attributes =>
                attributes.Count == 3 &&
                (Int32)attributes["data-id"] == 1 &&
                (String)attributes["data-source-url"] == "DataSource" &&
                (String)attributes["class"] == "classy mvc-grid"), null).Returns(view);

            Object actual = html.AjaxGrid("DataSource", new { @class = "classy", data_source_url = "Test", data_id = 1 });
            Object expected = view.Result;

            Assert.Same(expected, actual);
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

        #region AddMvcGrid(this IServiceCollection services, Action<IGridFilters> configure)

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
            Action<IGridFilters> configure = Substitute.For<Action<IGridFilters>>();
            IServiceCollection services = new ServiceCollection();

            services.AddMvcGrid(configure);

            ServiceDescriptor actual = services.Single();

            configure.Received()(actual.ImplementationInstance as IGridFilters);
        }

        #endregion
    }
}
