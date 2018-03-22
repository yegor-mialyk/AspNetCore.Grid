using Microsoft.AspNetCore.Html;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public abstract class BaseGridColumn<T, TValue> : IGridColumn<T>
    {
        public String Name { get; set; }
        public String Format { get; set; }
        public String CssClasses { get; set; }
        public Boolean IsEncoded { get; set; }
        public IHtmlContent Title { get; set; }

        public Boolean? IsSortable { get; set; }
        public GridSortOrder? FirstSortOrder { get; set; }
        public GridSortOrder? InitialSortOrder { get; set; }
        public virtual GridSortOrder? SortOrder { get; set; }

        IGridColumnFilter IGridColumn.Filter => Filter;
        public virtual IGridColumnFilter<T> Filter { get; set; }

        public IGrid<T> Grid { get; set; }
        public Func<T, Object> RenderValue { get; set; }
        public GridProcessorType ProcessorType { get; set; }
        public Func<T, TValue> ExpressionValue { get; set; }
        LambdaExpression IGridColumn<T>.Expression => Expression;
        public Expression<Func<T, TValue>> Expression { get; set; }

        public abstract IQueryable<T> Process(IQueryable<T> items);
        public abstract IHtmlContent ValueFor(IGridRow<Object> row);
    }
}
