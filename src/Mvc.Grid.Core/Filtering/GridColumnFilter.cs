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
        public GridFilterType? Type { get; set; }

        private Boolean OptionsIsSet { get; set; }
        public virtual IEnumerable<SelectListItem> Options
        {
            get
            {
                if (IsEnabled == true && !OptionsIsSet)
                    Options = GetFilters().GetFilterOptions(Column);

                return OptionsValue;
            }
            set
            {
                OptionsValue = value;
                OptionsIsSet = true;
            }
        }
        private IEnumerable<SelectListItem> OptionsValue { get; set; }

        public virtual String Operator
        {
            get
            {
                if (IsEnabled == true && Type == GridFilterType.Double && !OperatorIsSet)
                {
                    String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
                    Operator = Column.Grid.Query[prefix + Column.Name + "-op"].FirstOrDefault()?.ToLower();
                }

                return OperatorValue;
            }
            set
            {
                OperatorValue = value;
                OperatorIsSet = true;
            }
        }
        private String OperatorValue { get; set; }
        private Boolean OperatorIsSet { get; set; }

        public virtual IGridFilter First
        {
            get
            {
                if (IsEnabled == true && !FirstIsSet)
                    First = GetFirstFilter();

                return FirstValue;
            }
            set
            {
                FirstValue = value;
                FirstIsSet = true;
            }
        }
        private Boolean FirstIsSet { get; set; }
        private IGridFilter FirstValue { get; set; }

        public virtual IGridFilter Second
        {
            get
            {
                if (IsEnabled == true && Type == GridFilterType.Double && !SecondIsSet)
                    Second = GetSecondFilter();

                return SecondValue;
            }
            set
            {
                SecondValue = value;
                SecondIsSet = true;
            }
        }
        private Boolean SecondIsSet { get; set; }
        private IGridFilter SecondValue { get; set; }

        public IGridColumn<T, TValue> Column { get; set; }

        public GridColumnFilter(IGridColumn<T, TValue> column)
        {
            Column = column;
            Name = GetName();
            IsEnabled = column.Expression.Body is MemberExpression ? IsEnabled : false;
        }

        public IQueryable<T> Apply(IQueryable<T> items)
        {
            if (IsEnabled != true)
                return items;

            Expression expression = CreateFilterExpression();

            return expression == null ? items : items.Where(ToLambda(expression));
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
                    return type == typeof(Guid) ? "guid" : null;
            }
        }
        private IGridFilters GetFilters()
        {
            return Column.Grid.ViewContext?.HttpContext.RequestServices.GetService<IGridFilters>() ?? new GridFilters();
        }
        private IGridFilter GetFirstFilter()
        {
            String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
            String columnName = (prefix + Column.Name + "-").ToLower();
            String[] keys = GetFilterKeys(columnName);

            if (keys.Length == 0)
                return null;

            String method = keys[0].Substring(columnName.Length);
            String value = Column.Grid.Query[keys[0]][0];

            return GetFilter(method, value);
        }
        private IGridFilter GetSecondFilter()
        {
            String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
            String columnName = (prefix + Column.Name + "-").ToLower();
            String[] keys = GetFilterKeys(columnName);

            if (keys.Length == 0)
                return null;

            if (keys.Length == 1)
            {
                StringValues values = Column.Grid.Query[keys[0]];
                if (values.Count < 2)
                    return null;

                return GetFilter(keys[0].Substring(columnName.Length), values[1]);
            }

            String method = keys[1].Substring(columnName.Length);
            String value = Column.Grid.Query[keys[1]][0];

            return GetFilter(method, value);
        }
        private String[] GetFilterKeys(String columnName)
        {
            return Column
                .Grid
                .Query
                .Keys
                .Where(key =>
                    key.StartsWith(columnName, StringComparison.OrdinalIgnoreCase) &&
                    !key.Equals(columnName + "op", StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
        private IGridFilter GetFilter(String method, String value)
        {
            IGridFilter filter = GetFilters().GetFilter(typeof(TValue), method);

            if (filter != null)
                filter.Value = value;

            return filter;
        }

        private Expression CreateFilterExpression()
        {
            Expression left = First?.Apply(Column.Expression.Body);
            Expression right = Second?.Apply(Column.Expression.Body);

            if (Type == GridFilterType.Double && left != null && right != null)
            {
                if ("and".Equals(Operator, StringComparison.OrdinalIgnoreCase))
                    return Expression.AndAlso(left, right);

                if ("or".Equals(Operator, StringComparison.OrdinalIgnoreCase))
                    return Expression.OrElse(left, right);
            }

            return left ?? right;
        }
        private Expression<Func<T, Boolean>> ToLambda(Expression expression)
        {
            return Expression.Lambda<Func<T, Boolean>>(expression, Column.Expression.Parameters[0]);
        }
    }
}
