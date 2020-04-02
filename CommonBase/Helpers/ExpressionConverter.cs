//@QnSCodeCopy
//MdStart
using System;
using System.Linq.Expressions;

namespace CommonBase.Helpers
{
    public class ExpressionConverter : ExpressionVisitor
    {
        private Expression _source;
        private Expression _dest;

        public ExpressionConverter(Expression source, Expression dest)
        {
            _source = source;
            _dest = dest;
        }

        public static Expression<Func<TConParam, TConReturn>> ConvertToObject<TParm, TReturn, TConParam, TConReturn>(Expression<Func<TParm, TReturn>> input)
        {
            var parm = Expression.Parameter(typeof(TConParam));
            var castParm = Expression.Convert(parm, typeof(TParm));
            var body = ReplaceExpression(input.Body, input.Parameters[0], castParm);
            body = Expression.Convert(body, typeof(TConReturn));
            return Expression.Lambda<Func<TConParam, TConReturn>>(body, parm);
        }

        public static Expression ReplaceExpression(Expression body, Expression source, Expression dest)
        {
            var replacer = new ExpressionConverter(source, dest);
            return replacer.Visit(body);
        }

        public override Expression Visit(Expression node)
        {
            if (node == _source)
                return _dest;

            return base.Visit(node);
        }
    }
}
//MdEnd
