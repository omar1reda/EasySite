using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Homepage
{
    public class Slider:BaseEntites
    {
        public int Index { get; set; }

        public int HomepageId { get; set; }

        //[InverseProperty("Slider")]
        public Homepage Homepage { get; set; }

        [InverseProperty("Slider")]
        public ICollection<SliderImage> SliderImage { get; set; } = new HashSet<SliderImage>();

    }
}
