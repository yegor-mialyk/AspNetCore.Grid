using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridEnumerableFilterTests
    {
        private Expression<Func<GridModel, IEnumerable<String?>?>> nEnumerableExpression;
        private Expression<Func<GridModel, List<String?>?>> nListExpression;
        private Expression<Func<GridModel, String?[]?>> nArrayExpression;
        private IQueryable<GridModel> items;

        public GridEnumerableFilterTests()
        {
            items = new[]
            {
                new GridModel { NullableArrayField = new[] { "", "678", "retest", "58" } },
                new GridModel { NullableArrayField = new[] { "10", "12", "33", "84", "58" } },
                new GridModel { NullableArrayField = new[] { null, "test", "2", "3", "4", "5" } },
                new GridModel { NullableArrayField = new[] { null, "TEST", "2", "3", "4", "5" } }
            }.AsQueryable();

            foreach (GridModel model in items)
            {
                model.NullableEnumerableField = model.NullableArrayField;
                model.NullableListField = model.NullableArrayField.ToList();
            }

            nEnumerableExpression = (model) => model.NullableEnumerableField;
            nArrayExpression = (model) => model.NullableArrayField;
            nListExpression = (model) => model.NullableListField;
        }

        [Fact]
        public void Method_Set()
        {
            String? actual = new EnumerableFilter<StringContainsFilter> { Method = "test" }.Method;
            String? expected = "test";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Values_Set()
        {
            StringValues actual = new EnumerableFilter<StringContainsFilter> { Values = "test" }.Values;
            StringValues expected = "test";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Case_Set()
        {
            GridFilterCase actual = new EnumerableFilter<StringContainsFilter> { Case = GridFilterCase.Upper }.Case;
            GridFilterCase expected = GridFilterCase.Upper;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NotEnumerable_ReturnsNull()
        {
            Expression<Func<GridModel, Object>> expression = (model) => "test";

            Assert.Null(new EnumerableFilter<StringEqualsFilter>().Apply(expression));
        }

        [Fact]
        public void Apply_OnList()
        {
            EnumerableFilter<StringEqualsFilter> filter = new EnumerableFilter<StringEqualsFilter> { Method = "equals", Values = new[] { "test", "33" } };
            filter.Case = GridFilterCase.Original;

            IEnumerable expected = items.Where(model => model.NullableListField.Any(item => item == "test" || item == "33"));
            IEnumerable actual = items.Where(nListExpression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_OnArray()
        {
            EnumerableFilter<StringEqualsFilter> filter = new EnumerableFilter<StringEqualsFilter> { Method = "equals", Values = new[] { "test", "33" } };
            filter.Case = GridFilterCase.Original;

            IEnumerable expected = items.Where(model => model.NullableArrayField.Any(item => item == "test" || item == "33"));
            IEnumerable actual = items.Where(nArrayExpression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_OnEnumerable()
        {
            EnumerableFilter<StringEqualsFilter> filter = new EnumerableFilter<StringEqualsFilter> { Method = "equals", Values = new[] { "test", "33" } };
            filter.Case = GridFilterCase.Original;

            IEnumerable expected = items.Where(model => model.NullableEnumerableField.Any(item => item == "test" || item == "33"));
            IEnumerable actual = items.Where(nEnumerableExpression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingOriginalCaseFilter()
        {
            EnumerableFilter<StringEqualsFilter> filter = new EnumerableFilter<StringEqualsFilter> { Method = "equals", Values = new[] { "test", "33" } };
            filter.Case = GridFilterCase.Original;

            IEnumerable expected = items.Where(model => model.NullableArrayField.Any(item => item == "test" || item == "33"));
            IEnumerable actual = items.Where(nListExpression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingUpperCaseFilter()
        {
            EnumerableFilter<StringEqualsFilter> filter = new EnumerableFilter<StringEqualsFilter> { Method = "equals", Values = new[] { "test", "33" } };
            filter.Case = GridFilterCase.Upper;

            IEnumerable expected = items.Where(model => model.NullableArrayField.Any(item => item != null && item.ToUpper() == "TEST" || item == "33"));
            IEnumerable actual = items.Where(nListExpression, filter);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_UsingLowerCaseFilter()
        {
            EnumerableFilter<StringEqualsFilter> filter = new EnumerableFilter<StringEqualsFilter> { Method = "equals", Values = new[] { "TEST", "33" } };
            filter.Case = GridFilterCase.Lower;

            IEnumerable expected = items.Where(model => model.NullableArrayField != null && model.NullableArrayField.Any(item => item != null && item.ToLower() == "test" || item == "33"));
            IEnumerable actual = items.Where(nListExpression, filter);

            Assert.Equal(expected, actual);
        }
    }
}
