using System;
using System.IO;
using System.Text.Encodings.Web;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests
{
    public class GridHtmlAttributesTests
    {
        [Fact]
        public void GridHtmlAttributes_Empty()
        {
            Assert.Empty(new GridHtmlAttributes());
        }

        [Fact]
        public void GridHtmlAttributes_ChangesUnderscoresToDashes()
        {
            StringWriter writer = new();
            new GridHtmlAttributes(new
            {
                id = "",
                src = "test.png",
                data_temp = 10000,
                data_null = (String?)null
            }).WriteTo(writer, HtmlEncoder.Default);

            Assert.Equal(" id=\"\" src=\"test.png\" data-temp=\"10000\"", writer.ToString());
        }

        [Fact]
        public void WriteTo_EncodesValues()
        {
            StringWriter writer = new();
            new GridHtmlAttributes(new
            {
                value = "Temp \"str\""
            }).WriteTo(writer, HtmlEncoder.Default);

            Assert.Equal(" value=\"Temp &quot;str&quot;\"", writer.ToString());
        }
    }
}
