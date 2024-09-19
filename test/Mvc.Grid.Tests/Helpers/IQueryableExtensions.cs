namespace NonFactors.Mvc.Grid;

public static class IQueryableExtensions
{
    public static IQueryable<T> Where<T, V>(this IQueryable<T> items, Expression<Func<T, V>> expression, IGridFilter filter)
    {
        return items.Where(Expression.Lambda<Func<T, Boolean>>(filter.Apply(expression.Body, CultureInfo.CurrentCulture)!, expression.Parameters[0]));
    }
}
