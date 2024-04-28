using EasySite.Core.Entites.Enums;

namespace EasySite.DTOs
{
    public class UserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public TypeUser Type { get; set; }
        public float? AmountPaid { get; set; } = 0;
        public float? AmountDue { get; set; } = 0;
        public string Token { get; set; }
        //public int VerificationCode { get; set; }
        public bool Verification { get; set; }
    }
}
