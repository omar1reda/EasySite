using Core.Entites;
using Core.Entites.Homepage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Homepag
{
    public class DepartmentsInHomePage:BaseEntites
    {
        public int Index { get; set; }

        public int HomepageId { get; set; }

        //[InverseProperty("DepartmentsInHomePage")]
        public Homepage Homepage { get; set; }
    }
}
