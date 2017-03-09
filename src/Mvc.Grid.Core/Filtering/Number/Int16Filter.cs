using System;

namespace NonFactors.Mvc.Grid
{
    public class Int16Filter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            if (Int16.TryParse(Value, out Int16 number))
                return number;

            return null;
        }
    }
}
