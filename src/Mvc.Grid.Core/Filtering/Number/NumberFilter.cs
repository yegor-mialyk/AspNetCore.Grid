using System;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class NumberFilter<T> : BaseGridFilter
    {
        public override Expression Apply(Expression expression)
        {
            Object value = GetNumericValue(Value);
            if (value == null)
                return null;

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

        private Object GetNumericValue(String value)
        {
            Type type = typeof(T);

            if (type == typeof(Decimal) && Decimal.TryParse(value, out Decimal decimalNumber))
                return decimalNumber;
            else if (type == typeof(Double) && Double.TryParse(value, out Double doubleNumber))
                return doubleNumber;
            else if (type == typeof(Single) && Single.TryParse(value, out Single singleNumber))
                return singleNumber;
            else if (type == typeof(Int64) && Int64.TryParse(value, out Int64 int64Number))
                return int64Number;
            else if (type == typeof(UInt64) && UInt64.TryParse(value, out UInt64 uint64Number))
                return uint64Number;
            else if (type == typeof(Int32) && Int32.TryParse(value, out Int32 int32Number))
                return int32Number;
            else if (type == typeof(UInt32) && UInt32.TryParse(value, out UInt32 uint32Number))
                return uint32Number;
            else if (type == typeof(Int16) && Int16.TryParse(value, out Int16 int16Number))
                return int16Number;
            else if (type == typeof(UInt16) && UInt16.TryParse(value, out UInt16 uint16Number))
                return uint16Number;
            else if (type == typeof(SByte) && SByte.TryParse(value, out SByte sbyteNumber))
                return sbyteNumber;
            else if (type == typeof(Byte) && Byte.TryParse(value, out Byte byteNumber))
                return byteNumber;

            return null;
        }
    }
}
