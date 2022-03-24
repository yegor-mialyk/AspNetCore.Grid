namespace NonFactors.Mvc.Grid;

public class GridColumnConfigTests
{
    [Fact]
    public void GridColumnConfig_Defaults()
    {
        GridColumnConfig actual = new();

        Assert.False(actual.Hidden);
        Assert.Empty(actual.Width);
        Assert.Empty(actual.Name);
    }
}
