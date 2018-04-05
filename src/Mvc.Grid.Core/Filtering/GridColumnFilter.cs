using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class GridColumnFilter<T, TValue> : IGridColumnFilter<T, TValue>
    {
        public String Name { get; set; }
        public Boolean? IsMulti { get; set; }
        public Boolean? IsEnabled { get; set; }

        public virtual String Operator
        {
            get
            {
                if (OperatorIsSet)
                    return OperatorValue;

                String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
                OperatorValue = Column.Grid.Query[prefix + Column.Name + "-op"].FirstOrDefault()?.ToLower();

                OperatorIsSet = true;

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
                if (FirstIsSet)
                    return FirstValue;

                FirstValue = GetFirstFilter();

                FirstIsSet = true;

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
                if (SecondIsSet)
                    return SecondValue;

                SecondValue = GetSecondFilter();

                SecondIsSet = true;

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
            Name = GetName(column);
            IsEnabled = column.Expression.Body is MemberExpression ? (Boolean?)null : false;
        }

        public IQueryable<T> Apply(IQueryable<T> items)
        {
            if (IsEnabled != true)
                return items;

            Expression expression = CreateFilterExpression();

            return expression == null ? items : items.Where(ToLambda(expression));
        }

        private IGridFilter GetFirstFilter()
        {
            String prefix = String.IsNullOrEmpty(Column.Grid.Name) ? "" : Column.Grid.Name + "-";
            String columnName = (prefix + Column.Name + "-").ToLower();
            String[] keys = GetFilterKeys(columnName);

            if (keys.Length == 0)
                return null;

            String filterType = keys[0].Substring(columnName.Length);
            String value = Column.Grid.Query[keys[0]][0];

            return GetFilter(filterType, value);
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

                String filterType = keys[0].Substring(columnName.Length);

                return GetFilter(filterType, values[1]);
            }

            String type = keys[1].Substring(columnName.Length);
            String value = Column.Grid.Query[keys[1]][0];

            return GetFilter(type, value);
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
        private String GetName(IGridColumn<T, TValue> column)
        {
            Type type = Nullable.GetUnderlyingType(column.Expression.ReturnType) ?? column.Expression.ReturnType;
            if (type.GetTypeInfo().IsEnum)
                return null;

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
                    return "number";
                case TypeCode.String:
                    return "text";
                case TypeCode.DateTime:
                    return "date";
                case TypeCode.Boolean:
                    return "boolean";
                default:
                    return null;
            }
        }
        private IGridFilter GetFilter(String filterType, String value)
        {
            Type valueType = Nullable.GetUnderlyingType(Column.Expression.ReturnType) ?? Column.Expression.ReturnType;

            IGridFilter filter = (Column.Grid.ViewContext?.HttpContext.RequestServices.GetService<IGridFilters>() ?? new GridFilters()).GetFilter(valueType, filterType);

            if (filter != null)
                filter.Value = value;

            return filter;
        }

        private Expression CreateFilterExpression()
        {
            Expression right = Second?.Apply(Column.Expression.Body);
            Expression left = First?.Apply(Column.Expression.Body);

            if (IsMulti == true && left != null && right != null)
            {
                if ("and".Equals(Operator, StringComparison.OrdinalIgnoreCase))
                    return Expression.AndAlso(left, right);
                else if ("or".Equals(Operator, StringComparison.OrdinalIgnoreCase))
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
