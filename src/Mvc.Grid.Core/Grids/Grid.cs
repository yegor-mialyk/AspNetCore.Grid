using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public class Grid<T> : IGrid<T> where T : class
    {
        public String Url { get; set; }
        public String? Id { get; set; }
        public String Name { get; set; }
        public String? EmptyText { get; set; }

        public IGridSort<T> Sort { get; set; }
        public IQueryable<T> Source { get; set; }
        public IQueryCollection? Query { get; set; }
        public GridProcessingMode Mode { get; set; }
        public ViewContext? ViewContext { get; set; }
        public GridFilterMode FilterMode { get; set; }
        public String FooterPartialViewName { get; set; }
        public GridHtmlAttributes Attributes { get; set; }
        public HashSet<IGridProcessor<T>> Processors { get; set; }

        IGridColumns<IGridColumn> IGrid.Columns => Columns;
        public IGridColumnsOf<T> Columns { get; set; }

        IGridRows<Object> IGrid.Rows => Rows;
        public IGridRowsOf<T> Rows { get; set; }

        IGridPager? IGrid.Pager => Pager;
        public IGridPager<T>? Pager { get; set; }

        public Grid(IEnumerable<T> source)
        {
            Url = "";
            Name = "";
            FooterPartialViewName = "";
            Source = source.AsQueryable();
            FilterMode = GridFilterMode.Excel;
            Mode = GridProcessingMode.Automatic;
            Attributes = new GridHtmlAttributes();
            Processors = new HashSet<IGridProcessor<T>>();

            Columns = new GridColumns<T>(this);
            Rows = new GridRows<T>(this);
            Sort = new GridSort<T>(this);
        }
    }
}
