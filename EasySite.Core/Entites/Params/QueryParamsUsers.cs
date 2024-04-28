using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Params
{
    public class QueryParamsUsers
    {
        public string? Sort { get; set; }

        public string? searchBy_UserName { get; set; }
        public string? searchBy_Email { get; set; }
        public string? searchBy_SiteLink { get; set; }
        public string? searchBy_UserType { get; set; }
        public string? searchBy_Id { get; set; }

        public int PageIndex { get; set; } = 1;

        private int pageSize = 25;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 100 ? value = 100 : value; }
        }
        public bool IsPagination { get; set; } = true;
    }
}
