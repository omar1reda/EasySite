using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Repository.Spesifications
{
    public class Spesification<T> : ISpesification<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public List<string> IncludeStrings { get; set; } = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagination { get; set; }
        //public Expression<Func<T, object>> GroupBy { get; set; }

        public Spesification()
        {
            Includes = new List<Expression<Func<T, object>>>();
        }

        public Spesification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
            Includes = new List<Expression<Func<T, object>>>();
        }

        public Spesification(Expression<Func<T, bool>> criteria , string includeStrings)
        {
            Criteria = criteria;
            Includes = new List<Expression<Func<T, object>>>();
            IncludeStrings.Add(includeStrings);  
        }

        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        public void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }

        public void AddPagination( int pageIndex , int pageSize )
        {
            IsPagination = true;
            Skip = pageIndex;
            Take = pageSize;
        }


        //public void AddGroupBy(Expression<Func<T, object>> groupBy)
        //{
        //    GroupBy = groupBy;
        //}

    }
}
