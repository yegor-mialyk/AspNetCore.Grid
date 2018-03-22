using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public class GridFilters : IGridFilters
    {
        public IDictionary<Type, IDictionary<String, Type>> Table
        {
            get;
        }

        public GridFilters()
        {
            Table = new Dictionary<Type, IDictionary<String, Type>>();

            Register(typeof(SByte), "Equals", typeof(SByteFilter));
            Register(typeof(SByte), "NotEquals", typeof(SByteFilter));
            Register(typeof(SByte), "LessThan", typeof(SByteFilter));
            Register(typeof(SByte), "GreaterThan", typeof(SByteFilter));
            Register(typeof(SByte), "LessThanOrEqual", typeof(SByteFilter));
            Register(typeof(SByte), "GreaterThanOrEqual", typeof(SByteFilter));

            Register(typeof(Byte), "Equals", typeof(ByteFilter));
            Register(typeof(Byte), "NotEquals", typeof(ByteFilter));
            Register(typeof(Byte), "LessThan", typeof(ByteFilter));
            Register(typeof(Byte), "GreaterThan", typeof(ByteFilter));
            Register(typeof(Byte), "LessThanOrEqual", typeof(ByteFilter));
            Register(typeof(Byte), "GreaterThanOrEqual", typeof(ByteFilter));

            Register(typeof(Int16), "Equals", typeof(Int16Filter));
            Register(typeof(Int16), "NotEquals", typeof(Int16Filter));
            Register(typeof(Int16), "LessThan", typeof(Int16Filter));
            Register(typeof(Int16), "GreaterThan", typeof(Int16Filter));
            Register(typeof(Int16), "LessThanOrEqual", typeof(Int16Filter));
            Register(typeof(Int16), "GreaterThanOrEqual", typeof(Int16Filter));

            Register(typeof(UInt16), "Equals", typeof(UInt16Filter));
            Register(typeof(UInt16), "NotEquals", typeof(UInt16Filter));
            Register(typeof(UInt16), "LessThan", typeof(UInt16Filter));
            Register(typeof(UInt16), "GreaterThan", typeof(UInt16Filter));
            Register(typeof(UInt16), "LessThanOrEqual", typeof(UInt16Filter));
            Register(typeof(UInt16), "GreaterThanOrEqual", typeof(UInt16Filter));

            Register(typeof(Int32), "Equals", typeof(Int32Filter));
            Register(typeof(Int32), "NotEquals", typeof(Int32Filter));
            Register(typeof(Int32), "LessThan", typeof(Int32Filter));
            Register(typeof(Int32), "GreaterThan", typeof(Int32Filter));
            Register(typeof(Int32), "LessThanOrEqual", typeof(Int32Filter));
            Register(typeof(Int32), "GreaterThanOrEqual", typeof(Int32Filter));

            Register(typeof(UInt32), "Equals", typeof(UInt32Filter));
            Register(typeof(UInt32), "NotEquals", typeof(UInt32Filter));
            Register(typeof(UInt32), "LessThan", typeof(UInt32Filter));
            Register(typeof(UInt32), "GreaterThan", typeof(UInt32Filter));
            Register(typeof(UInt32), "LessThanOrEqual", typeof(UInt32Filter));
            Register(typeof(UInt32), "GreaterThanOrEqual", typeof(UInt32Filter));

            Register(typeof(Int64), "Equals", typeof(Int64Filter));
            Register(typeof(Int64), "NotEquals", typeof(Int64Filter));
            Register(typeof(Int64), "LessThan", typeof(Int64Filter));
            Register(typeof(Int64), "GreaterThan", typeof(Int64Filter));
            Register(typeof(Int64), "LessThanOrEqual", typeof(Int64Filter));
            Register(typeof(Int64), "GreaterThanOrEqual", typeof(Int64Filter));

            Register(typeof(UInt64), "Equals", typeof(UInt64Filter));
            Register(typeof(UInt64), "NotEquals", typeof(UInt64Filter));
            Register(typeof(UInt64), "LessThan", typeof(UInt64Filter));
            Register(typeof(UInt64), "GreaterThan", typeof(UInt64Filter));
            Register(typeof(UInt64), "LessThanOrEqual", typeof(UInt64Filter));
            Register(typeof(UInt64), "GreaterThanOrEqual", typeof(UInt64Filter));

            Register(typeof(Single), "Equals", typeof(SingleFilter));
            Register(typeof(Single), "NotEquals", typeof(SingleFilter));
            Register(typeof(Single), "LessThan", typeof(SingleFilter));
            Register(typeof(Single), "GreaterThan", typeof(SingleFilter));
            Register(typeof(Single), "LessThanOrEqual", typeof(SingleFilter));
            Register(typeof(Single), "GreaterThanOrEqual", typeof(SingleFilter));

            Register(typeof(Double), "Equals", typeof(DoubleFilter));
            Register(typeof(Double), "NotEquals", typeof(DoubleFilter));
            Register(typeof(Double), "LessThan", typeof(DoubleFilter));
            Register(typeof(Double), "GreaterThan", typeof(DoubleFilter));
            Register(typeof(Double), "LessThanOrEqual", typeof(DoubleFilter));
            Register(typeof(Double), "GreaterThanOrEqual", typeof(DoubleFilter));

            Register(typeof(Decimal), "Equals", typeof(DecimalFilter));
            Register(typeof(Decimal), "NotEquals", typeof(DecimalFilter));
            Register(typeof(Decimal), "LessThan", typeof(DecimalFilter));
            Register(typeof(Decimal), "GreaterThan", typeof(DecimalFilter));
            Register(typeof(Decimal), "LessThanOrEqual", typeof(DecimalFilter));
            Register(typeof(Decimal), "GreaterThanOrEqual", typeof(DecimalFilter));

            Register(typeof(DateTime), "Equals", typeof(DateTimeFilter));
            Register(typeof(DateTime), "NotEquals", typeof(DateTimeFilter));
            Register(typeof(DateTime), "EarlierThan", typeof(DateTimeFilter));
            Register(typeof(DateTime), "LaterThan", typeof(DateTimeFilter));
            Register(typeof(DateTime), "EarlierThanOrEqual", typeof(DateTimeFilter));
            Register(typeof(DateTime), "LaterThanOrEqual", typeof(DateTimeFilter));

            Register(typeof(Boolean), "Equals", typeof(BooleanFilter));

            Register(typeof(String), "Equals", typeof(StringEqualsFilter));
            Register(typeof(String), "NotEquals", typeof(StringNotEqualsFilter));
            Register(typeof(String), "Contains", typeof(StringContainsFilter));
            Register(typeof(String), "EndsWith", typeof(StringEndsWithFilter));
            Register(typeof(String), "StartsWith", typeof(StringStartsWithFilter));
        }

        public IGridColumnFilter<T> GetFilter<T>(IGridColumn<T> column)
        {
            String prefix = String.IsNullOrEmpty(column.Grid.Name) ? "" : column.Grid.Name + "-";
            String[] keys = column
                .Grid
                .Query
                .Keys
                .Where(key =>
                    (key ?? "").StartsWith(prefix + column.Name + "-") &&
                    key != prefix + column.Name + "-Op")
                .ToArray();

            GridColumnFilter<T> filter = new GridColumnFilter<T>(column);
            filter.Second = GetSecondFilter(prefix, column, keys);
            filter.First = GetFirstFilter(prefix, column, keys);
            filter.Operator = GetOperator(prefix, column);
            filter.Name = GetFilterName(column);

            return filter;
        }

        public void Register(Type forType, String filterType, Type filter)
        {
            IDictionary<String, Type> filters = new Dictionary<String, Type>();
            Type underlyingType = Nullable.GetUnderlyingType(forType) ?? forType;

            if (Table.ContainsKey(underlyingType))
                filters = Table[underlyingType];
            else
                Table[underlyingType] = filters;

            filters[filterType] = filter;
        }
        public void Unregister(Type forType, String filterType)
        {
            if (Table.ContainsKey(forType))
                Table[forType].Remove(filterType);
        }

        private IGridFilter GetFilter<T>(IGridColumn<T> column, String type, String value)
        {
            Type valueType = Nullable.GetUnderlyingType(column.Expression.ReturnType) ?? column.Expression.ReturnType;
            if (!Table.ContainsKey(valueType))
                return null;

            IDictionary<String, Type> typedFilters = Table[valueType];
            if (!typedFilters.ContainsKey(type))
                return null;

            IGridFilter filter = (IGridFilter)Activator.CreateInstance(typedFilters[type]);
            filter.Value = value;
            filter.Type = type;

            return filter;
        }
        private IGridFilter GetSecondFilter<T>(String prefix, IGridColumn<T> column, String[] keys)
        {
            if (keys.Length == 0)
                return null;

            if (keys.Length == 1)
            {
                StringValues values = column.Grid.Query[keys[0]];
                if (values.Count < 2)
                    return null;

                String keyType = keys[0].Substring((prefix + column.Name + "-").Length);

                return GetFilter(column, keyType, values[1]);
            }

            String type = keys[1].Substring((prefix + column.Name + "-").Length);
            String value = column.Grid.Query[keys[1]][0];

            return GetFilter(column, type, value);
        }
        private IGridFilter GetFirstFilter<T>(String prefix, IGridColumn<T> column, String[] keys)
        {
            if (keys.Length == 0)
                return null;

            String type = keys[0].Substring((prefix + column.Name + "-").Length);
            String value = column.Grid.Query[keys[0]][0];

            return GetFilter(column, type, value);
        }
        private String GetOperator<T>(String prefix, IGridColumn<T> column)
        {
            return column.Grid.Query[prefix + column.Name + "-Op"].FirstOrDefault();
        }


        private String GetFilterName<T>(IGridColumn<T> column)
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
