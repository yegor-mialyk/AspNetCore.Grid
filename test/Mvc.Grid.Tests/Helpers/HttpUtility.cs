using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace NonFactors.Mvc.Grid.Tests
{
    public static class HttpUtility
    {
        public static IQueryCollection ParseQueryString(String query)
        {
            return new QueryCollection(QueryHelpers.ParseQuery(query));
        }
    }
}
