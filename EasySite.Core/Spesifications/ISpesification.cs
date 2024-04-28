using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Repository.Spesifications
{
    public interface ISpesification<T> where T : class
    {
        public Expression<Func<T,bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public List<string> IncludeStrings { get; set; }

        // => OrderBy 
        public Expression<Func<T, Object>> OrderBy { get; set; } 

        // => OrderByDescending
        public Expression<Func<T, Object>> OrderByDescending { get; set; }

        // => Pagination 
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPagination { get; set; }

        ////==> GroupBy
        //public Expression<Func<T, object>> GroupBy { get; set; }
    }
}
