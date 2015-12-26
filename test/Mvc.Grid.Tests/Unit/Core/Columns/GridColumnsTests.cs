using Microsoft.AspNet.Http.Internal;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridColumnsTests
    {
        private GridColumns<GridModel> columns;

        public GridColumnsTests()
        {
            IDictionary<String, StringValues> query = new Dictionary<String, StringValues>();
            columns = new GridColumns<GridModel>(Substitute.For<IGrid<GridModel>>());
            columns.Grid.Processors = new List<IGridProcessor<GridModel>>();
            columns.Grid.Query = new ReadableStringCollection(query);
        }

        #region Constructor: GridColumns(IGrid<T> grid)

        [Fact]
        public void GridColumns_SetsGrid()
        {
            IGrid actual = new GridColumns<GridModel>(columns.Grid).Grid;
            IGrid expected = columns.Grid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Add<TValue>(Expression<Func<T, TValue>> expression)

        [Fact]
        public void Add_GridColumn()
        {
            Expression<Func<GridModel, String>> expression = (model) => model.Name;
            columns.Add(expression);

            GridColumn<GridModel, String> expected = new GridColumn<GridModel, String>(columns.Grid, expression);
            GridColumn<GridModel, String> actual = columns.Single() as GridColumn<GridModel, String>;

            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.IsFilterable, actual.IsFilterable);
            Assert.Equal(expected.FilterName, actual.FilterName);
            Assert.Equal(expected.Expression, actual.Expression);
            Assert.Equal(expected.CssClasses, actual.CssClasses);
            Assert.Equal(expected.IsSortable, actual.IsSortable);
            Assert.Equal(expected.SortOrder, actual.SortOrder);
            Assert.Equal(expected.IsEncoded, actual.IsEncoded);
            Assert.Equal(expected.Format, actual.Format);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Add_GridColumnProcessor()
        {
            columns.Add(model => model.Name);

            Object actual = columns.Grid.Processors.Single();
            Object expected = columns.Single();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Add_ReturnsAddedColumn()
        {
            IGridColumn actual = columns.Add(model => model.Name);
            IGridColumn expected = columns.Single();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Insert<TValue>(Int32 index, Expression<Func<T, TValue>> expression)

        [Fact]
        public void Insert_GridColumn()
        {
            Expression<Func<GridModel, Int32>> expression = (model) => model.Sum;
            columns.Add(model => model.Name);
            columns.Insert(0, expression);

            GridColumn<GridModel, Int32> expected = new GridColumn<GridModel, Int32>(columns.Grid, expression);
            GridColumn<GridModel, Int32> actual = columns.First() as GridColumn<GridModel, Int32>;

            Assert.Equal(expected.ProcessorType, actual.ProcessorType);
            Assert.Equal(expected.IsFilterable, actual.IsFilterable);
            Assert.Equal(expected.FilterName, actual.FilterName);
            Assert.Equal(expected.Expression, actual.Expression);
            Assert.Equal(expected.CssClasses, actual.CssClasses);
            Assert.Equal(expected.IsSortable, actual.IsSortable);
            Assert.Equal(expected.SortOrder, actual.SortOrder);
            Assert.Equal(expected.IsEncoded, actual.IsEncoded);
            Assert.Equal(expected.Format, actual.Format);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Grid, actual.Grid);
        }

        [Fact]
        public void Insert_GridColumnProcessor()
        {
            columns.Insert(0, model => model.Name);

            Object actual = columns.Grid.Processors.Single();
            Object expected = columns.Single();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Insert_ReturnsInsertedColumn()
        {
            IGridColumn actual = columns.Insert(0, model => model.Name);
            IGridColumn expected = columns.Single();

            Assert.Same(expected, actual);
        }

        #endregion
    }
}
