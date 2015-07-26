using System;

namespace NonFactors.Mvc.Grid
{
    public class ByteFilter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            Byte number;
            if (Byte.TryParse(Value, out number))
                return number;

            return null;
        }
    }
}
