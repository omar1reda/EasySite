using Core.Entites.Product;
using EasySite.Core.Entites.Params;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class RatingSpesification:Spesification<Ratings>
    {
        public RatingSpesification(QureyParams qureyParams , int? productId):base(p=> productId > 0 ? p.ProductId == productId: p.Id >0)
        {
            Includes.Add(r=>r.Product);

            switch (qureyParams.Sort)
            {
                case "CountAsc":
                    AddOrderBy(r=>r.Count);
                    break;
                case "CountDesc":
                    AddOrderByDescending(r=>r.Count);
                    break;
                case "ShowRatingAsc":
                    AddOrderBy(r=>r.ShowRating);
                    break;
                case "ShowRatingDesc":
                    AddOrderByDescending(r => r.ShowRating);
                    break;
                default:
                    AddOrderByDescending(r=>r.DateCreated);
                    break;
            }

            if(qureyParams.IsPagination)
            {
                AddPagination((qureyParams.PageIndex -1) * qureyParams.PageSize, qureyParams.PageSize);
            }
        }
        public RatingSpesification(int id) : base(r => r.Id == id)
        {
            Includes.Add(r => r.Product);
        }


        public RatingSpesification(int productId , int id) : base(r => r.ProductId == productId && r.ShowRating == true)
        {
            Includes.Add(r => r.Product);
        }
    }
}
