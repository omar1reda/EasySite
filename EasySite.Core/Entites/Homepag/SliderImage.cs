using EasySite.Core.Entites.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Homepage
{
    public class SliderImage:BaseEntites
    {
        public string Image { get; set; }
        public SliderRedirectToType TypeName { get; set; }
        public int TypeId { get; set; }
        public int SliderId { get; set; }
        public Slider Slider { get; set; }

    

        //[InverseProperty("SliderImage")]
        //public SliderRedirectDepartment SliderRedirectDepartment { get; set; }

        //[InverseProperty("SliderImage")]
        //public SliderRedirectProdact SliderRedirectProdact { get; set; }


        //[InverseProperty("SliderImage")]
        //public SliderRedirectPage SliderRedirectPage { get; set; }



    }
}
