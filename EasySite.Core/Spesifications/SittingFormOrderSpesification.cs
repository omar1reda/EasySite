using EasySite.Core.Entites.SittingFormOrder;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class SittingFormOrderSpesification:Spesification<SittingFormOrder>
    {
        public SittingFormOrderSpesification(int siteId ):base(s=>s.SiteId== siteId)
        {
            Includes.Add(s=>s.Address);
            Includes.Add(s => s.Email);
            Includes.Add(s => s.Country);
            Includes.Add(s => s.FullName);
            Includes.Add(s => s.Government);
            Includes.Add(s => s.Phone);

           
        }
    }
}
