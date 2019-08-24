using System;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridQueryTests
    {
        [Fact]
        public void IsOrdered_False()
        {
            Assert.False(GridQuery.IsOrdered(new Object[0].OrderBy(model => 0).AsQueryable()));
        }

        [Fact]
        public void IsOrdered_True()
        {
            Assert.True(GridQuery.IsOrdered(new Object[0].AsQueryable().OrderBy(model => 0)));
        }
    }
}
