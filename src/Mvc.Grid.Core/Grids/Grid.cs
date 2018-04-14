using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public class Grid<T> : IGrid<T> where T : class
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String EmptyText { get; set; }
        public String SourceUrl { get; set; }
        public String CssClasses { get; set; }

        public IQueryable<T> Source { get; set; }
        public IQueryCollection Query { get; set; }
        public ViewContext ViewContext { get; set; }
        public GridFilterMode FilterMode { get; set; }
        public String FooterPartialViewName { get; set; }
        public GridHtmlAttributes Attributes { get; set; }
        public HashSet<IGridProcessor<T>> Processors { get; set; }

        IGridColumns<IGridColumn> IGrid.Columns => Columns;
        public IGridColumnsOf<T> Columns { get; set; }

        IGridRows<Object> IGrid.Rows => Rows;
        public IGridRowsOf<T> Rows { get; set; }

        IGridPager IGrid.Pager => Pager;
        public IGridPager<T> Pager { get; set; }

        public Grid(IEnumerable<T> source)
        {
            Query = new QueryCollection();
            Source = source.AsQueryable();
            Processors = new HashSet<IGridProcessor<T>>();

            Columns = new GridColumns<T>(this);
            Rows = new GridRows<T>(this);
        }
    }
}
