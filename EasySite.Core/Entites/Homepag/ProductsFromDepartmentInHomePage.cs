using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Homepage
{
    public class ProductsFromDepartmentInHomePage:BaseEntites
    {
        public int Index { get; set; }
        public string Titele { get; set; }
        public int FormatNumber { get; set; }
        public bool ShowProductesSale  { get; set; }
        public bool LatestProducts { get; set; }
        public bool ShowExpensiveProducts  { get; set; }
        public bool ShowCheapestProducts  { get; set; }


        public int HomepageId { get; set; }

        [InverseProperty("ProductsFromDepartmentInHomePage")]
        public Homepage Homepage { get; set; }

    }
}
