using Otlob.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        // GetAll endpoint (We Don't Need AnyWhere Condition)
        public BaseSpecification() { }

        // GetByID endpoint (We Need Where Condition)
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        // Protected method so whoever inherits the class can push all of its includes into the list
        protected void AddIncludes(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression); // Add() => Adds the given object to the end of this list
        }
        protected void AddOrderBy(Expression<Func<T,object>> expression)
        { 
        OrderBy = expression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> expression)
        { 
        OrderByDescending = expression; 
        }
        protected void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
