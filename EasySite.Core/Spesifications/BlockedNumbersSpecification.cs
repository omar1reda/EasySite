using Core.Entites;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class BlockedNumbersSpecification:Spesification<BlockedNumbers>
    {
        public BlockedNumbersSpecification(int SiteId):base(b=>b.SiteId==SiteId)
        {
            Includes.Add(b => b.Site);
        }

        public BlockedNumbersSpecification(string phoneNumber) : base(b => b.Number == phoneNumber)
        {
            Includes.Add(b => b.Site);
        }
    }
}
