using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace NonFactors.Mvc.Grid.Tests
{
    public class TestHelper
    {
        public static IQueryCollection ParseQuery(String queryString)
        {
            return new QueryCollection(QueryHelpers.ParseQuery(queryString));
        }
    }
}
