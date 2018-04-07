using System;

namespace NonFactors.Mvc.Grid
{
    public interface IGridFilters
    {
        IGridFilter GetFilter(Type type, String method);

        void Register(Type type, String method, Type filter);
        void Unregister(Type type, String method);
    }
}
