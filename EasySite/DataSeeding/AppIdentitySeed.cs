using Core.Entites;
using EasySite.Core.Entites;
using EasySite.DTOs;
using Microsoft.AspNetCore.Identity;

namespace EasySite.DataSeeding
{
    public  static class AppIdentitySeed
    {
        public  static async Task SeedUserAsync(UserManager<AppUser> Usermanager)
        {
            if(!Usermanager.Users.Any())
            {
                var User = new AppUser()
                {
                  Email = "omar01reda@gmail.com",
                  UserName= "omar01reda",
                  Id="1"
                };
               await Usermanager.CreateAsync(User,"Omarreda123*#");
            }
        }
    }
}
