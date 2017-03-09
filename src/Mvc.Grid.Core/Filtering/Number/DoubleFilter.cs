using System;

namespace NonFactors.Mvc.Grid
{
    public class DoubleFilter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            if (Double.TryParse(Value, out Double number))
                return number;

            return null;
        }
    }
}
