using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace NonFactors.Mvc.Grid
{
    public interface IGridFilters
    {
        Func<String> BooleanTrueOptionText { get; set; }
        Func<String> BooleanFalseOptionText { get; set; }
        Func<String> BooleanEmptyOptionText { get; set; }

        IGridFilter GetFilter(Type type, String method);
        IEnumerable<SelectListItem> GetFilterOptions<T, TValue>(IGridColumn<T, TValue> column);

        void Register(Type type, String method, Type filter);
        void Unregister(Type type, String method);
    }
}
