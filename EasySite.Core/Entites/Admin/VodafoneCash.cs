using Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites.Admin
{
    public class VodafoneCash:BaseEntites
    {
        [Phone]
        public int PhoneNumber { get; set; }
    }
}
