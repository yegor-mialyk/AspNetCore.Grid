using System;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public class GridPager<T> : IGridPager<T>
    {
        public IGrid<T> Grid { get; set; }

        public virtual Int32 TotalRows
        {
            get;
            set;
        }
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
                String prefix = String.IsNullOrEmpty(Grid.Name) ? "" : Grid.Name + "-";
                CurrentPageValue = Int32.TryParse(Grid.Query[prefix + "Page"], out Int32 page) ? page : CurrentPageValue;
                CurrentPageValue = CurrentPageValue > TotalPages ? TotalPages : CurrentPageValue;
                CurrentPageValue = CurrentPageValue <= 0 ? 1 : CurrentPageValue;

                return CurrentPageValue;
            }
            set
            {
                CurrentPageValue = value;
            }
        }
        public virtual Int32 RowsPerPage
        {
            get
            {
                String prefix = String.IsNullOrEmpty(Grid.Name) ? "" : Grid.Name + "-";
                RowsPerPageValue = Int32.TryParse(Grid.Query[prefix + "Rows"], out Int32 rows) ? rows : RowsPerPageValue;
                RowsPerPageValue = RowsPerPageValue <= 0 ? 1 : RowsPerPageValue;

                return RowsPerPageValue;
            }
            set
            {
                RowsPerPageValue = value;
            }
        }
        public virtual Int32 PagesToDisplay
        {
            get;
            set;
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

        public String CssClasses { get; set; }
        public String PartialViewName { get; set; }
        public GridProcessorType ProcessorType { get; set; }

        private Int32 CurrentPageValue { get; set; }
        private Int32 RowsPerPageValue { get; set; }

        public GridPager(IGrid<T> grid)
        {
            Grid = grid;
            CurrentPage = 1;
            RowsPerPage = 20;
            PagesToDisplay = 5;
            PartialViewName = "MvcGrid/_Pager";
            ProcessorType = GridProcessorType.Post;
        }

        public virtual IQueryable<T> Process(IQueryable<T> items)
        {
            TotalRows = items.Count();

            return items.Skip((CurrentPage - 1) * RowsPerPage).Take(RowsPerPage);
        }
    }
}
