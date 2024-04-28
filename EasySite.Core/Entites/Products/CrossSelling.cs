using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EasySite.Core.Entites.Products
{
    public class CrossSelling:BaseEntites
    {
        public bool IsActive { get; set; }
        public int ProductIdToRedirect { get; set; }
        public int ProductId { get; set; }

        public int Sale { get; set; }

        public string BottonTextAgree { get; set; }
        public string BottonColorAgree { get; set; }


        public string ButtonTextReject { get; set; }
        public string ButtonColorReject { get; set; }

        public string Titele { get; set; }
        public string Description { get; set; }
    }
}
