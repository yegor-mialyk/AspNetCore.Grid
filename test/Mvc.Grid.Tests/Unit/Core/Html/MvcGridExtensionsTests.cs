using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class MvcGridExtensions
    {
        private static IHtmlHelper html;

        static MvcGridExtensions()
        {
            html = Substitute.For<IHtmlHelper>();
            html.ViewContext.Returns(new ViewContext());
            html.ViewContext.HttpContext = new DefaultHttpContext();
        }

        #region Extension: Grid<T>(this HtmlHelper html, IEnumerable<T> source)

        [Fact]
        public void Grid_CreatesHtmlGridWithHtml()
        {
            IHtmlHelper actual = html.Grid(new GridModel[0]).Html;
            IHtmlHelper expected = html;

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

        #region Extension: Grid<T>(this HtmlHelper html, String partialViewName, IEnumerable<T> source)

        [Fact]
        public void Grid_PartialViewName_CreatesHtmlGridWithHtml()
        {
            IHtmlHelper actual = html.Grid("_Partial", new GridModel[0]).Html;
            IHtmlHelper expected = html;

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

            Assert.Same(expected, actual);
        }

        #endregion

        #region Extension: AjaxGrid(this HtmlHelper, String dataSource)

        [Fact]
        public void AjaxGrid_RendersPartial()
        {
            Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
            html.PartialAsync("MvcGrid/_AjaxGrid", "DataSource", null).Returns(view);

            String actual = html.AjaxGrid("DataSource").ToString();
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension: AddMvcGrid(this IServiceCollection services)

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

        #region Extension: AddMvcGrid(this IServiceCollection services, Action<IGridFilters> configure)

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
            IServiceCollection services = new ServiceCollection();
            Action<IGridFilters> configure = Substitute.For<Action<IGridFilters>>();

            services.AddMvcGrid(configure);

            ServiceDescriptor actual = services.Single();

            configure.Received()(actual.ImplementationInstance as IGridFilters);
        }

        #endregion
    }
}
