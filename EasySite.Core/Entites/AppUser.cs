using Core.Entites;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.SignalR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Entites
{
    public class AppUser : IdentityUser
    {
        public string? MarketerId { get; set; }
        public TypeUser UserType { get; set; } = TypeUser.Basic;
        public int? VerificationCode { get; set; }
        public Double Commission { get; set; } = 0;
        public bool Verification{ get; set; }
        public Double? YourAmount { get; set; } = 0;
        public Double? AmountDue { get; set; } = 0;
        public Double AllAmountPaid { get; set; } = 0;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;
        public string? AccountLockMessage { get; set; }
        public DateTime? DateCreated { get; set; }= DateTime.Now;
        public int? Duration { get; set; }
        public FreeTrial freeTrial { get; set; } = FreeTrial.DidntStart;

        [InverseProperty("AppUser")]
        public ICollection<Site> Sites { get; set; } = new HashSet<Site>();

        [InverseProperty("AppUser")]
        public ICollection<Transactions> Transactions { get; set; } = new HashSet<Transactions>();


        public Permitions Permition { get; set; }
        public int? PermitionsId { get; set; }
        public string? MangerId { get; set; }

        // ==> SignalR
        public string? ConnictionId_SignalR { get; set; }

        public ICollection<UsersGroups> UsersGroups { get; set; } = new HashSet<UsersGroups>();
        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
