using Core.Entites;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class SiteSpesification :Spesification<Site>
    {
        public SiteSpesification(string id):base(p => p.AppUserId == id)
        {
            Includes.Add(p => p.AppUser);
            Includes.Add(p=>p.Homepage);
            Includes.Add(p=>p.Departments);
            Includes.Add(p=>p.Pages);
            Includes.Add(p=>p.ShippingGovernoratesPrices);
            Includes.Add(p => p.Thanks);
        }

        public SiteSpesification(string linkName , int id) : base(p => p.LinkName == linkName)
        {
            Includes.Add(p => p.AppUser);
            Includes.Add(p => p.Homepage);
            Includes.Add(p => p.Departments);
            Includes.Add(p => p.Pages);
            Includes.Add(p => p.ShippingGovernoratesPrices);
            Includes.Add(p => p.Thanks);
        }

        public SiteSpesification(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.Products);
            Includes.Add(p => p.AppUser);
            Includes.Add(p => p.Homepage);
            Includes.Add(p => p.Departments);
            Includes.Add(p => p.Pages);
            Includes.Add(p => p.ShippingGovernoratesPrices);
            Includes.Add(p => p.Thanks);
        }


    }
}
