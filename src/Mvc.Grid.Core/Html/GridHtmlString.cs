using Microsoft.AspNetCore.Html;
using System;
using System.IO;
using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid
{
    public class GridHtmlString : IHtmlContent
    {
        private String Value { get; }

        public GridHtmlString(String value)
        {
            Value = value;
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            if (encoder == null)
                writer.Write(Value);
            else
                writer.Write(encoder.Encode(Value));
        }

        public override String ToString()
        {
            return Value ?? String.Empty;
        }
    }
}
