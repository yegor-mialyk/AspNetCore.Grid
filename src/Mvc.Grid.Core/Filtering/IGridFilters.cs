using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridFilters
    {
        IGridColumnFilter<T> GetFilter<T>(IGridColumn<T> column);

        void Register(Type forType, String filterType, Type filter);
        void Unregister(Type forType, String filterType);
    }
}
