using System;

namespace NonFactors.Mvc.Grid
{
    public class SByteFilter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            SByte number;
            if (SByte.TryParse(Value, out number))
                return number;

            return null;
        }
    }
}
