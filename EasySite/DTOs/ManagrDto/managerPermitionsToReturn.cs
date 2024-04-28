using System.ComponentModel.DataAnnotations;

namespace EasySite.DTOs.ManagrDto
{
    public class managerPermitionsToReturn
    {
        public string Id { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }

        public DateTime? DateCreated { get; set; }

        public PermitionsDto PermitionsDto { get; set; }

    }
}
