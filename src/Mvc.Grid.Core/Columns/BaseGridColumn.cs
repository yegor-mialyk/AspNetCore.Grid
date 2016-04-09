using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
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

        public String FilterName { get; set; }
        public Boolean? IsFilterable { get; set; }
        public Boolean? IsMultiFilterable { get; set; }
        IGridColumnFilter IFilterableColumn.Filter => Filter;
        public virtual IGridColumnFilter<T> Filter { get; set; }

        public IGrid<T> Grid { get; set; }
        public Func<T, Object> RenderValue { get; set; }
        public GridProcessorType ProcessorType { get; set; }
        public Func<T, TValue> ExpressionValue { get; set; }
        LambdaExpression IGridColumn<T>.Expression => Expression;
        public Expression<Func<T, TValue>> Expression { get; set; }

        public virtual IGridColumn<T> RenderedAs(Func<T, Object> value)
        {
            RenderValue = value;

            return this;
        }

        public virtual IGridColumn<T> MultiFilterable(Boolean isMultiple)
        {
            IsMultiFilterable = isMultiple;

            return this;
        }
        public virtual IGridColumn<T> Filterable(Boolean isFilterable)
        {
            IsFilterable = isFilterable;

            return this;
        }
        public virtual IGridColumn<T> FilteredAs(String filterName)
        {
            FilterName = filterName;

            return this;
        }

        public virtual IGridColumn<T> InitialSort(GridSortOrder order)
        {
            InitialSortOrder = order;

            return this;
        }
        public virtual IGridColumn<T> FirstSort(GridSortOrder order)
        {
            FirstSortOrder = order;

            return this;
        }
        public virtual IGridColumn<T> Sortable(Boolean isSortable)
        {
            IsSortable = isSortable;

            return this;
        }

        public virtual IGridColumn<T> Encoded(Boolean isEncoded)
        {
            IsEncoded = isEncoded;

            return this;
        }
        public virtual IGridColumn<T> Formatted(String format)
        {
            Format = format;

            return this;
        }
        public virtual IGridColumn<T> Css(String cssClasses)
        {
            CssClasses = cssClasses;

            return this;
        }
        public virtual IGridColumn<T> Titled(Object value)
        {
            Title = value as IHtmlContent ?? new HtmlString(value?.ToString());

            return this;
        }
        public virtual IGridColumn<T> Named(String name)
        {
            Name = name;

            return this;
        }

        public abstract IQueryable<T> Process(IQueryable<T> items);
        public abstract IHtmlContent ValueFor(IGridRow<Object> row);
    }
}
