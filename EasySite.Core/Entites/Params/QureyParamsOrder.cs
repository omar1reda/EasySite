using EasySite.Core.Entites.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Params
{
    public class QureyParamsOrder
    {
        public string? Sort { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumper { get; set; }
        public string? CustomerAddress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? StatusOrder { get; set; }

        public int? ProductId { get; set; }


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
