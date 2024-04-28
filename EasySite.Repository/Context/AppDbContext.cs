using Core.Entites;
using Core.Entites.Homepage;
using Core.Entites.orders;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Admin;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Homepag;
using EasySite.Core.Entites.Order;
using EasySite.Core.Entites.orders;
using EasySite.Core.Entites.Product;
using EasySite.Core.Entites.SignalR;
using EasySite.Core.Entites.SittingFormOrder;
using EasySite.DTOs.HomePage;
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
            //builder.Entity<IdentityUserRole<string>>().HasNoKey();

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.HasKey(iur => new { iur.UserId, iur.RoleId });
            });

            // One To one ===>
            builder.Entity<AppUser>()
                .HasOne(a => a.Permition)
                .WithOne(m => m.User)
                .HasForeignKey<Permitions>(m => m.AppUserId);


            builder.Entity<ProductData>()
           .HasOne(u => u.Product).WithMany(u => u.ProductData).IsRequired().OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
           .HasOne(u => u.Site).WithMany(u => u.Products).IsRequired().OnDelete(DeleteBehavior.Restrict);

            /// Composite Primary Key ====>
            builder.Entity<product_Variant_Options_ProductData>().HasKey(P=>new { P.ProductDataId, P.Product_Variant_OptionsId});

            builder.Entity<UsersGroups>().HasKey(P => new { P.AppUserId, P.GroupId });

            builder.Entity<Site>()
           .HasOne(u => u.Homepage)
           .WithOne(h => h.Site)
           .HasForeignKey<Homepage>(h => h.SiteId);


            // تحويل ال Enum الي string في الداتا بيز
            builder.Entity<AppUser>(u =>
            {
                u.Property(e => e.UserType)
                    .HasConversion(
                        v => v.ToString(),
                        v => (TypeUser)Enum.Parse(typeof(TypeUser), v)
                    );

                u.Property(e => e.freeTrial)
                   .HasConversion(
                       v => v.ToString(),
                       v => (FreeTrial)Enum.Parse(typeof(FreeTrial), v)
                   );
            });

            builder.Entity<Site>(u =>
            {
                u.Property(e => e.FontType)
                    .HasConversion(
                        v => v.ToString(),
                        v => (FontType)Enum.Parse(typeof(FontType), v)
                    );

                u.Property(e => e.Currency)
                    .HasConversion(
                        v => v.ToString(),
                        v => (currency)Enum.Parse(typeof(currency), v)
                    );


            });

            builder.Entity<ProductsInHomePage>(u =>
            {
                u.Property(e => e.sortBy)
                    .HasConversion(
                        v => v.ToString(),
                        v => (SortBy)Enum.Parse(typeof(SortBy), v)
                    );
            });

            builder.Entity<SliderImage>(u =>
            {
                u.Property(e => e.TypeName)
                    .HasConversion(
                        v => v.ToString(),
                        v => (SliderRedirectToType)Enum.Parse(typeof(SliderRedirectToType), v)
                    );
            });

            builder.Entity<Order>(u =>
            {
                u.Property(e => e.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (StatusOrder)Enum.Parse(typeof(StatusOrder), v)
                    );
            });

            builder.Entity<Message>(u =>
            {
                u.Property(e => e.statusMessage)
                    .HasConversion(
                        v => v.ToString(),
                        v => (StatusMessage)Enum.Parse(typeof(StatusMessage), v)
                    );
            });

            builder.Entity<Message>(u =>
            {
                u.Property(e => e.messageType)
                    .HasConversion(
                        v => v.ToString(),
                        v => (MessageType)Enum.Parse(typeof(MessageType), v)
                    );
            });
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
        public DbSet<OtherImageOfProduct> OtherImagesOfProduct { get; set; }
        public DbSet<Product_Variants> Product_Variants { get; set; }
        public DbSet<Product_Variant_Options> Product_Variant_Options { get; set; }
        //public DbSet<product_variant_option_combination> product_variant_option_combination { get; set; }
        public DbSet<ProductData> ProductData { get; set; }
        public DbSet<Ratings> Ratings { get; set; }
        public DbSet<DepartmentsInHomePage> DepartmentsInHomePage { get; set; }
        public DbSet<Homepage> Homepage { get; set; }
        public DbSet<ProductsInHomePage> ProductsFromDepartmentInHomePage { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<SliderImage> SliderImage { get; set; }
        public DbSet<SittingFormOrder> SittingFormOrder { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductDataInOrder> ProductDataInOrder { get; set; }
        public DbSet<VariantOptionOrder> ItemVariantOption { get; set; }

        public DbSet<Permitions> ManagerPermitions { get; set; }
        public DbSet<VodafoneCash> VodafoneCash { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<DollarPrice> DollarPrice { get; set; }
        public DbSet<YoutubeVideo> YoutubeVideo { get; set; }


        public DbSet<UsersGroups> UsersGroups { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }
    }



}
