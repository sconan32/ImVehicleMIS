using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Socona.ImVehicle.Core.Interfaces;
using Socona.ImVehicle.Core.Specifications;

namespace Socona.ImVehicle.Infrastructure.Specifications
{
    public class OrSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _right = right;
            _left = left;
            foreach (var inc in _right.Includes)
            {
                Includes.Add(inc);
            }
            foreach (var inc in _left.Includes)
            {
                Includes.Add(inc);
            }
            foreach (var incStr in _right.IncludeStrings)
            {
                IncludeStrings.Add(incStr);
            }
            foreach (var incStr in _left.IncludeStrings)
            {
                IncludeStrings.Add(incStr);
            }
            if (left.Sort != null)
            {
                if (right.Sort != null)
                {
                    Sort = items => right.Sort(left.Sort(items));
                }
                else
                {
                    Sort = items => left.Sort(items);
                }

            }
            else if (right.Sort != null)
            {
                Sort = items => right.Sort(items);
            }

            if (left.PostProcess != null)
            {
                if (right.PostProcess != null)
                {
                    PostProcess = items => right.PostProcess(left.PostProcess(items));
                }
                else
                {
                    PostProcess = items => left.PostProcess(items);
                }
            }
            else if (right.PostProcess != null)
            {
                PostProcess = items => right.PostProcess(items);
            }
            Criteria = ToExpression();
        }

        protected Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.Criteria;
            Expression<Func<T, bool>> rightExpression = _right.Criteria;

            /*************NET35***************
            // need to detect whether they use the same
            // parameter instance; if not, they need fixing
            ParameterExpression param = leftExpression.Parameters[0];
            if (ReferenceEquals(param, rightExpression.Parameters[0]))
            {
                // simple version
                return Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(leftExpression.Body, rightExpression.Body), param);
            }
            // otherwise, keep expr1 "as is" and invoke expr2
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    leftExpression.Body,
                    Expression.Invoke(rightExpression, param)), param);
                    *******************/
            // NET40 VERSION
            //replace both parameter with a new defined parameter
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(leftExpression.Parameters[0], parameter);
            var left = leftVisitor.Visit(leftExpression.Body);

            var rightVisitor = new ReplaceExpressionVisitor(rightExpression.Parameters[0], parameter);
            var right = rightVisitor.Visit(rightExpression.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);

        }
        private class ReplaceExpressionVisitor
        : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }

}

