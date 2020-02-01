using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridQueryTests
    {
        [Fact]
        public void IsOrdered_False()
        {
            Assert.False(GridQuery.IsOrdered(Array.Empty<Object>().AsQueryable().Where(model => true)));
        }

        [Fact]
        public void IsOrdered_True()
        {
            Assert.True(GridQuery.IsOrdered(Array.Empty<Object>().AsQueryable().OrderBy(model => 0)));
        }
    }
}
