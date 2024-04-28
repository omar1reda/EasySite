using EasySite.DTOs.HomePage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Homepage
{
    public class ProductsInHomePage:BaseEntites
    {
        public int Index { get; set; }
        public string Titele { get; set; }
        public int FormatNumber { get; set; }
        public SortBy sortBy { get; set; }

        public int DepartmentId { get; set; }
        //public Department Department { get; set; }
        public int HomepageId { get; set; }

        //[InverseProperty("ProductsFromDepartmentInHomePage")]
        public Homepage Homepage { get; set; }

    }
}
