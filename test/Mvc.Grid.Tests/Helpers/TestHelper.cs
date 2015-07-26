using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.WebUtilities;
using System;

namespace NonFactors.Mvc.Grid.Tests
{
    public class TestHelper
    {
        public static IReadableStringCollection ParseQuery(String queryString)
        {
            return new ReadableStringCollection(QueryHelpers.ParseQuery(queryString));
        }
    }
}
