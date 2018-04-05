using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridFilters
    {
        IGridFilter GetFilter(Type forType, String filterType);

        void Register(Type forType, String filterType, Type filter);
        void Unregister(Type forType, String filterType);
    }
}
