using System;

namespace NonFactors.Mvc.Grid
{
    public class DecimalFilter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            Decimal number;
            if (Decimal.TryParse(Value, out number))
                return number;

            return null;
        }
    }
}
