using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NonFactors.Mvc.Grid
{
    public abstract class StringFilter : GridFilter
    {
        protected static Expression Null { get; }
        protected static Expression Empty { get; }
        protected static MethodInfo ToLower { get; }
        protected static MethodInfo ToUpper { get; }
        protected static MethodInfo Contains { get; }
        protected static MethodInfo EndsWith { get; }
        protected static MethodInfo StartsWith { get; }

        static StringFilter()
        {
            Empty = Expression.Constant("");
            Null = Expression.Constant(null, typeof(String));
            ToLower = typeof(String).GetMethod(nameof(String.ToLower), Array.Empty<Type>())!;
            ToUpper = typeof(String).GetMethod(nameof(String.ToUpper), Array.Empty<Type>())!;
            Contains = typeof(String).GetMethod(nameof(String.Contains), new[] { typeof(String) })!;
            EndsWith = typeof(String).GetMethod(nameof(String.EndsWith), new[] { typeof(String) })!;
            StartsWith = typeof(String).GetMethod(nameof(String.StartsWith), new[] { typeof(String) })!;
        }

        protected Expression ConvertCase(String? value)
        {
            return Expression.Constant(Case switch
            {
                GridFilterCase.Upper => value?.ToUpper(),
                GridFilterCase.Lower => value?.ToLower(),
                _ => value
            });
        }
        protected Expression ConvertCase(Expression expression)
        {
            return Case switch
            {
                GridFilterCase.Upper => Expression.Call(expression, ToUpper),
                GridFilterCase.Lower => Expression.Call(expression, ToLower),
                _ => expression
            };
        }
    }
}
