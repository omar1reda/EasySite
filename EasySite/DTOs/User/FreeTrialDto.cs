using EasySite.Core.Entites.Enums;

namespace EasySite.DTOs.User
{
    public class FreeTrialDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }

    }
}
