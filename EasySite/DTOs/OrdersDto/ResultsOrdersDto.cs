namespace EasySite.DTOs.OrdersDto
{
    public class ResultsOrdersDto
    {
        public int NewOrders { get; set; }
        public int TotleOrders { get; set; }
        public int TotalOrdersLastMonth { get; set; }
        public int TotalOrdersLastWeek { get; set; }

        public Double TotalSales { get; set; }
        public Double TotalSalesLastMonth { get; set; }
        public Double TotalSalesLastWeek { get; set; }

    }
}
