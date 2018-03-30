using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid
{
    public class GridHtmlAttributes : Dictionary<String, Object>, IHtmlContent
    {
        public GridHtmlAttributes()
        {
        }
        public GridHtmlAttributes(Object attributes)
            : base(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes))
        {
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            foreach (KeyValuePair<String, Object> attribute in this)
            {
                writer.Write(" ");
                writer.Write(attribute.Key);
                writer.Write("=\"");

                if (attribute.Value != null)
                    writer.Write(encoder.Encode(attribute.Value.ToString()));

                writer.Write("\"");
            }
        }
    }
}
