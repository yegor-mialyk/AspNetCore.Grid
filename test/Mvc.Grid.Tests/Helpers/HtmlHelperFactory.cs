using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using NSubstitute;
using System;

namespace NonFactors.Mvc.Grid.Tests
{
    public class HtmlHelperFactory
    {
        public static IHtmlHelper CreateHtmlHelper(String queryString = null)
        {
            IHtmlHelper html = Substitute.For<IHtmlHelper>();

            html.ViewContext.Returns(new ViewContext());
            html.ViewContext.HttpContext = HttpContextFactory.CreateHttpContext();

            return html;
        }
    }
}