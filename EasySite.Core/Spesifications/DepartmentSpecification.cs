using Core.Entites;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class DepartmentSpecification:Spesification<Department>
    {
        public DepartmentSpecification(string linkName, int sitId) :base(d=>d.SiteId == sitId && d.LinkName == linkName)
        {
            //AddOrderByDescending(p => p.index);

        }

        public DepartmentSpecification( int sitId) : base(d => d.SiteId == sitId)
        {
            AddOrderBy(p => p.index);
            Includes.Add(d => d.Products);

        }
        public DepartmentSpecification(string linkName) : base(d => d.LinkName == linkName)
        {



        }

        public DepartmentSpecification(int sitId , int id) : base(d => d.Id == id)
        {
            AddOrderBy(p => p.index);
            Includes.Add(d => d.Products);

        }
    }
}
