using EasySite.Core.Entites.Homepag;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Homepage
{
    public class Homepage : BaseEntites
    {
        public bool ShowInHedear { get; set; }
        public bool IsActive { get; set; }

        public int SiteId { get; set; }

        [InverseProperty("Homepage")]
        public Site Site { get; set; }


        [InverseProperty("Homepage")]
        public ICollection<ProductsInHomePage> ProductsInHomePage { get; set; } = new HashSet<ProductsInHomePage>();


        [InverseProperty("Homepage")]
        public ICollection<DepartmentsInHomePage> DepartmentsInHomePages { get; set; } = new HashSet<DepartmentsInHomePage>();


        [InverseProperty("Homepage")]
        public ICollection<Slider> Sliders { get; set; } = new HashSet<Slider>();

    }
}
