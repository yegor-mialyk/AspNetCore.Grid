using System;

namespace NonFactors.Mvc.Grid
{
    public class UInt32Filter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            if (UInt32.TryParse(Value, out UInt32 number))
                return number;

            return null;
        }
    }
}
