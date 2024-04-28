using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Product
{
    public class Ratings:BaseEntites
    {
        public string Text { get; set; }
        public int Count { get; set; }
        public bool ShowRating  { get; set; }
        public string UerName { get; set; }
        public DateTime DateCreated { get; set; }
        public int ProductId { get; set; }

        [InverseProperty("Ratings")]
        public Product Product { get; set; }

    }
}
