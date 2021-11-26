namespace NonFactors.Mvc.Grid;

public static class IQueryableFilterExtensions
{
    public static IQueryable<T> Where<T, V>(this IQueryable<T> items, Expression<Func<T, V>> expression, IGridFilter filter)
    {
        return items.Where(Expression.Lambda<Func<T, Boolean>>(filter.Apply(expression.Body)!, expression.Parameters[0]));
    }
}
