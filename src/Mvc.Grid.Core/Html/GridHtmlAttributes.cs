using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid;

public class GridHtmlAttributes : Dictionary<String, Object?>, IHtmlContent
{
    public GridHtmlAttributes()
    {
    }
    public GridHtmlAttributes(Object? attributes)
        : base(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes))
    {
    }

    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        foreach ((String attribute, Object? value) in this)
        {
            if (value == null)
                continue;

            writer.Write(" ");
            writer.Write(attribute);
            writer.Write("=\"");

            writer.Write(encoder.Encode(value.ToString() ?? ""));

            writer.Write("\"");
        }
    }
}
