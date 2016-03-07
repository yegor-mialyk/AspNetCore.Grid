using System;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public class GridPager<T> : IGridPager<T>
    {
        public IGrid<T> Grid { get; set; }

        public Int32 TotalRows { get; set; }
        public Int32 InitialPage { get; set; }
        public Int32 RowsPerPage { get; set; }
        public Int32 PagesToDisplay { get; set; }

        public virtual Int32 TotalPages
        {
            get
            {
                return (Int32)Math.Ceiling(TotalRows / (Double)RowsPerPage);
            }
        }
        public virtual Int32 CurrentPage
        {
            get
            {
                if (CurrentPageIsSet)
                    return CurrentPageValue;

                Int32 page;
                CurrentPageValue = Int32.TryParse(Grid.Query[Grid.Name + "-Page"], out page) ? page : InitialPage;
                CurrentPageValue = CurrentPageValue > TotalPages ? TotalPages : CurrentPageValue;
                CurrentPageValue = CurrentPageValue <= 0 ? 1 : CurrentPageValue;
                CurrentPageIsSet = true;

                return CurrentPageValue;
            }
        }
        public virtual Int32 FirstDisplayPage
        {
            get
            {
                Int32 middlePage = (PagesToDisplay / 2) + (PagesToDisplay % 2);
                if (CurrentPage < middlePage)
                    return 1;

                if (CurrentPage - middlePage + PagesToDisplay > TotalPages)
                    return Math.Max(TotalPages - PagesToDisplay + 1, 1);

                return CurrentPage - middlePage + 1;
            }
        }
        private Int32 CurrentPageValue { get; set; }
        private Boolean CurrentPageIsSet { get; set; }

        public String CssClasses { get; set; }
        public String PartialViewName { get; set; }
        public GridProcessorType ProcessorType { get; set; }

        public GridPager(IGrid<T> grid)
        {
            Grid = grid;
            InitialPage = 1;
            Int32 rowsPerPage;
            PagesToDisplay = 5;
            PartialViewName = "MvcGrid/_Pager";
            ProcessorType = GridProcessorType.Post;
            RowsPerPage = Int32.TryParse(grid.Query[grid.Name + "-Rows"], out rowsPerPage) ? rowsPerPage : 20;
        }

        public virtual IQueryable<T> Process(IQueryable<T> items)
        {
            TotalRows = items.Count();

            return items.Skip((CurrentPage - 1) * RowsPerPage).Take(RowsPerPage);
        }
    }
}
