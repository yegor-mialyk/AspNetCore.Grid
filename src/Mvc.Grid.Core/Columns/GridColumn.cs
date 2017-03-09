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
        private Boolean SortOrderIsSet { get; set; }
        public override GridSortOrder? SortOrder
        {
            get
            {
                if (SortOrderIsSet)
                    return base.SortOrder;

                if (Grid.Query[Grid.Name + "-Sort"] == Name)
                {
                    String orderValue = Grid.Query[Grid.Name + "-Order"];
                    if (Enum.TryParse(orderValue, out GridSortOrder order))
                        SortOrder = order;
                }
                else if (Grid.Query[Grid.Name + "-Sort"] == StringValues.Empty)
                {
                    SortOrder = InitialSortOrder;
                }

                SortOrderIsSet = true;

                return base.SortOrder;
            }
            set
            {
                base.SortOrder = value;
                SortOrderIsSet = true;
            }
        }

        private Boolean FilterIsSet { get; set; }
        public override IGridColumnFilter<T> Filter
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
            FilterName = GetFilterName();
            ProcessorType = GridProcessorType.Pre;
            ExpressionValue = expression.Compile();
            IsSortable = IsFilterable = IsMember(expression);
            Name = ExpressionHelper.GetExpressionText(expression);
        }

        public override IQueryable<T> Process(IQueryable<T> items)
        {
            if (IsFilterable == true && Filter != null)
                items = Filter.Process(items);

            if (IsSortable != true || SortOrder == null)
                return items;

            if (SortOrder == GridSortOrder.Asc)
                return items.OrderBy(Expression);

            return items.OrderByDescending(Expression);
        }
        public override IHtmlContent ValueFor(IGridRow<Object> row)
        {
            Object value = GetValueFor(row);
            if (value == null) return HtmlString.Empty;
            if (value is IHtmlContent) return value as IHtmlContent;
            if (Format != null) value = String.Format(Format, value);
            if (IsEncoded) return new HtmlString(HtmlEncoder.Default.Encode(value.ToString()));

            return new HtmlString(value.ToString());
        }

        private Boolean? IsMember(Expression<Func<T, TValue>> expression)
        {
            if (expression.Body is MemberExpression)
                return null;

            return false;
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
        private String GetFilterName()
        {
            Type type = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (type.GetTypeInfo().IsEnum) return null;

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "Number";
                case TypeCode.String:
                    return "Text";
                case TypeCode.DateTime:
                    return "Date";
                case TypeCode.Boolean:
                    return "Boolean";
                default:
                    return null;
            }
        }
    }
}
