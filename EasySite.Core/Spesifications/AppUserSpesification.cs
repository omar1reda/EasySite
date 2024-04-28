using EasySite.Core.Entites;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Params;
using EasySite.Repository.Spesifications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySite.Core.Spesifications
{
    public class AppUserSpesification:Spesification<AppUser>
    {
        private readonly UserManager<AppUser> UserManager;

        public AppUserSpesification(QueryParamsUsers qureyParams , UserManager<AppUser> userManager) 
            :base( u=>
            ( string.IsNullOrEmpty(qureyParams.searchBy_UserName) || u.UserName.ToLower()== qureyParams.searchBy_UserName.ToLower())
            &&
            (string.IsNullOrEmpty(qureyParams.searchBy_Email) || u.Email.ToLower() == qureyParams.searchBy_Email.ToLower())
            &&
            (string.IsNullOrEmpty(qureyParams.searchBy_UserType) || u.UserType == (TypeUser) Enum.Parse( typeof(Entites.Enums.TypeUser),qureyParams.searchBy_UserType))            )
        {

            switch (qureyParams.Sort)
            {
                case "DateCreatedAsc":
                    AddOrderBy(u => u.DateCreated);
                    break;
                case "DateCreatedDesc":
                    AddOrderByDescending(u => u.DateCreated);
                    break;

                case "IsActiveAsc":
                    AddOrderBy(u => u.IsActive);
                    break;
                case "IsActiveDesc":
                    AddOrderByDescending(u => u.IsActive);
                    break;

            }

            if(qureyParams.IsPagination)
            {
                AddPagination((qureyParams.PageIndex - 1) * qureyParams.PageSize, qureyParams.PageSize);
            }

            this.UserManager = userManager;
        }

        public AppUserSpesification(string userId):base(u=>u.MangerId == userId)
        {
            Includes.Add(u => u.Permition);
        }
    }
}
