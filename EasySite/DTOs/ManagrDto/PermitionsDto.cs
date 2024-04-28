namespace EasySite.DTOs.ManagrDto
{
    public class PermitionsDto
    {
        public int Id { get; set; }
        public bool ViewOrders { get; set; }
        public bool EditStatusOrder { get; set; }
        public bool AddProduct { get; set; }
        public bool UpdateProduct { get; set; }
        public bool DeleteProduct { get; set; }
        public bool AddDepartment { get; set; }
        public bool UpdateDepartment { get; set; }
        public bool DeleteDepartment { get; set; }
        public bool UpdateSite { get; set; }
        public bool UpdateHomePage { get; set; }
        public bool UpdateSittingFormOrder { get; set; }
        public bool DeleteShippingGovernorates { get; set; }
        public bool AddDeleteShippingGovernorates { get; set; }
        public bool AddRating { get; set; }
        public bool UpdateRating { get; set; }
        public bool DeleteRating { get; set; }
    }
}
