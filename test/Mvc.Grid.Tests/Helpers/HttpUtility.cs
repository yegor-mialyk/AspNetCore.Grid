using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace NonFactors.Mvc.Grid.Tests
{
    public class HttpUtility
    {
        public static IQueryCollection ParseQueryString(String queryString)
        {
            return new QueryCollection(QueryHelpers.ParseQuery(queryString));
        }
    }
}
