using System;

namespace NonFactors.Mvc.Grid
{
    public class DoubleFilter : NumberFilter
    {
        public override Object GetNumericValue()
        {
            Double number;
            if (Double.TryParse(Value, out number))
                return number;

            return null;
        }
    }
}
