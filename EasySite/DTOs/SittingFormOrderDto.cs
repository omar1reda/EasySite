using Core.Entites;
using EasySite.Core.Entites.SittingFormOrder;

namespace EasySite.DTOs
{
    public class SittingFormOrderDto
    {
        public int Id { get; set; }
        public int SiteId { get; set; }

        public List<object> Items { get; set; } = new List<object>();

    }
}
