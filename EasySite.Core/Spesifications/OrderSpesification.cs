using Core.Entites.orders;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Params;
using EasySite.Repository.Spesifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class OrderSpesification:Spesification<Order>
    {
        // Get All && Filtration
        public OrderSpesification(int sitId , QureyParamsOrder qureyParams)
            :base(o=>
           ( o.SiteId== sitId)
            &&
           (string.IsNullOrEmpty(qureyParams.CustomerName) || o.FullName.ToLower().Contains(qureyParams.CustomerName.ToLower()))
             &&
            (string.IsNullOrEmpty(qureyParams.CustomerAddress) || o.Address.ToLower().Contains(qureyParams.CustomerAddress.ToLower()))
             &&
            (string.IsNullOrEmpty(qureyParams.StatusOrder) || o.Status == (StatusOrder)Enum.Parse(typeof(StatusOrder), qureyParams.StatusOrder))
             &&
            (string.IsNullOrEmpty(qureyParams.PhoneNumper) || o.Phone.ToLower().Contains(qureyParams.PhoneNumper.ToLower()))
             &&
            (!qureyParams.EndDate.HasValue || qureyParams.EndDate <= o.OrderDate)
             &&
            (!qureyParams.StartDate.HasValue || qureyParams.StartDate >= o.OrderDate)
             &&
            (!qureyParams.ProductId.HasValue || o.OrderItems.Any(i => i.ProductId == qureyParams.ProductId))
            )
        {
            IncludeStrings.Add("OrderItems.ProductDataInOrder.VariantOptionsOrder");


            if (qureyParams.IsPagination)
            {
                AddPagination((qureyParams.PageIndex - 1 ) * qureyParams.PageSize , qureyParams.PageSize);
            }
             switch(qureyParams.Sort)
            {
                case "TotlePriceAsc":
                    AddOrderBy(p => p.TotalPrice);
                    break;
                case "TotlePriceDesc":
                    AddOrderByDescending(p => p.TotalPrice);
                    break;

                case "IsWatchedAsc":
                    AddOrderBy(p => p.IsWatched);
                    break;
                case "IsWatchedDesc":
                    AddOrderByDescending(p => p.IsWatched);
                    break;

                case "OrderDateAsc":
                    AddOrderBy(p => p.OrderDate);
                    break;
                case "OrderDateDesc":
                    AddOrderByDescending(p => p.OrderDate);
                    break;

                default:
                    AddOrderByDescending(p => p.OrderDate);
                    break;

            }
            
        }

        public OrderSpesification(int id):base(o=>o.Id==id)
        {
              IncludeStrings.Add("OrderItems.ProductDataInOrder.VariantOptionsOrder");
            //IncludeStrings.Add("OrderItems.ProductDataInOrder.VariantOptionOrder");
        }
        public OrderSpesification(int siteId,int id) : base(o => o.SiteId == siteId)
        {
            IncludeStrings.Add("OrderItems.ProductDataInOrder.VariantOptionsOrder");
            //IncludeStrings.Add("OrderItems.ProductDataInOrder.VariantOptionOrder");
        }
        public OrderSpesification(string ipAddres , int siteId) : base(o => o.SiteId==siteId && o.IpAddres == ipAddres)
        {
            //IncludeStrings.Add("OrderItems.ProductDataInOrder.VariantOptionsOrder");
            //IncludeStrings.Add("OrderItems.ProductDataInOrder.VariantOptionOrder");
        }
    }
}
