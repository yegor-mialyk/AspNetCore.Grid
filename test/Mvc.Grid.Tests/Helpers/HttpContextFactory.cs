using Microsoft.AspNet.Http;
using NSubstitute;
using System;

namespace NonFactors.Mvc.Grid.Tests
{
    public class HttpContextFactory
    {
        public static HttpContext CreateHttpContext(String query = null)
        {
            return Substitute.For<HttpContext>();
        }
    }
}
