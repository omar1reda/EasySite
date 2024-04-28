using Core.Entites;
using Core.Entites.Homepage;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.SittingFormOrder;
using EasySite.Core.I_Repository;
using EasySite.DataSeeding.TextData;
using EasySite.DTOs;
using EasySite.DTOs.HomePage;
using EasySite.DTOs.productDTO;
using EasySite.Repository.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using StackExchange.Redis;
using System;
using System.Data;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace EasySite.DataSeeding
{
    public  static class AppApplecationSeed
    {
        public  static async Task AddAllRolesAndAddAdmin(RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager)
        {
         
           
            string [] Listroles = { CsRoles.Manager , CsRoles.User, CsRoles.Admin, CsRoles.Supervisor };

            foreach (var role in Listroles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = role });
                }

            }


             var admin= await _userManager.FindByEmailAsync("omar0ereda@gmail.com");
            if(admin == null)
            {
                var userAdmin = new AppUser()
                {
                    Email = "omar01reda@gmail.com",
                    UserName = "omar01reda",
                    Verification=true,
                    VerificationCode=666666
                };
                var Result = await _userManager.CreateAsync(userAdmin, "Omarreda123*#");

                if (Result.Succeeded)
                {
                    var adminAdded = await _userManager.FindByEmailAsync("omar01reda@gmail.com");
                    await _userManager.AddToRoleAsync(adminAdded, CsRoles.Admin);
                }
                
            }          

        }

        public static async Task<ObjectesSeedProductRatingDepartment> GetObjectesSeedReturnJson()
        {
            var DepartmentsJson = File.ReadAllText("../EasySite/DataSeeding/TextData/Departments.json");

            var BagsJson = File.ReadAllText("../EasySite/DataSeeding/TextData/Bags.json");
            var ShosesJson = File.ReadAllText("../EasySite/DataSeeding/TextData/Shoses.json");
            var SportswearJson = File.ReadAllText("../EasySite/DataSeeding/TextData/Sportswear.json");
            var StwatchJson = File.ReadAllText("../EasySite/DataSeeding/TextData/Stwatch.json");
            var TShirtsJson = File.ReadAllText("../EasySite/DataSeeding/TextData/T-Shirts.json");
            var PagesJson = File.ReadAllText("../EasySite/DataSeeding/TextData/Pages.json");
            var RatingsJson = File.ReadAllText("../EasySite/DataSeeding/TextData/Ratings.json");
            var HomePageJson = File.ReadAllText("../EasySite/DataSeeding/TextData/HomePage.json");
            var sittingFormOrderJson = File.ReadAllText("../EasySite/DataSeeding/TextData/SittingFormOrder.json");

            var Departments = JsonSerializer.Deserialize<List<Department>>(DepartmentsJson);
            var Bags = JsonSerializer.Deserialize<List<ProductDto>>(BagsJson);
            var Shoses = JsonSerializer.Deserialize<List<ProductDto>>(ShosesJson);
            var Sportswear = JsonSerializer.Deserialize<List<ProductDto>>(SportswearJson);
            var Stwatch = JsonSerializer.Deserialize<List<ProductDto>>(StwatchJson);
            var TShirts = JsonSerializer.Deserialize<List<ProductDto>>(TShirtsJson);
            var Pages = JsonSerializer.Deserialize<List<Pages>>(PagesJson);
            var Ratings = JsonSerializer.Deserialize<List<Ratings>>(RatingsJson);
            var homePage = JsonSerializer.Deserialize<HomePageDto>(HomePageJson);
            var sittingFormOrder = JsonSerializer.Deserialize<SittingFormOrder>(sittingFormOrderJson);

            //var ProductsController = new ProductsController();
            List<List<ProductDto>> ListOfListProduct =new List<List<ProductDto>>();
            ListOfListProduct.Add(Bags);
            ListOfListProduct.Add(Shoses);
            ListOfListProduct.Add(Sportswear);
            ListOfListProduct.Add(Stwatch);
            ListOfListProduct.Add(TShirts);
            var objectesSeedProductRatingDepartment = new ObjectesSeedProductRatingDepartment();

            objectesSeedProductRatingDepartment.ListOfListProduct = ListOfListProduct;
            objectesSeedProductRatingDepartment.departments = Departments;
            objectesSeedProductRatingDepartment.pages = Pages;
            objectesSeedProductRatingDepartment.homePageDto = homePage;
            objectesSeedProductRatingDepartment.ratings = Ratings;
            objectesSeedProductRatingDepartment.sittingFormOrder = sittingFormOrder;
            return objectesSeedProductRatingDepartment;


           
        }
   




    }
}
