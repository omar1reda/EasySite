using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Order
{
    public class MissingOrders:BaseEntites
    {
        public int Phone { get; set; }
        public int Email { get; set; }
        public int Name { get; set; }
        public int Governorate { get; set; }
        public int Address { get; set; }
    }
}
