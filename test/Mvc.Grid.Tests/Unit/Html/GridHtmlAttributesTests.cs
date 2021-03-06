using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid;

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
            value = "Temp \"str\"",
            nullValue = (String?)null
        }).WriteTo(writer, HtmlEncoder.Default);

        Assert.Equal(" value=\"Temp &quot;str&quot;\"", writer.ToString());
    }
}
