using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using EasySite.Core.Entites.Enums;

namespace Core.Entites.Order
{
    public class OrderForm
    {
        public Phone Phone { get; set; }
        public Email Email { get; set; }
        public Address Address { get; set; }
        public FullName FullName { get; set; }
        public int Count { get; set; }
        public double Total { get; set; }
        public DateTime RegistrationTime { get; set; }
        public int DeliveryTime { get; set; }
        public StatusOrder Status { get; set; }
        public int price { get; set; }
        public int Sale { get; set; }
        public string Governorate { get; set; }
        public int ShippingPrice { get; set; }
    }
}
