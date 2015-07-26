namespace NonFactors.Mvc.Grid
{
    public static class MvcGrid
    {
        public static IGridFilters Filters { get; set; }

        static MvcGrid()
        {
            Filters = new GridFilters();
        }
    }
}
