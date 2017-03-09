using System;

namespace NonFactors.Mvc.Grid
{
    public class SByteFilter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            if (SByte.TryParse(Value, out SByte number))
                return number;

            return null;
        }
    }
}
