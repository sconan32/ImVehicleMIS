using Socona.ImVehicle.Core.Interfaces;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Socona.ImVehicle.Core.Specifications
{
    public  class Specification<T> : ISpecification<T>
    {
        public Specification(Expression<Func<T, bool>> criteria = null)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; protected set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }
    }
}
