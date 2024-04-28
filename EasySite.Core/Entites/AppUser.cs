using Core.Entites;
using EasySite.Core.Entites.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites
{
    public class AppUser : IdentityUser
    {
        public TypeUser Type { get; set; }
        public int? VerificationCode { get; set; }
        public bool Verification{ get; set; }
        public float? AmountPaid { get; set; } = 0;
        public float? AmountDue { get; set; } = 0;

        [InverseProperty("AppUser")]
        public ICollection<Site> ShippingGovernoratesPrices { get; set; } = new HashSet<Site>();

    }
}
