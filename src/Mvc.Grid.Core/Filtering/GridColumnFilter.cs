using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class GridColumnFilter<T, TValue> : IGridColumnFilter<T, TValue>
    {
        public String Name { get; set; }
        public Boolean? IsMulti { get; set; }
        public Boolean? IsEnabled { get; set; }

        public String Operator { get; set; }
        public IGridFilter First { get; set; }
        public IGridFilter Second { get; set; }

        public IGridColumn<T, TValue> Column { get; set; }

        public GridColumnFilter(IGridColumn<T, TValue> column)
        {
            Column = column;
            IsEnabled = column.Expression.Body is MemberExpression ? (Boolean?)null : false;
        }

        public IQueryable<T> Apply(IQueryable<T> items)
        {
            if (IsEnabled != true)
                return items;

            Expression expression = CreateFilterExpression();

            return expression == null ? items : items.Where(ToLambda(expression));
        }

        private Expression CreateFilterExpression()
        {
            Expression right = Second?.Apply(Column.Expression.Body);
            Expression left = First?.Apply(Column.Expression.Body);

            if (IsMulti == true && left != null && right != null)
            {
                switch (Operator)
                {
                    case "And":
                        return Expression.AndAlso(left, right);
                    case "Or":
                        return Expression.OrElse(left, right);
                }
            }

            return left ?? right;
        }
        private Expression<Func<T, Boolean>> ToLambda(Expression expression)
        {
            return Expression.Lambda<Func<T, Boolean>>(expression, Column.Expression.Parameters[0]);
        }
    }
}
