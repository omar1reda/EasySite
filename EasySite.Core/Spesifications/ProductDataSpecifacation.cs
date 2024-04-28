using Core.Entites.Product;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class ProductDataSpecifacation:Spesification<ProductData>
    {
        public ProductDataSpecifacation(int prodactId ):base(D=>D.ProductId==prodactId)
        {
            
        }
        public ProductDataSpecifacation(int id , int prodactId) : base(D => D.Id == id)
        {

        }
    }
}
