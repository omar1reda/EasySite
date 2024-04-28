using Core.Entites;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class PagesSpesification:Spesification<Pages>
    {
        public PagesSpesification(int SiteId):base(p=>p.SiteId== SiteId)
        {
            Includes.Add(p => p.Site);
        }
    }
}
