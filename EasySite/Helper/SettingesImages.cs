using AutoMapper;
using Core.Entites;
using Core.Entites.Product;
using EasySite.Core.Entites.Product;
using EasySite.DTOs;
using EasySite.DTOs.productDTO.ToReturn;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace EasySite.Helper
{



    public class ResolveImageInSite : IValueResolver<Site, SiteNoFileDto, string>
    {
        private readonly IConfiguration _configuration;

        public ResolveImageInSite(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string Resolve(Site source, SiteNoFileDto destination, string destMember, ResolutionContext context)
        {
            if (source.MiniIcon != null)
            {
                return $"{_configuration["ApiBaseUrl"]}{source.MiniIcon}";
            }
            if (source.Logo != null)
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Logo}";
            }
            return string.Empty;
        }
    }


    public class ResolveImageInDepartment : IValueResolver<Department, DepartmentToreturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ResolveImageInDepartment(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string Resolve(Department source, DepartmentToreturnDto destination, string destMember, ResolutionContext context)
        {
            if (source.Image != null)
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Image}";
            }
            return string.Empty;
        }
    }


    public class ResolveImageInProduct : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ResolveImageInProduct(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (source.Image != null)
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Image}";
            }
            if (source.OtherImagesOfProduct != null)
            {
                foreach (var item in source.OtherImagesOfProduct)
                {
                    return $"{_configuration["ApiBaseUrl"]}{item.Image}";
                }                
            }
            return string.Empty;
        }
    }


    public static class SettingesImages
    {
        public static string UplodeFile(IFormFile file , string FoldeName )
        {
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot\\Files" , FoldeName);

            var FileName = $"{Guid.NewGuid()}{file.FileName}";

            var FilePath = Path.Combine(FolderPath, FileName);

            using var Sf = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Sf);

            return $"Files/{FoldeName}/{FileName}";
        }

    }
}

