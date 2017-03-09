using System;

namespace NonFactors.Mvc.Grid
{
    public class Int64Filter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            if (Int64.TryParse(Value, out Int64 number))
                return number;

            return null;
        }
    }
}
