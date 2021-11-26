using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid;

public class GridHtmlString : IHtmlContent
{
    private String Value { get; }

    public GridHtmlString(String? value)
    {
        Value = value ?? "";
    }

    public void WriteTo(TextWriter writer, HtmlEncoder? encoder)
    {
        writer.Write(encoder?.Encode(Value) ?? Value);
    }

    public override String ToString()
    {
        return Value;
    }
}
