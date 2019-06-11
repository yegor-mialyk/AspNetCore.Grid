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
            Assert.Empty(new GridHtmlAttributes());
        }

        #endregion

        #region GridHtmlAttributes(Object htmlAttributes)

        [Fact]
        public void GridHtmlAttributes_ChangesUnderscoresToDashes()
        {
            TextWriter writer = new StringWriter();
            new GridHtmlAttributes(new
            {
                id= "",
                src = "test.png",
                data_temp = 10000,
                data_null = (String)null
            }).WriteTo(writer, HtmlEncoder.Default);

            String expected = " id=\"\" src=\"test.png\" data-temp=\"10000\"";
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
