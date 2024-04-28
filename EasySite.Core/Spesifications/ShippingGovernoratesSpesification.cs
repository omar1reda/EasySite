using Core.Entites;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class ShippingGovernoratesSpesification : Spesification<ShippingGovernoratesPrices>
    {
        public ShippingGovernoratesSpesification(int SiteId):base(s=>s.SiteId== SiteId)
        {
            Includes.Add(s => s.Site);

            
        }

    }
}
