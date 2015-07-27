using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using NSubstitute;

namespace NonFactors.Mvc.Grid.Tests
{
    public class HtmlHelperFactory
    {
        public static IHtmlHelper CreateHtmlHelper()
        {
            IHtmlHelper html = Substitute.For<IHtmlHelper>();

            html.ViewContext.Returns(new ViewContext());
            html.ViewContext.HttpContext = new DefaultHttpContext();

            return html;
        }
    }
}