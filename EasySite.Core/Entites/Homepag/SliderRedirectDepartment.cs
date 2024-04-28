using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Homepage
{
    public class SliderRedirectDepartment:BaseEntites
    {
        public int DeptId { get; set; }


        public int SliderImageId { get; set; }

        [InverseProperty("SliderRedirectDepartment")]
        public SliderImage SliderImage { get; set; }
    }
}
