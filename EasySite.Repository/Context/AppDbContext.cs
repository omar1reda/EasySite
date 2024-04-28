using Core.Entites;
using Core.Entites.Homepage;
using Core.Entites.Order;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Homepag;
using EasySite.Core.Entites.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasySite.Repository.Context
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        //IdentityDbContext<UserSite>
        public AppDbContext(DbContextOptions<AppDbContext> option):base(option) 
        {
         
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<IdentityUserLogin<string>>().HasNoKey();
            builder.Entity<IdentityUserToken<string>>().HasNoKey();
            builder.Entity<IdentityUserRole<string>>().HasNoKey();
            

            builder.Entity<product_variant_option_combination>()
          .HasKey(e => new { e.SKUId, e.Product_Variant_OptionsId });


                builder.Entity<Site>()
           .HasOne(u => u.Homepage)
           .WithOne(h => h.Site)
           .HasForeignKey<Homepage>(h => h.SiteId);

           
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Thanks> Thanks { get; set; }
        public DbSet<SocialMedia> SocialMedia { get; set; }
        public DbSet<ShippingGovernoratesPrices> ShippingGovernoratesPrices { get; set; }
        public DbSet<Pages> Pages { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<BlockedNumbers> BlockedNumbers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Variants> Product_Variants { get; set; }
        public DbSet<Product_Variant_Options> Product_Variant_Options { get; set; }
        public DbSet<product_variant_option_combination> product_variant_option_combination { get; set; }
        public DbSet<SKU> SKUs { get; set; }
        public DbSet<Ratings> Ratings { get; set; }
        public DbSet<DepartmentsInHomePage> DepartmentsInHomePage { get; set; }
        public DbSet<Homepage> Homepage { get; set; }
        public DbSet<ProductsFromDepartmentInHomePage> ProductsFromDepartmentInHomePage { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<SliderImage> SliderImage { get; set; }
        public DbSet<SliderRedirectDepartment> SliderRedirectDepartment { get; set; }
        public DbSet<SliderRedirectPage> SliderRedirectPage { get; set; }
        public DbSet<SliderRedirectProdact> SliderRedirectProdact { get; set; }


    }
  
    //protected override void OnModelCreating(ModelBuilder modelBuilder) =>
    //    modelBuilder.Entity<product_variant_option_combination>()
    //        .HasKey(e => new { e.SKUId, e.Product_Variant_OptionsId });


}
