using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid;

public class GridHtmlStringTests
{
    private StringWriter writer;

    public GridHtmlStringTests()
    {
        writer = new StringWriter();
    }

    [Fact]
    public void WriteTo_RawString()
    {
        new GridHtmlString("<test>").WriteTo(writer, null);

        Assert.Equal("<test>", writer.ToString());
    }

    [Fact]
    public void WriteTo_EncodedString()
    {
        new GridHtmlString("<test>").WriteTo(writer, HtmlEncoder.Default);

        Assert.Equal(HtmlEncoder.Default.Encode("<test>"), writer.ToString());
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData(null, "")]
    [InlineData("test", "test")]
    public void ToString_Value(String? value, String expected)
    {
        Assert.Equal(expected, new GridHtmlString(value).ToString());
    }
}
