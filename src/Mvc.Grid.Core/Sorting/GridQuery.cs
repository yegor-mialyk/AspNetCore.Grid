using System;
using System.Linq;
using System.Linq.Expressions;

namespace NonFactors.Mvc.Grid
{
    public class GridQuery : ExpressionVisitor
    {
        private Boolean Ordered { get; set; }

        private GridQuery()
        {
        }

        public static Boolean IsOrdered(IQueryable models)
        {
            GridQuery expression = new GridQuery();
            expression.Visit(models.Expression);

            return expression.Ordered;
        }

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
