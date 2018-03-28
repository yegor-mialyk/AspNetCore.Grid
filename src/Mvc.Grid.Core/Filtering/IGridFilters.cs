using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridFilters
    {
        IGridColumnFilter<T, TValue> GetFilter<T, TValue>(IGridColumn<T, TValue> column);

        void Register(Type forType, String filterType, Type filter);
        void Unregister(Type forType, String filterType);
    }
}
