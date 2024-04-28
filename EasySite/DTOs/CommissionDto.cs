using EasySite.DTOs.User;

namespace EasySite.DTOs
{
    public class CommissionDto
    {
        
        public Double Commission { get; set; }
        public List<ReferralsDto> ReferralsesDto { get; set; }= new List<ReferralsDto>();
    }
}
