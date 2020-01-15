using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class GridColumnSort<T, TValue> : IGridColumnSort<T, TValue>
    {
        public Boolean? IsEnabled { get; set; }

        public virtual GridSortOrder? Order
        {
            get
            {
                if (IsEnabled == true && !OrderIsSet)
                {
                    String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
                    if (String.Equals(Column.Grid.Query?[prefix + "sort"], Column.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        String? order = Column.Grid.Query?[prefix + "order"];

                        if ("asc".Equals(order, StringComparison.OrdinalIgnoreCase))
                            Order = GridSortOrder.Asc;
                        else if ("desc".Equals(order, StringComparison.OrdinalIgnoreCase))
                            Order = GridSortOrder.Desc;
                        else
                            Order = null;
                    }
                    else
                    {
                        Order = null;
                    }
                }

                return OrderValue;
            }
            set
            {
                OrderValue = value;
                OrderIsSet = true;
            }
        }
        private Boolean OrderIsSet { get; set; }
        private GridSortOrder? OrderValue { get; set; }

        public GridSortOrder FirstOrder { get; set; }

        public IGridColumn<T, TValue> Column { get; set; }

        public GridColumnSort(IGridColumn<T, TValue> column)
        {
            Column = column;
            FirstOrder = GridSortOrder.Asc;
            IsEnabled = column.Expression.Body is MemberExpression ? IsEnabled : false;
        }

        public IQueryable<T> Apply(IQueryable<T> items)
        {
            if (IsEnabled != true || Order == null)
                return items;

            if (Order == GridSortOrder.Asc)
                return items.OrderBy(Column.Expression);

            return items.OrderByDescending(Column.Expression);
        }
    }
}
