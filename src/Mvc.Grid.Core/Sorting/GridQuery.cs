using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public static class GridQuery
    {
        public static Boolean IsOrdered(IQueryable models)
        {
            GridExpressionVisitor expression = new GridExpressionVisitor();
            expression.Visit(models.Expression);

            return expression.Ordered;
        }

        private class GridExpressionVisitor : ExpressionVisitor
        {
            public Boolean Ordered { get; set; }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.DeclaringType != typeof(Queryable))
                    return base.VisitMethodCall(node);

                if (!node.Method.Name.StartsWith("OrderBy"))
                    return base.VisitMethodCall(node);

                Ordered = true;

                return node;
            }
        }
    }
}
