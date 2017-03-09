using System;

namespace NonFactors.Mvc.Grid
{
    public class UInt16Filter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            if (UInt16.TryParse(Value, out UInt16 number))
                return number;

            return null;
        }
    }
}
