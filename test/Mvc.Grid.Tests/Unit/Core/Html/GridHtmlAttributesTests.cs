using System;
using System.IO;
using System.Text.Encodings.Web;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridHtmlAttributesTests
    {
        #region GridHtmlAttributes()

        [Fact]
        public void GridHtmlAttributes_Empty()
        {
            TextWriter writer = new StringWriter();
            new GridHtmlAttributes().WriteTo(writer, HtmlEncoder.Default);

            String actual = writer.ToString();

            Assert.Empty(actual);
        }

        #endregion

        #region GridHtmlAttributes(Object htmlAttributes)

        [Fact]
        public void GridHtmlAttributes_ChangesUnderscoresToDashes()
        {
            TextWriter writer = new StringWriter();
            new GridHtmlAttributes(new { data_temp = 100, src = "test.png" }).WriteTo(writer, HtmlEncoder.Default);

            String expected = " data-temp=\"100\" src=\"test.png\"";
            String actual = writer.ToString();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region WriteTo(TextWriter writer, HtmlEncoder encoder)

        [Fact]
        public void WriteTo_EncodesValues()
        {
            TextWriter writer = new StringWriter();
            new GridHtmlAttributes(new { value = "Temp \"str\"" }).WriteTo(writer, HtmlEncoder.Default);

            String expected = " value=\"Temp &quot;str&quot;\"";
            String actual = writer.ToString();

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
