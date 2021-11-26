namespace NonFactors.Mvc.Grid;

public class GridTests
{
    [Fact]
    public void IGrid_ReturnsColumns()
    {
        Grid<GridModel> grid = new(Array.Empty<GridModel>());

        Object actual = ((IGrid)grid).Columns;
        Object expected = grid.Columns;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void IGrid_ReturnsRows()
    {
        Grid<GridModel> grid = new(Array.Empty<GridModel>());

        Assert.Equal(grid.Rows, ((IGrid)grid).Rows);
    }

    [Fact]
    public void IGrid_ReturnsPager()
    {
        Grid<GridModel> grid = new(Array.Empty<GridModel>());

        Assert.Equal(grid.Pager, ((IGrid)grid).Pager);
    }

    [Fact]
    public void Grid_SetsName()
    {
        Assert.Empty(new Grid<GridModel>(Array.Empty<GridModel>()).Name);
    }

    [Fact]
    public void Grid_SetsUrl()
    {
        Assert.Empty(new Grid<GridModel>(Array.Empty<GridModel>()).Url);
    }

    [Fact]
    public void Grid_SetsFooterPartialViewName()
    {
        Assert.Empty(new Grid<GridModel>(Array.Empty<GridModel>()).FooterPartialViewName);
    }

    [Fact]
    public void Grid_SetsProcessors()
    {
        Assert.Empty(new Grid<GridModel>(Array.Empty<GridModel>()).Processors);
    }

    [Fact]
    public void Grid_SetsSource()
    {
        IQueryable<GridModel> expected = new GridModel[2].AsQueryable();
        IQueryable<GridModel> actual = new Grid<GridModel>(expected).Source;

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Grid_SetsFilterMode()
    {
        GridFilterMode actual = new Grid<GridModel>(Array.Empty<GridModel>()).FilterMode;
        GridFilterMode expected = GridFilterMode.Excel;

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Grid_SetsEmptyAttributes()
    {
        Assert.Empty(new Grid<GridModel>(Array.Empty<GridModel>()).Attributes);
    }

    [Fact]
    public void Grid_SetsEmptyColumns()
    {
        Assert.Empty(new Grid<GridModel>(Array.Empty<GridModel>()).Columns);
    }

    [Fact]
    public void Grid_SetsMode()
    {
        Assert.Equal(GridProcessingMode.Automatic, new Grid<GridModel>(Array.Empty<GridModel>()).Mode);
    }

    [Fact]
    public void Grid_SetsEmptyRows()
    {
        Assert.Empty(new Grid<GridModel>(Array.Empty<GridModel>()).Rows);
    }

    [Fact]
    public void Grid_SetsSort()
    {
        Assert.NotNull(new Grid<GridModel>(Array.Empty<GridModel>()).Sort);
    }
}
