using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NonFactors.Mvc.Grid.Tests.Unit
{
    public class GridRowsTests
    {
        #region GridRows(IGrid<T> grid)

        [Fact]
        public void GridRows_SetsGrid()
        {
            IGrid<GridModel> expected = new Grid<GridModel>(new GridModel[0]);
            IGrid<GridModel> actual = new GridRows<GridModel>(expected).Grid;

            Assert.Same(expected, actual);
        }

        #endregion

        #region GetEnumerator()

        [Fact]
        public void GetEnumerator_OnNullCurrentRowsProcessesRows()
        {
            IQueryable<GridModel> items = new[] { new GridModel(), new GridModel() }.AsQueryable();
            IGridProcessor<GridModel> postProcessor = Substitute.For<IGridProcessor<GridModel>>();
            IGridProcessor<GridModel> preProcessor = Substitute.For<IGridProcessor<GridModel>>();
            IGridProcessor<GridModel> manProcessor = Substitute.For<IGridProcessor<GridModel>>();
            IQueryable<GridModel> postProcessedItems = new[] { new GridModel() }.AsQueryable();
            IQueryable<GridModel> preProcessedItems = new[] { new GridModel() }.AsQueryable();
            manProcessor.ProcessorType = GridProcessorType.Manual;
            postProcessor.ProcessorType = GridProcessorType.Post;
            preProcessor.ProcessorType = GridProcessorType.Pre;
            Grid<GridModel> grid = new Grid<GridModel>(items);

            postProcessor.Process(preProcessedItems).Returns(postProcessedItems);
            manProcessor.Process(items).Returns(new GridModel[0].AsQueryable());
            preProcessor.Process(items).Returns(preProcessedItems);
            grid.Processors.Add(postProcessor);
            grid.Processors.Add(preProcessor);
            grid.Processors.Add(manProcessor);

            GridRows<GridModel> rows = new GridRows<GridModel>(grid);
            IEnumerable<IGridRow<GridModel>> currentRows = rows.CurrentRows;

            IEnumerable<Object> actual = rows.ToList().Select(row => row.Model);
            IEnumerable<Object> expected = postProcessedItems;

            postProcessor.Received().Process(preProcessedItems);
            manProcessor.DidNotReceive().Process(items);
            preProcessor.Received().Process(items);

            Assert.Equal(expected, actual);
            Assert.Null(currentRows);
        }

        [Fact]
        public void GetEnumerator_SetsRowCssClasses()
        {
            IQueryable<GridModel> items = new[] { new GridModel(), new GridModel() }.AsQueryable();
            Grid<GridModel> grid = new Grid<GridModel>(items);

            GridRows<GridModel> rows = new GridRows<GridModel>(grid);
            rows.CssClasses = (model) => "grid-row";

            Assert.True(rows.All(row => row.CssClasses == "grid-row"));
        }

        [Fact]
        public void GetEnumerator_ReturnsCurrentRows()
        {
            IQueryable<GridModel> items = new[] { new GridModel(), new GridModel() }.AsQueryable();
            IGridProcessor<GridModel> preProcessor = Substitute.For<IGridProcessor<GridModel>>();
            preProcessor.Process(items).Returns(new GridModel[0].AsQueryable());
            preProcessor.ProcessorType = GridProcessorType.Pre;
            Grid<GridModel> grid = new Grid<GridModel>(items);

            GridRows<GridModel> rows = new GridRows<GridModel>(grid);
            rows.ToList();

            grid.Processors.Add(preProcessor);

            IEnumerable<Object> actual = rows.ToList().Select(row => row.Model);
            IEnumerable<Object> expected = items;

            preProcessor.DidNotReceive().Process(Arg.Any<IQueryable<GridModel>>());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEnumerator_ReturnsSameEnumerable()
        {
            GridModel[] items = { new GridModel(), new GridModel() };
            Grid<GridModel> grid = new Grid<GridModel>(items);

            GridRows<GridModel> rows = new GridRows<GridModel>(grid);

            IEnumerator actual = ((IEnumerable)rows).GetEnumerator();
            IEnumerator expected = rows.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
                Assert.Same((expected.Current as IGridRow<GridModel>).Model, (actual.Current as IGridRow<GridModel>).Model);
        }

        #endregion
    }
}
