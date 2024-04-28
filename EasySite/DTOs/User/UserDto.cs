using Core.Entites;
using EasySite.Core.Entites.Enums;

namespace EasySite.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Role { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public double? YourAmount { get; set; } = 0;
        public double? AmountDue { get; set; } = 0;
        public string Token { get; set; }
        //public int VerificationCode { get; set; }
        public bool Verification { get; set; }
        public bool IsActive { get; set; }
        public string AccountLockMessage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? Duration { get; set; }
        public string freeTrial { get; set; }
    }
}
