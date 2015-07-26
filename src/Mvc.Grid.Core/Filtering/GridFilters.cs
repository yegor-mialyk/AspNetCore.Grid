using System;
using System.Collections.Generic;
using System.Linq;

namespace NonFactors.Mvc.Grid
{
    public class GridFilters : IGridFilters
    {
        public IDictionary<Type, IDictionary<String, Type>> Table
        {
            get;
            private set;
        }

        public GridFilters()
        {
            Table = new Dictionary<Type, IDictionary<String, Type>>();

            Register(typeof(SByte), "Equals", typeof(SByteFilter));
            Register(typeof(SByte), "LessThan", typeof(SByteFilter));
            Register(typeof(SByte), "GreaterThan", typeof(SByteFilter));
            Register(typeof(SByte), "LessThanOrEqual", typeof(SByteFilter));
            Register(typeof(SByte), "GreaterThanOrEqual", typeof(SByteFilter));

            Register(typeof(Byte), "Equals", typeof(ByteFilter));
            Register(typeof(Byte), "LessThan", typeof(ByteFilter));
            Register(typeof(Byte), "GreaterThan", typeof(ByteFilter));
            Register(typeof(Byte), "LessThanOrEqual", typeof(ByteFilter));
            Register(typeof(Byte), "GreaterThanOrEqual", typeof(ByteFilter));

            Register(typeof(Int16), "Equals", typeof(Int16Filter));
            Register(typeof(Int16), "LessThan", typeof(Int16Filter));
            Register(typeof(Int16), "GreaterThan", typeof(Int16Filter));
            Register(typeof(Int16), "LessThanOrEqual", typeof(Int16Filter));
            Register(typeof(Int16), "GreaterThanOrEqual", typeof(Int16Filter));

            Register(typeof(UInt16), "Equals", typeof(UInt16Filter));
            Register(typeof(UInt16), "LessThan", typeof(UInt16Filter));
            Register(typeof(UInt16), "GreaterThan", typeof(UInt16Filter));
            Register(typeof(UInt16), "LessThanOrEqual", typeof(UInt16Filter));
            Register(typeof(UInt16), "GreaterThanOrEqual", typeof(UInt16Filter));

            Register(typeof(Int32), "Equals", typeof(Int32Filter));
            Register(typeof(Int32), "LessThan", typeof(Int32Filter));
            Register(typeof(Int32), "GreaterThan", typeof(Int32Filter));
            Register(typeof(Int32), "LessThanOrEqual", typeof(Int32Filter));
            Register(typeof(Int32), "GreaterThanOrEqual", typeof(Int32Filter));

            Register(typeof(UInt32), "Equals", typeof(UInt32Filter));
            Register(typeof(UInt32), "LessThan", typeof(UInt32Filter));
            Register(typeof(UInt32), "GreaterThan", typeof(UInt32Filter));
            Register(typeof(UInt32), "LessThanOrEqual", typeof(UInt32Filter));
            Register(typeof(UInt32), "GreaterThanOrEqual", typeof(UInt32Filter));

            Register(typeof(Int64), "Equals", typeof(Int64Filter));
            Register(typeof(Int64), "LessThan", typeof(Int64Filter));
            Register(typeof(Int64), "GreaterThan", typeof(Int64Filter));
            Register(typeof(Int64), "LessThanOrEqual", typeof(Int64Filter));
            Register(typeof(Int64), "GreaterThanOrEqual", typeof(Int64Filter));

            Register(typeof(UInt64), "Equals", typeof(UInt64Filter));
            Register(typeof(UInt64), "LessThan", typeof(UInt64Filter));
            Register(typeof(UInt64), "GreaterThan", typeof(UInt64Filter));
            Register(typeof(UInt64), "LessThanOrEqual", typeof(UInt64Filter));
            Register(typeof(UInt64), "GreaterThanOrEqual", typeof(UInt64Filter));

            Register(typeof(Single), "Equals", typeof(SingleFilter));
            Register(typeof(Single), "LessThan", typeof(SingleFilter));
            Register(typeof(Single), "GreaterThan", typeof(SingleFilter));
            Register(typeof(Single), "LessThanOrEqual", typeof(SingleFilter));
            Register(typeof(Single), "GreaterThanOrEqual", typeof(SingleFilter));

            Register(typeof(Double), "Equals", typeof(DoubleFilter));
            Register(typeof(Double), "LessThan", typeof(DoubleFilter));
            Register(typeof(Double), "GreaterThan", typeof(DoubleFilter));
            Register(typeof(Double), "LessThanOrEqual", typeof(DoubleFilter));
            Register(typeof(Double), "GreaterThanOrEqual", typeof(DoubleFilter));

            Register(typeof(Decimal), "Equals", typeof(DecimalFilter));
            Register(typeof(Decimal), "LessThan", typeof(DecimalFilter));
            Register(typeof(Decimal), "GreaterThan", typeof(DecimalFilter));
            Register(typeof(Decimal), "LessThanOrEqual", typeof(DecimalFilter));
            Register(typeof(Decimal), "GreaterThanOrEqual", typeof(DecimalFilter));

            Register(typeof(DateTime), "Equals", typeof(DateTimeFilter));
            Register(typeof(DateTime), "LessThan", typeof(DateTimeFilter));
            Register(typeof(DateTime), "GreaterThan", typeof(DateTimeFilter));
            Register(typeof(DateTime), "LessThanOrEqual", typeof(DateTimeFilter));
            Register(typeof(DateTime), "GreaterThanOrEqual", typeof(DateTimeFilter));

            Register(typeof(Boolean), "Equals", typeof(BooleanFilter));

            Register(typeof(String), "Equals", typeof(StringEqualsFilter));
            Register(typeof(String), "Contains", typeof(StringContainsFilter));
            Register(typeof(String), "EndsWith", typeof(StringEndsWithFilter));
            Register(typeof(String), "StartsWith", typeof(StringStartsWithFilter));
        }

        public IGridColumnFilter<T> GetFilter<T>(IGridColumn<T> column)
        {
            String[] keys = column
                .Grid
                .Query
                .Keys
                .Where(key =>
                    (key ?? "").StartsWith(column.Grid.Name + "-" + column.Name + "-") &&
                    key != column.Grid.Name + "-" + column.Name + "-Op")
                .ToArray();

            GridColumnFilter<T> filter = new GridColumnFilter<T>();
            filter.Second = GetSecondFilter(column, keys);
            filter.First = GetFirstFilter(column, keys);
            filter.Operator = GetOperator(column);
            filter.Column = column;

            return filter;
        }

        public void Register(Type forType, String filterType, Type filter)
        {
            IDictionary<String, Type> typedFilters = new Dictionary<String, Type>();
            Type underlyingType = Nullable.GetUnderlyingType(forType) ?? forType;

            if (Table.ContainsKey(underlyingType))
                typedFilters = Table[underlyingType];
            else
                Table.Add(underlyingType, typedFilters);

            typedFilters.Add(filterType, filter);
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
        private IGridFilter GetSecondFilter<T>(IGridColumn<T> column, String[] keys)
        {
            if (column.IsMultiFilterable != true || keys.Length == 0)
                return null;

            if (keys.Length == 1)
            {
                IList<String> values = column.Grid.Query.GetValues(keys[0]);
                if (values.Count > 1)
                {
                    String keyType = keys[0].Substring((column.Grid.Name + "-" + column.Name + "-").Length);

                    return GetFilter(column, keyType, values[1]);
                }

                return null;
            }

            String type = keys[1].Substring((column.Grid.Name + "-" + column.Name + "-").Length);
            String value = column.Grid.Query.GetValues(keys[1])[0];

            return GetFilter(column, type, value);
        }
        private IGridFilter GetFirstFilter<T>(IGridColumn<T> column, String[] keys)
        {
            if (keys.Length == 0) return null;

            String type = keys[0].Substring((column.Grid.Name + "-" + column.Name + "-").Length);
            String value = column.Grid.Query.GetValues(keys[0])[0];

            return GetFilter(column, type, value);
        }
        private String GetOperator<T>(IGridColumn<T> column)
        {
            IList<String> values = column.Grid.Query.GetValues(column.Grid.Name + "-" + column.Name + "-Op");
            if (column.IsMultiFilterable != true || values == null) return null;

            return values[0];
        }
    }
}
