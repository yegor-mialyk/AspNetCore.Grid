using System;

namespace NonFactors.Mvc.Grid
{
    public class Int32Filter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            if (Int32.TryParse(Value, out Int32 number))
                return number;

            return null;
        }
    }
}
