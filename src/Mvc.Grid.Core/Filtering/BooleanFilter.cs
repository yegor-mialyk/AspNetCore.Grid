namespace NonFactors.Mvc.Grid;

public class BooleanFilter : AGridFilter
{
    protected override Expression? Apply(Expression expression, String? value, CultureInfo culture)
    {
        if (String.IsNullOrEmpty(value) && Nullable.GetUnderlyingType(expression.Type) == null)
            expression = Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));

        try
        {
            Object boolValue = TypeDescriptor.GetConverter(expression.Type).ConvertFrom(null, culture, value!)!;

            return Method switch
            {
                "not-equals" => Expression.NotEqual(expression, Expression.Constant(boolValue, expression.Type)),
                "equals" => Expression.Equal(expression, Expression.Constant(boolValue, expression.Type)),
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }
}
