using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
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
            html = HtmlHelperFactory.CreateHtmlHelper();
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
        public void AjaxGrid_RendersAjaxGridPartial()
        {
            Task<IHtmlContent> view = Task.FromResult<IHtmlContent>(new HtmlString("Test"));
            html.PartialAsync("MvcGrid/_AjaxGrid", "DataSource", null).Returns(view);

            String actual = html.AjaxGrid("DataSource").ToString();
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
