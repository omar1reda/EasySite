using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Homepage
{
    public class SliderRedirectProdact:BaseEntites
    {
        public int ProductId { get; set; }


        public int SliderImageId { get; set; }

        [InverseProperty("SliderRedirectProdact")]
        public SliderImage SliderImage { get; set; }
    }
}
