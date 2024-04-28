using Core.Entites;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EasySite.Core.Spesifications
{
    public class TransactionSpesification: Spesification<Transactions>
    {
        public TransactionSpesification(string UserId):base(t=>t.AppUserId==UserId)
        {
            
        }
    }
}
