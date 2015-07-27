using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public class Grid<T> : IGrid<T> where T : class
    {
        public String Name { get; set; }
        public String EmptyText { get; set; }
        public String CssClasses { get; set; }

        public IQueryable<T> Source { get; set; }
        public HttpContext HttpContext { get; set; }
        public IReadableStringCollection Query { get; set; }
        public IList<IGridProcessor<T>> Processors { get; set; }

        IGridColumns<IGridColumn> IGrid.Columns { get { return Columns; } }
        public IGridColumnsOf<T> Columns { get; set; }

        IGridRows<Object> IGrid.Rows { get { return Rows; } }
        public IGridRowsOf<T> Rows { get; set; }

        public IGridPager<T> Pager { get; set; }
        IGridPager IGrid.Pager { get { return Pager; } }

        public Grid(IEnumerable<T> source)
        {
            Processors = new List<IGridProcessor<T>>();
            Source = source.AsQueryable();

            Name = "Grid";

            Columns = new GridColumns<T>(this);
            Rows = new GridRows<T>(this);
        }
    }
}
