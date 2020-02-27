using System;
using System.IO;
using System.Text.Encodings.Web;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests
{
    public class GridHtmlStringTests
    {
        private TextWriter writer;

        public GridHtmlStringTests()
        {
            writer = new StringWriter();
        }

        [Fact]
        public void WriteTo_RawString()
        {
            new GridHtmlString("<test>").WriteTo(writer, null);

            String? actual = writer.ToString();
            String? expected = "<test>";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WriteTo_EncodedString()
        {
            new GridHtmlString("<test>").WriteTo(writer, HtmlEncoder.Default);

            String? expected = HtmlEncoder.Default.Encode("<test>");
            String? actual = writer.ToString();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(null, "")]
        [InlineData("test", "test")]
        public void ToString_Value(String value, String representation)
        {
            String actual = new GridHtmlString(value).ToString();
            String expected = representation;

            Assert.Equal(expected, actual);
        }
    }
}
