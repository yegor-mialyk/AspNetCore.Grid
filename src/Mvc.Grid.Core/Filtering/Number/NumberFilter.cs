using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class NumberFilter<T> : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            Object value = null;
            if (String.IsNullOrEmpty(Value))
            {
                if (Nullable.GetUnderlyingType(expression.Type) == null)
                    expression = Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));
            }
            else if ((value = GetTypedValue()) == null)
            {
                return null;
            }

            switch (Method)
            {
                case "equals":
                    return Expression.Equal(expression, Expression.Constant(value, expression.Type));
                case "not-equals":
                    return Expression.NotEqual(expression, Expression.Constant(value, expression.Type));
                case "less-than":
                    return Expression.LessThan(expression, Expression.Constant(value, expression.Type));
                case "greater-than":
                    return Expression.GreaterThan(expression, Expression.Constant(value, expression.Type));
                case "less-than-or-equal":
                    return Expression.LessThanOrEqual(expression, Expression.Constant(value, expression.Type));
                case "greater-than-or-equal":
                    return Expression.GreaterThanOrEqual(expression, Expression.Constant(value, expression.Type));
                default:
                    return null;
            }
        }

        private Object GetTypedValue()
        {
            Type type = typeof(T);

            if (type == typeof(Decimal) && Decimal.TryParse(Value, out Decimal decimalNumber))
                return decimalNumber;
            if (type == typeof(Double) && Double.TryParse(Value, out Double doubleNumber))
                return doubleNumber;
            if (type == typeof(Single) && Single.TryParse(Value, out Single singleNumber))
                return singleNumber;
            if (type == typeof(Int64) && Int64.TryParse(Value, out Int64 int64Number))
                return int64Number;
            if (type == typeof(UInt64) && UInt64.TryParse(Value, out UInt64 uint64Number))
                return uint64Number;
            if (type == typeof(Int32) && Int32.TryParse(Value, out Int32 int32Number))
                return int32Number;
            if (type == typeof(UInt32) && UInt32.TryParse(Value, out UInt32 uint32Number))
                return uint32Number;
            if (type == typeof(Int16) && Int16.TryParse(Value, out Int16 int16Number))
                return int16Number;
            if (type == typeof(UInt16) && UInt16.TryParse(Value, out UInt16 uint16Number))
                return uint16Number;
            if (type == typeof(SByte) && SByte.TryParse(Value, out SByte sbyteNumber))
                return sbyteNumber;
            if (type == typeof(Byte) && Byte.TryParse(Value, out Byte byteNumber))
                return byteNumber;

            return null;
        }
    }
}
