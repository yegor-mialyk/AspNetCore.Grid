using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class GridColumnFilter<T> : IGridColumnFilter<T>
    {
        public String Operator { get; set; }
        public IGridFilter First { get; set; }
        public IGridFilter Second { get; set; }
        public IGridColumn<T> Column { get; set; }
        public GridProcessorType ProcessorType { get; set; }

        public IQueryable<T> Process(IQueryable<T> items)
        {
            Expression filterExpression = CreateFilterExpression();
            if (filterExpression == null)
                return items;

            return items.Where(ToPredicate(filterExpression));
        }

        private Expression CreateFilterExpression()
        {
            Expression right = Second != null ? Second.Apply(Column.Expression.Body) : null;
            Expression left = First != null ? First.Apply(Column.Expression.Body) : null;

            if (left != null && right != null)
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
        private Expression<Func<T, Boolean>> ToPredicate(Expression expression)
        {
            return Expression.Lambda<Func<T, Boolean>>(expression, Column.Expression.Parameters[0]);
        }
    }
}
