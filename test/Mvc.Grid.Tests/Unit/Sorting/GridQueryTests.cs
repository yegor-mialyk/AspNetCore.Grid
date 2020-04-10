using System;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests
{
    public class GridQueryTests
    {
        [Fact]
        public void IsOrdered_False()
        {
            Assert.False(GridQuery.IsOrdered(Array.Empty<Object>().AsQueryable().Where(model => true)));
        }

        [Fact]
        public void IsOrdered_Ascending_True()
        {
            Assert.True(GridQuery.IsOrdered(Array.Empty<Object>().AsQueryable().OrderBy(model => 0)));
        }

        [Fact]
        public void IsOrdered_Descending_True()
        {
            Assert.True(GridQuery.IsOrdered(Array.Empty<Object>().AsQueryable().OrderByDescending(model => 0)));
        }
    }
}
