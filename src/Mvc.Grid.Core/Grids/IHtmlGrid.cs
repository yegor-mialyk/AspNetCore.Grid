using Microsoft.AspNetCore.Html;

namespace NonFactors.Mvc.Grid;

public interface IHtmlGrid<T> : IHtmlContent
{
    IGrid<T> Grid { get; }

    String PartialViewName { get; set; }
}
