using System;

namespace NonFactors.Mvc.Grid
{
    public class SingleFilter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            Single number;
            if (Single.TryParse(Value, out number))
                return number;

            return null;
        }
    }
}
