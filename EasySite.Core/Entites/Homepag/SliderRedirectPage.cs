using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Homepage
{
    public class SliderRedirectPage:BaseEntites
    {
        public int PageId { get; set; }


        public int SliderImageId { get; set; }

        [InverseProperty("SliderRedirectPage")]
        public SliderImage SliderImage { get; set; }
    }
}
