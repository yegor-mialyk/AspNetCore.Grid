using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class GridColumnFilter<T, TValue> : IGridColumnFilter<T, TValue>
    {
        public String Name { get; set; }
        public Boolean? IsEnabled { get; set; }
        public String DefaultMethod { get; set; }
        public GridFilterCase? Case { get; set; }
        public GridFilterType? Type { get; set; }

        private Boolean OptionsIsSet { get; set; }
        public virtual IEnumerable<SelectListItem> Options
        {
            get
            {
                if (IsEnabled == true && !OptionsIsSet)
                    Options = GetFilters().OptionsFor(Column);

                return OptionsValue;
            }
            set
            {
                OptionsValue = value;
                OptionsIsSet = true;
            }
        }
        private IEnumerable<SelectListItem> OptionsValue { get; set; }

        public virtual String? Operator
        {
            get
            {
                if (IsEnabled == true && Type == GridFilterType.Double && !OperatorIsSet)
                {
                    String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
                    Operator = Column.Grid.Query?[prefix + Column.Name + "-op"].FirstOrDefault()?.ToLower();
                }

                return OperatorValue;
            }
            set
            {
                OperatorValue = value;
                OperatorIsSet = true;
            }
        }
        private String? OperatorValue { get; set; }
        private Boolean OperatorIsSet { get; set; }

        public virtual IGridFilter? First
        {
            get
            {
                if (IsEnabled == true && !FirstIsSet)
                    First = CreateFirstFilter();

                return FirstValue;
            }
            set
            {
                FirstValue = value;
                FirstIsSet = true;
            }
        }
        private Boolean FirstIsSet { get; set; }
        private IGridFilter? FirstValue { get; set; }

        public virtual IGridFilter? Second
        {
            get
            {
                if (IsEnabled == true && Type == GridFilterType.Double && !SecondIsSet)
                    Second = CreateSecondFilter();

                return SecondValue;
            }
            set
            {
                SecondValue = value;
                SecondIsSet = true;
            }
        }
        private Boolean SecondIsSet { get; set; }
        private IGridFilter? SecondValue { get; set; }

        public IGridColumn<T, TValue> Column { get; set; }

        public GridColumnFilter(IGridColumn<T, TValue> column)
        {
            Column = column;
            Name = GetName();
            DefaultMethod = "";
            OptionsValue = Array.Empty<SelectListItem>();
            IsEnabled = column.Expression.Body is MemberExpression ? IsEnabled : false;
        }

        public IQueryable<T> Apply(IQueryable<T> items)
        {
            if (IsEnabled != true)
                return items;

            Expression? expression = BuildFilterExpression();

            return expression == null ? items : items.Where(ToLambda(expression));
        }

        private IGridFilters GetFilters()
        {
            return Column.Grid.ViewContext?.HttpContext.RequestServices.GetService<IGridFilters>() ?? new GridFilters();
        }
        private IGridFilter? CreateFirstFilter()
        {
            String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
            String columnName = (prefix + Column.Name + "-").ToLower();
            String[] keys = FilterKeysFor(columnName);

            if (keys.Length == 0)
                return null;

            String method = keys[0].Substring(columnName.Length);

            if (Type == GridFilterType.Multi)
                return CreateFilter(method, Column.Grid.Query?[keys[0]] ?? "");

            return CreateFilter(method, Column.Grid.Query?[keys[0]].FirstOrDefault());
        }
        private IGridFilter? CreateSecondFilter()
        {
            String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
            String columnName = (prefix + Column.Name + "-").ToLower();
            String[] keys = FilterKeysFor(columnName);

            if (keys.Length == 0)
                return null;

            if (keys.Length == 1)
            {
                StringValues values = Column.Grid.Query?[keys[0]] ?? "";
                if (values.Count < 2)
                    return null;

                return CreateFilter(keys[0].Substring(columnName.Length), values[1]);
            }

            String method = keys[1].Substring(columnName.Length);
            String value = Column.Grid.Query?[keys[1]][0] ?? "";

            return CreateFilter(method, value);
        }
        private IGridFilter? CreateFilter(String method, StringValues values)
        {
            if (GetFilters().Create(typeof(TValue), method, values) is IGridFilter filter)
            {
                filter.Case = Case ?? GridFilterCase.Original;

                return filter;
            }

            return null;
        }

        private String GetName()
        {
            Type type = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (type.GetTypeInfo().IsEnum)
                return "enum";

            switch (System.Type.GetTypeCode(type))
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
                    return "number";
                case TypeCode.String:
                    return "text";
                case TypeCode.DateTime:
                    return "date";
                case TypeCode.Boolean:
                    return "boolean";
                default:
                    return type == typeof(Guid) ? "guid" : "";
            }
        }
        private Expression? BuildFilterExpression()
        {
            Expression? left = First?.Apply(Column.Expression.Body);

            if (Type == GridFilterType.Double && left != null)
            {
                Expression? right = Second?.Apply(Column.Expression.Body);

                if (right != null && "and".Equals(Operator, StringComparison.OrdinalIgnoreCase))
                    return Expression.AndAlso(left, right);

                if (right != null && "or".Equals(Operator, StringComparison.OrdinalIgnoreCase))
                    return Expression.OrElse(left, right);
            }

            return left;
        }
        private String[] FilterKeysFor(String prefix)
        {
            return Column
                .Grid
                .Query?
                .Keys
                .Where(key =>
                    key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) &&
                    !key.Equals(prefix + "op", StringComparison.OrdinalIgnoreCase))
                .ToArray() ?? Array.Empty<String>();
        }
        private Expression<Func<T, Boolean>> ToLambda(Expression expression)
        {
            return Expression.Lambda<Func<T, Boolean>>(expression, Column.Expression.Parameters[0]);
        }
    }
}
