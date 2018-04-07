using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class NumberFilterTests : BaseGridFilterTests
    {
        private Expression<Func<GridModel, Int32?>> nSumExpression;
        private Expression<Func<GridModel, Int32>> sumExpression;
        private IQueryable<GridModel> items;
        private NumberFilter<Int32> filter;

        public NumberFilterTests()
        {
            items = new[]
            {
                new GridModel(),
                new GridModel { NSum = 1, Sum = 2 },
                new GridModel { NSum = 2, Sum = 1 }
            }.AsQueryable();

            nSumExpression = (model) => model.NSum;
            sumExpression = (model) => model.Sum;
            filter = new NumberFilter<Int32>();
        }

        #region Apply(Expression expression)

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        [InlineData("79228162514264337593543950336")]
        [InlineData("-79228162514264337593543950336")]
        public void Apply_BadDecimalValue_ReturnsNull(String value)
        {
            NumberFilter<Decimal> numberFilter = new NumberFilter<Decimal> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        [InlineData("1.8076931348623157E+308")]
        [InlineData("-1.8076931348623157E+308")]
        public void Apply_BadDoubleValue_ReturnsNull(String value)
        {
            NumberFilter<Double> numberFilter = new NumberFilter<Double> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        [InlineData("3.50282347E+38")]
        [InlineData("-3.50282347E+38")]
        public void Apply_BadSingleValue_ReturnsNull(String value)
        {
            NumberFilter<Single> numberFilter = new NumberFilter<Single> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        [InlineData("9223372036854775808")]
        [InlineData("-9223372036854775809")]
        public void Apply_BadInt64Value_ReturnsNull(String value)
        {
            NumberFilter<Int64> numberFilter = new NumberFilter<Int64> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("test")]
        [InlineData("18446744073709551616")]
        public void Apply_BadUInt64Value_ReturnsNull(String value)
        {
            NumberFilter<UInt64> numberFilter = new NumberFilter<UInt64> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        [InlineData("2147483648")]
        [InlineData("-2147483649")]
        public void Apply_BadInt32Value_ReturnsNull(String value)
        {
            NumberFilter<Int32> numberFilter = new NumberFilter<Int32> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("test")]
        [InlineData("4294967296")]
        public void Apply_BadUInt32Value_ReturnsNull(String value)
        {
            NumberFilter<UInt32> numberFilter = new NumberFilter<UInt32> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("test")]
        [InlineData("32768")]
        [InlineData("-32769")]
        public void Apply_BadInt16Value_ReturnsNull(String value)
        {
            NumberFilter<Int16> numberFilter = new NumberFilter<Int16> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("test")]
        [InlineData("65536")]
        public void Apply_BadUInt16Value_ReturnsNull(String value)
        {
            NumberFilter<UInt16> numberFilter = new NumberFilter<UInt16> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("128")]
        [InlineData("-129")]
        [InlineData("test")]
        public void Apply_BadSByteValue_ReturnsNull(String value)
        {
            NumberFilter<SByte> numberFilter = new NumberFilter<SByte> { Method = "equals", Value = value };

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("-1")]
        [InlineData("256")]
        [InlineData("test")]
        public void Apply_BadByteValue_ReturnsNull(String value)
        {
            NumberFilter<Byte> numberFilter = new NumberFilter<Byte>();
            numberFilter.Method = "equals";
            numberFilter.Value = value;

            Assert.Null(numberFilter.Apply(sumExpression.Body));
        }

        [Fact]
        public void Apply_NullableEqualsFilter()
        {
            filter.Value = "1";
            filter.Method = "equals";

            IEnumerable expected = items.Where(model => model.NSum == 1);
            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_EqualsFilter()
        {
            filter.Value = "1";
            filter.Method = "equals";

            IEnumerable expected = items.Where(model => model.Sum == 1);
            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableNotEqualsFilter()
        {
            filter.Value = "1";
            filter.Method = "not-equals";

            IEnumerable expected = items.Where(model => model.NSum != 1);
            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NotEqualsFilter()
        {
            filter.Value = "1";
            filter.Method = "not-equals";

            IEnumerable expected = items.Where(model => model.Sum != 1);
            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableLessThanFilter()
        {
            filter.Value = "1";
            filter.Method = "less-than";

            IEnumerable expected = items.Where(model => model.NSum < 1);
            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_LessThanFilter()
        {
            filter.Value = "1";
            filter.Method = "less-than";

            IEnumerable expected = items.Where(model => model.Sum < 1);
            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableGreaterThanFilter()
        {
            filter.Value = "1";
            filter.Method = "greater-than";

            IEnumerable expected = items.Where(model => model.NSum > 1);
            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_GreaterThanFilter()
        {
            filter.Value = "1";
            filter.Method = "greater-than";

            IEnumerable expected = items.Where(model => model.Sum > 1);
            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableLessThanOrEqualFilter()
        {
            filter.Value = "1";
            filter.Method = "less-than-or-equal";

            IEnumerable expected = items.Where(model => model.NSum <= 1);
            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_LessThanOrEqualFilter()
        {
            filter.Value = "1";
            filter.Method = "less-than-or-equal";

            IEnumerable expected = items.Where(model => model.Sum <= 1);
            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_NullableGreaterThanOrEqualFilter()
        {
            filter.Value = "1";
            filter.Method = "greater-than-or-equal";

            IEnumerable expected = items.Where(model => model.NSum >= 1);
            IEnumerable actual = Filter(items, filter.Apply(nSumExpression.Body), nSumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_GreaterThanOrEqualFilter()
        {
            filter.Value = "1";
            filter.Method = "greater-than-or-equal";

            IEnumerable expected = items.Where(model => model.Sum >= 1);
            IEnumerable actual = Filter(items, filter.Apply(sumExpression.Body), sumExpression);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Apply_BadMethod_ReturnsNull()
        {
            filter.Value = "1";
            filter.Method = "test";

            Assert.Null(filter.Apply(sumExpression.Body));
        }

        #endregion
    }
}
