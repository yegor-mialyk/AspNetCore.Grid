using System;

namespace NonFactors.Mvc.Grid
{
    public class GridConfig
    {
        public String Name { get; set; }
        public GridColumnConfig[] Columns { get; set; }

        public GridConfig()
        {
            Name = "";
            Columns = Array.Empty<GridColumnConfig>();
        }
    }
}
