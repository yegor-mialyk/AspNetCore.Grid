using System;

namespace NonFactors.Mvc.Grid
{
    public class UInt32Filter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            UInt32 number;
            if (UInt32.TryParse(Value, out number))
                return number;

            return null;
        }
    }
}
