using Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    public class Department:BaseEntites
    {
        public string Name { get; set; }
        public int index { get; set; }
        public string LinkName { get; set; }
        public bool ShowInHedar  { get; set; }
        public string Image { get; set; }

        public int SiteId { get; set; }

        //[InverseProperty("Department")]
        public Site Site { get; set; }


        //[InverseProperty("Department")]
        public ICollection<Product.Product> Products { get; set; } = new HashSet<Product.Product>();

    }
}
