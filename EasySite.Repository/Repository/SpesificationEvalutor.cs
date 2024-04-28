using Core.Entites;
using EasySite.Repository.Spesifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EasySite.Repository.Repository
{
    public static class SpesificationEvalutor<T> where T : class
    {
        public static IQueryable<T> GetQuery( IQueryable<T> InputQuery , ISpesification<T> spesification)
        {

            var Query = InputQuery;

            if (spesification.Criteria != null)
            {
                Query = Query.Where(spesification.Criteria);
            }

            if(spesification.OrderBy != null)
            {
                Query = Query.OrderBy(spesification.OrderBy);
            }
            if (spesification.OrderByDescending != null)
            {
                Query = Query.OrderByDescending(spesification.OrderByDescending);
            }

            if (spesification.IsPagination)
            {
                Query= Query.Skip(spesification.Skip).Take(spesification.Take);
            }

            //if(spesification.GroupBy != null)
            //{
            //    Query = Query.GroupBy(spesification.Includes);
            //}
 
            Query = spesification.Includes.Aggregate(Query , (CurrentQuery, IncludesExpretion) => CurrentQuery.Include(IncludesExpretion));
            Query = spesification.IncludeStrings.Aggregate(Query, (CurrentQuery, IncludesExpretion) => CurrentQuery.Include(IncludesExpretion));


            


            return  Query;

        }
    }
}
