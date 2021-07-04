using Xunit;

namespace NonFactors.Mvc.Grid
{
    public class GridConfigTests
    {
        [Fact]
        public void GridConfig_Defaults()
        {
            GridConfig actual = new();

            Assert.Empty(actual.Columns);
            Assert.Empty(actual.Name);
        }
    }
}
