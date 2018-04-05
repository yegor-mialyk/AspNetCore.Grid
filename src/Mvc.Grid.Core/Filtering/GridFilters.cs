using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public class GridFilters : IGridFilters
    {
        private IDictionary<Type, IDictionary<String, Type>> Table
        {
            get;
        }

        public GridFilters()
        {
            Table = new Dictionary<Type, IDictionary<String, Type>>();

            Register(typeof(SByte), "equals", typeof(SByteFilter));
            Register(typeof(SByte), "not-equals", typeof(SByteFilter));
            Register(typeof(SByte), "less-than", typeof(SByteFilter));
            Register(typeof(SByte), "greater-than", typeof(SByteFilter));
            Register(typeof(SByte), "less-than-or-equal", typeof(SByteFilter));
            Register(typeof(SByte), "greater-than-or-equal", typeof(SByteFilter));

            Register(typeof(Byte), "equals", typeof(ByteFilter));
            Register(typeof(Byte), "not-equals", typeof(ByteFilter));
            Register(typeof(Byte), "less-than", typeof(ByteFilter));
            Register(typeof(Byte), "greater-than", typeof(ByteFilter));
            Register(typeof(Byte), "less-than-or-equal", typeof(ByteFilter));
            Register(typeof(Byte), "greater-than-or-equal", typeof(ByteFilter));

            Register(typeof(Int16), "equals", typeof(Int16Filter));
            Register(typeof(Int16), "not-equals", typeof(Int16Filter));
            Register(typeof(Int16), "less-than", typeof(Int16Filter));
            Register(typeof(Int16), "greater-than", typeof(Int16Filter));
            Register(typeof(Int16), "less-than-or-equal", typeof(Int16Filter));
            Register(typeof(Int16), "greater-than-or-equal", typeof(Int16Filter));

            Register(typeof(UInt16), "equals", typeof(UInt16Filter));
            Register(typeof(UInt16), "not-equals", typeof(UInt16Filter));
            Register(typeof(UInt16), "less-than", typeof(UInt16Filter));
            Register(typeof(UInt16), "greater-than", typeof(UInt16Filter));
            Register(typeof(UInt16), "less-than-or-equal", typeof(UInt16Filter));
            Register(typeof(UInt16), "greater-than-or-equal", typeof(UInt16Filter));

            Register(typeof(Int32), "equals", typeof(Int32Filter));
            Register(typeof(Int32), "not-equals", typeof(Int32Filter));
            Register(typeof(Int32), "less-than", typeof(Int32Filter));
            Register(typeof(Int32), "greater-than", typeof(Int32Filter));
            Register(typeof(Int32), "less-than-or-equal", typeof(Int32Filter));
            Register(typeof(Int32), "greater-than-or-equal", typeof(Int32Filter));

            Register(typeof(UInt32), "equals", typeof(UInt32Filter));
            Register(typeof(UInt32), "not-equals", typeof(UInt32Filter));
            Register(typeof(UInt32), "less-than", typeof(UInt32Filter));
            Register(typeof(UInt32), "greater-than", typeof(UInt32Filter));
            Register(typeof(UInt32), "less-than-or-equal", typeof(UInt32Filter));
            Register(typeof(UInt32), "greater-than-or-equal", typeof(UInt32Filter));

            Register(typeof(Int64), "equals", typeof(Int64Filter));
            Register(typeof(Int64), "not-equals", typeof(Int64Filter));
            Register(typeof(Int64), "less-than", typeof(Int64Filter));
            Register(typeof(Int64), "greater-than", typeof(Int64Filter));
            Register(typeof(Int64), "less-than-or-equal", typeof(Int64Filter));
            Register(typeof(Int64), "greater-than-or-equal", typeof(Int64Filter));

            Register(typeof(UInt64), "equals", typeof(UInt64Filter));
            Register(typeof(UInt64), "not-equals", typeof(UInt64Filter));
            Register(typeof(UInt64), "less-than", typeof(UInt64Filter));
            Register(typeof(UInt64), "greater-than", typeof(UInt64Filter));
            Register(typeof(UInt64), "less-than-or-equal", typeof(UInt64Filter));
            Register(typeof(UInt64), "greater-than-or-equal", typeof(UInt64Filter));

            Register(typeof(Single), "equals", typeof(SingleFilter));
            Register(typeof(Single), "not-equals", typeof(SingleFilter));
            Register(typeof(Single), "less-than", typeof(SingleFilter));
            Register(typeof(Single), "greater-than", typeof(SingleFilter));
            Register(typeof(Single), "less-than-or-equal", typeof(SingleFilter));
            Register(typeof(Single), "greater-than-or-equal", typeof(SingleFilter));

            Register(typeof(Double), "equals", typeof(DoubleFilter));
            Register(typeof(Double), "not-equals", typeof(DoubleFilter));
            Register(typeof(Double), "less-than", typeof(DoubleFilter));
            Register(typeof(Double), "greater-than", typeof(DoubleFilter));
            Register(typeof(Double), "less-than-or-equal", typeof(DoubleFilter));
            Register(typeof(Double), "greater-than-or-equal", typeof(DoubleFilter));

            Register(typeof(Decimal), "equals", typeof(DecimalFilter));
            Register(typeof(Decimal), "not-equals", typeof(DecimalFilter));
            Register(typeof(Decimal), "less-than", typeof(DecimalFilter));
            Register(typeof(Decimal), "greater-than", typeof(DecimalFilter));
            Register(typeof(Decimal), "less-than-or-equal", typeof(DecimalFilter));
            Register(typeof(Decimal), "greater-than-or-equal", typeof(DecimalFilter));

            Register(typeof(DateTime), "equals", typeof(DateTimeFilter));
            Register(typeof(DateTime), "not-equals", typeof(DateTimeFilter));
            Register(typeof(DateTime), "earlier-than", typeof(DateTimeFilter));
            Register(typeof(DateTime), "later-than", typeof(DateTimeFilter));
            Register(typeof(DateTime), "earlier-than-or-equal", typeof(DateTimeFilter));
            Register(typeof(DateTime), "later-than-or-equal", typeof(DateTimeFilter));

            Register(typeof(Boolean), "equals", typeof(BooleanFilter));

            Register(typeof(String), "equals", typeof(StringEqualsFilter));
            Register(typeof(String), "not-equals", typeof(StringNotEqualsFilter));
            Register(typeof(String), "contains", typeof(StringContainsFilter));
            Register(typeof(String), "ends-with", typeof(StringEndsWithFilter));
            Register(typeof(String), "starts-with", typeof(StringStartsWithFilter));
        }

        public IGridFilter GetFilter(Type forType, String filterType)
        {
            if (!Table.ContainsKey(forType))
                return null;

            if (!Table[forType].TryGetValue(filterType, out Type type))
                return null;

            IGridFilter filter = (IGridFilter)Activator.CreateInstance(type);
            filter.Type = filterType.ToLower();

            return filter;
        }

        public void Register(Type forType, String filterType, Type filter)
        {
            IDictionary<String, Type> filters = new Dictionary<String, Type>(StringComparer.OrdinalIgnoreCase);
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
    }
}
