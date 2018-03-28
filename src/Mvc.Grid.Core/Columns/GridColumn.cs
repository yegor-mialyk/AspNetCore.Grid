using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;

namespace NonFactors.Mvc.Grid
{
    public class GridColumn<T, TValue> : BaseGridColumn<T, TValue> where T : class
    {
        private Boolean FilterIsSet { get; set; }
        public override IGridColumnFilter<T, TValue> Filter
        {
            get
            {
                if (!FilterIsSet)
                    Filter = Grid.ViewContext.HttpContext.RequestServices.GetRequiredService<IGridFilters>().GetFilter(this);

                return base.Filter;
            }
            set
            {
                base.Filter = value;
                FilterIsSet = true;
            }
        }

        public GridColumn(IGrid<T> grid, Expression<Func<T, TValue>> expression)
        {
            Grid = grid;
            IsEncoded = true;
            Expression = expression;
            Title = GetTitle(expression);
            ProcessorType = GridProcessorType.Pre;
            ExpressionValue = expression.Compile();
            Sort = new GridColumnSort<T, TValue>(this);
            Name = ExpressionHelper.GetExpressionText(expression);
        }

        public override IQueryable<T> Process(IQueryable<T> items)
        {
            return Sort.Apply(Filter.Apply(items));
        }
        public override IHtmlContent ValueFor(IGridRow<Object> row)
        {
            Object value = GetValueFor(row);

            if (value == null)
                return HtmlString.Empty;

            if (value is IHtmlContent content)
                return content;

            if (Format != null)
                value = String.Format(Format, value);

            if (IsEncoded)
                return new HtmlString(HtmlEncoder.Default.Encode(value.ToString()));

            return new HtmlString(value.ToString());
        }

        private IHtmlContent GetTitle(Expression<Func<T, TValue>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;
            DisplayAttribute display = body?.Member.GetCustomAttribute<DisplayAttribute>();

            return new HtmlString(display?.GetShortName());
        }
        private Object GetValueFor(IGridRow<Object> row)
        {
            try
            {
                if (RenderValue != null)
                    return RenderValue(row.Model as T);

                return ExpressionValue(row.Model as T);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
