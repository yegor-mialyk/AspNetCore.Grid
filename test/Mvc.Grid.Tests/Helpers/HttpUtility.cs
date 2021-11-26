using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace NonFactors.Mvc.Grid;

public static class HttpUtility
{
    public static IQueryCollection ParseQueryString(String query)
    {
        return new QueryCollection(QueryHelpers.ParseQuery(query));
    }
}
