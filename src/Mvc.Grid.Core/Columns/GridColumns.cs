using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class GridColumns<T> : List<IGridColumn<T>>, IGridColumnsOf<T> where T : class
    {
        public IGrid<T> Grid { get; set; }

        public GridColumns(IGrid<T> grid)
        {
            Grid = grid;
        }

        public virtual IGridColumn<T> Add()
        {
            return Add<Object>(model => null);
        }
        public virtual IGridColumn<T> Add<TValue>(Expression<Func<T, TValue>> expression)
        {
            IGridColumn<T> column = new GridColumn<T, TValue>(Grid, expression);
            Grid.Processors.Add(column);
            Add(column);

            return column;
        }

        public virtual IGridColumn<T> Insert(Int32 index)
        {
            return Insert<Object>(index, model => null);
        }
        public virtual IGridColumn<T> Insert<TValue>(Int32 index, Expression<Func<T, TValue>> expression)
        {
            IGridColumn<T> column = new GridColumn<T, TValue>(Grid, expression);
            Grid.Processors.Add(column);
            Insert(index, column);

            return column;
        }
    }
}
