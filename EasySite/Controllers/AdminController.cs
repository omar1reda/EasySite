using Core.Entites;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Admin;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Params;
using EasySite.Core.Entites.SittingFormOrder;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs.Admins;
using EasySite.DTOs.User;
using EasySite.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using static System.TimeZoneInfo;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AdminController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._tokenService = tokenService;
        }

        [HttpGet("GetAllAdmins")]
        [ProducesResponseType(typeof(List<AdminToReturn>), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.Admin)]
        public async Task<ActionResult<List<AdminToReturn>>> GetAllAdmins()
        {
            var Admins = await _userManager.GetUsersInRoleAsync(CsRoles.Admin);
            var Supervisor = await _userManager.GetUsersInRoleAsync(CsRoles.Supervisor);
            var AllAdmins = new List<AppUser>();
            AllAdmins.AddRange(Admins.OrderBy(a => a.DateCreated));
            AllAdmins.AddRange(Supervisor.OrderBy(s => s.DateCreated));

            var AdminsToreturn = AllAdmins.Select( a =>
                new AdminToReturn()
                {
                    Id = a.Id,
                    Email = a.Email,
                    UserName = a.UserName,
                    Role = _userManager.GetRolesAsync(a).Result.FirstOrDefault(),
                    DateCreated = a.DateCreated
                }).ToList();

            return Ok(AdminsToreturn);
        }

        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(AdminToReturn), 200)]
        [HttpPost("AddAdminOrSopervisor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.Admin)]
        public async Task<ActionResult<AdminToReturn>> AddAdminOrSopervisor(AdminRegesterDto adminRegesterDto)
        {
            if (adminRegesterDto.Email == null)
            {
                adminRegesterDto.Email = $"{Guid.NewGuid().ToString()}@gmail.com";
            }
            else
            {
                var userAdmin = await _userManager.FindByEmailAsync(adminRegesterDto.Email);
                if (userAdmin != null)
                {
                    return BadRequest(new ApiResponse(400, "  الاميل موجود بالفعل "));
                }
            }

            var userAdminByUserName = await _userManager.FindByEmailAsync(adminRegesterDto.Email);
            if (userAdminByUserName != null)
            {
                return BadRequest(new ApiResponse(400, "  اليوزر موجود بالفعل "));
            }

            if (adminRegesterDto.Role != CsRoles.Admin && adminRegesterDto.Role != CsRoles.Supervisor)
            {
                return BadRequest(new ApiResponse(400, "  [Supervisor or Admin] يرجي اختيار  "));
            }

            var admin = new AppUser()
            {
                Email = adminRegesterDto.Email,
                UserName = adminRegesterDto.UserName,
                Verification= true,
            };

            var result = await _userManager.CreateAsync(admin, adminRegesterDto.Password);
            if (result.Succeeded)
            {
                
                var adminuser = await _userManager.FindByEmailAsync(adminRegesterDto.Email);
                await _userManager.AddToRoleAsync(adminuser , adminRegesterDto.Role);

                var AdminToReturn = new AdminToReturn()
                {
                    Id = adminuser.Id,
                    Email = adminuser.Email,
                    UserName = adminuser.UserName,
                    Role = adminRegesterDto.Role,
                    DateCreated = adminuser.DateCreated
                };
                return Ok(AdminToReturn);
            }
            return BadRequest(new ApiResponse(400, "  خطء في انشاء الحساب "));
        }

        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(200)]
        [HttpDelete("DeleteAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.Admin)]
        public async Task<ActionResult> DeleteAdmin(string AdminId)
        {
            var admin = await _userManager.FindByIdAsync(AdminId);
            if (admin != null)
            {
                if (admin.Email == "omar01reda@gmail.com")
                {
                    return BadRequest(new ApiResponse(400, "لأ يمكن حذف المسؤل الرئيسي"));
                }
                await _userManager.DeleteAsync(admin);
                return Ok();
            }
            return BadRequest(new ApiResponse(400, "الايميل غير موجود"));
        }

        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(AdminToReturn), 200)]
        [HttpPost("AdminLogin")]
        public async Task<ActionResult<AdminToReturn>> AdminLogin(LoginDto loginDto)
        {
            var AdminByEmail = await _userManager.FindByEmailAsync(loginDto.EmailOrUserName);
            var AdminByUserName = await _userManager.FindByNameAsync(loginDto.EmailOrUserName);

            if (AdminByEmail != null)
            {

                var signInResult = await _signInManager.CheckPasswordSignInAsync(AdminByEmail, loginDto.Password, false);
                if (signInResult.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(AdminByEmail);
                    if (role.FirstOrDefault() != CsRoles.Supervisor && role.FirstOrDefault() != CsRoles.Admin)
                    {
                        return BadRequest(new ApiResponse(400, $"غير مصرح لك الدخول من هنا"));
                    }
                    var AdminToReturnDto = await CreateUserAdminDto(AdminByEmail);
                    return Ok(AdminToReturnDto);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, $"الباسورد غير صحيح"));
                }

            }
            else if (AdminByUserName != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(AdminByUserName, loginDto.Password, false);
                if (signInResult.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(AdminByUserName);
                    if (role.FirstOrDefault() != CsRoles.Supervisor && role.FirstOrDefault() != CsRoles.Admin)
                    {
                        return BadRequest(new ApiResponse(400, $"غير مصرح لك الدخول من هنا"));
                    }
                    var adminToReturn = await CreateUserAdminDto(AdminByUserName);
                    return Ok(adminToReturn);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, $"الباسورد غير صحيح"));
                }
            }

            return BadRequest(new ApiResponse(400, $"ايميل غير صحيح"));
        }

        private async Task<AdminToReturn> CreateUserAdminDto(AppUser admin)
        {
            var AdminToReturn = new AdminToReturn()
            {
                Id = admin.Id,
                Email = admin.Email,
                UserName = admin.UserName,
                Role = _userManager.GetRolesAsync(admin).Result.FirstOrDefault(),
                DateCreated = DateTime.Now,

                Token =await _tokenService.GetToken(admin)
            };

            return AdminToReturn;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{CsRoles.Admin},{CsRoles.Supervisor}" )]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(List<UserDto>), 200)]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers( [FromQuery] QueryParamsUsers qureyParams)
        {
            var AppUserSpec =  new AppUserSpesification(qureyParams, _userManager);
            var Users =await _unitOfWork.Repository<AppUser>().GetAllWithSpesificationAsync(AppUserSpec);
            var filteredUsers = new List<AppUser>();
            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.FirstOrDefault() == CsRoles.User)
                {
                    filteredUsers.Add(user);
                }
            }
            var UsersToReturn = filteredUsers.Select( u => new UserDto()
            { 
                AmountDue = u.AmountDue,
                YourAmount = u.YourAmount,
                DateCreated = u.DateCreated,
                Duration = u.Duration,
                Email = u.Email,
                EndDate = u.EndDate,
                freeTrial   = u.freeTrial.ToString(),
                Id = u.Id,
                IsActive = u.IsActive,
                StartDate = u.StartDate,
                Type = u.UserType.ToString(),
                UserName = u.UserName,
                Role  = _userManager.GetRolesAsync(u).Result.FirstOrDefault(),
                Verification = u.Verification,     
            }).ToList();

            return Ok(UsersToReturn);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.Admin},{CsRoles.Supervisor}")]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType( 200)]
        [HttpPut("DisableAccount")]
        public async Task<ActionResult> DisableAccount(string UserId , string? accountLockMessage = "حسابك غير متاح مؤقتأ")
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if(user.Email == "omar01reda@gmail.com")
                return BadRequest(new ApiResponse(400, "لا يمكن تعطيل حساب الادمن الاساسي"));

            if (user != null)
            {
                user.AccountLockMessage= accountLockMessage;
                user.IsActive = false;
                user.Sites.Select(s => s.IsActive= false).ToList();
                await _userManager.UpdateAsync(user);
                return Ok();
            }
          return BadRequest(new ApiResponse(400 , "معرف غير صحيح"));  
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.Admin},{CsRoles.Supervisor}")]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(200)]
        [HttpPut("EnableAccount")]
        public async Task<ActionResult> EnableAccount(string UserId ) 
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                user.AccountLockMessage = ""; 
                user.IsActive = true;
                user.Sites.Select(s => s.IsActive = true).ToList();
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            return BadRequest(new ApiResponse(400, "معرف غير صحيح"));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.Admin},{CsRoles.Supervisor}")]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(200)]
        [HttpPut("MakeUserAccountPro")]
        public async Task<ActionResult> MakeUserAccountPro(int AmountOfMoney , string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                if(user.AmountDue > AmountOfMoney)
                {
                    user.AmountDue =  user.AmountDue - AmountOfMoney ;
                }
                user.YourAmount = user.YourAmount+  AmountOfMoney - user.AmountDue ;
                user.UserType = TypeUser.Pro;
                await _userManager.UpdateAsync(user);

                if (user.MarketerId != null)
                {
                    var UserMarketer = await _userManager.FindByIdAsync(user.MarketerId);
                    if(UserMarketer != null)
                    {
                        UserMarketer.Commission = UserMarketer.Commission + AmountOfMoney /5;
                        await _userManager.UpdateAsync(UserMarketer);
                    }
                }
                var transactions = new Transactions()
                {
                     AppUserId = UserId,
                     Money= AmountOfMoney ,
                     Status = "ناجحه",
                     TransactionTime= DateTime.Now,
                     Details = $"عن طريق خدمة العملاء {AmountOfMoney}$ تم شحن رصيدك بقيمة"
                };
                var transactions2 = new Transactions()
                {
                    AppUserId = UserId,
                    Money = AmountOfMoney,
                    Status = "تفاصيل الرصيد",
                    TransactionTime = DateTime.Now,
                    Details = $"رصيدك بعد الشحن: {user.YourAmount+ user.AmountDue}"
                };
                await _unitOfWork.Repository<Transactions>().AddRangeAsync(new[] { transactions, transactions2 });
                await _unitOfWork.CompletedAsynk();

                return Ok();
            }
            return BadRequest(new ApiResponse (404 , "معرف غير موجود"));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.Admin}")]
        [ProducesResponseType(typeof(VodafoneCash), 200)]
        [HttpPost("AddVodafoneCash")]
        public async Task<ActionResult> AddVodafoneCash(int PhoneNumber)
        {
          var phoneAdded=  await _unitOfWork.Repository<VodafoneCash>().AddAsync(new VodafoneCash {  PhoneNumber = PhoneNumber });
            await _unitOfWork.CompletedAsynk();
            return Ok(phoneAdded);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<VodafoneCash>), 200)]
        [HttpGet("GetAllVodafoneCash")]
        public async Task<ActionResult<IEnumerable<VodafoneCash>>> GetAllVodafoneCash()
        {
            var Phones = await _unitOfWork.Repository<VodafoneCash>().GetAllAsync();
            return Ok(Phones);

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =CsRoles.Admin)]
        [ProducesResponseType(200)]
        [HttpDelete("DeleteVodafoneCash")]
        public async Task<ActionResult> DeleteVodafoneCash(int id)
        {
            var Phone = await _unitOfWork.Repository<VodafoneCash>().GetByIdAsync(id);
            if (Phone == null)
                return BadRequest(new ApiResponse(404, "معرف غير صحيح"));

            _unitOfWork.Repository<VodafoneCash>().Delete(Phone);
            await _unitOfWork.CompletedAsynk();
            return Ok();

        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.Admin)]
        [ProducesResponseType(typeof(DollarPrice), 200)]
        [HttpPut("AddOrUpdateDollarPrice")]
        public async Task<ActionResult> AddOrUpdateDollarPrice(int dollarPrice)
        {
            var TableDollarPrice = await _unitOfWork.Repository<DollarPrice>().GetAllAsync();
            // Update ===> 
            if(TableDollarPrice.Count() > 0)
            {
                var PriceUpdated = TableDollarPrice.FirstOrDefault();
                PriceUpdated.Price = dollarPrice;

                 _unitOfWork.Repository<DollarPrice>().Update(PriceUpdated);
                await _unitOfWork.CompletedAsynk();
                return Ok(PriceUpdated);
            }
            else // Add ===> 
            {
                var priceAdded=  await _unitOfWork.Repository<DollarPrice>().AddAsync(new DollarPrice() { Price = dollarPrice});
                await _unitOfWork.CompletedAsynk();
                return Ok(priceAdded);
            }     

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(DollarPrice), 200)]
        [HttpGet("GetDollarPrice")]
        public async Task<ActionResult> GetDollarPrice()
        {
            var price = await _unitOfWork.Repository<DollarPrice>().GetAllAsync();
            if(price != null)
            {
                return Ok(price.FirstOrDefault());
            }
            return Ok(new DollarPrice() { Price = 50});
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<PackagesDto>), 200)]
        [ProducesResponseType(200)]
        [HttpGet("GetAllPackages")]
        public async Task<ActionResult<IEnumerable<PackagesDto>>> GetAllPackages()
        {
            var priceDollar = await _unitOfWork.Repository<DollarPrice>().GetAllAsync();
            if (priceDollar == null)
                priceDollar.FirstOrDefault().Price = 50;

            var packages = await _unitOfWork.Repository<Packages>().GetAllAsync();
           var packagesDto =  packages.Select(p=>new PackagesDto() {Id=p.Id, CountFoDollar = p.CountFoDollar , PriceEgyptian = priceDollar.FirstOrDefault().Price * p.CountFoDollar });
            return Ok(packagesDto);

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = CsRoles.Admin)]
        [ProducesResponseType(typeof(PackagesDto), 200)]
        [ProducesResponseType(200)]
        [HttpPost("AddPackage")]
        public async Task<ActionResult<PackagesDto>> AddPackage(int countFoDollar)
        {        
            var package = new Packages() { CountFoDollar = countFoDollar };
            var packageAdded= await _unitOfWork.Repository<Packages>().AddAsync(package);
            await _unitOfWork.CompletedAsynk();

            var price = await _unitOfWork.Repository<DollarPrice>().GetAllAsync();
            var PriceDoller = 50;
            if (price != null)
            {
                PriceDoller=price.FirstOrDefault().Price;
            }

            return Ok(new PackagesDto() { CountFoDollar = packageAdded.CountFoDollar , PriceEgyptian= packageAdded.CountFoDollar * PriceDoller , Id=packageAdded.Id});
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.Admin)]
        [ProducesResponseType(200)]
        [HttpDelete("DeletePackage")]
        public async Task<ActionResult> DeletePackage(int id)
        {
            var package = await _unitOfWork.Repository<Packages>().GetByIdAsync(id);
            if (package != null)
            {
                _unitOfWork.Repository<Packages>().Delete(package);
                await _unitOfWork.CompletedAsynk();
                return Ok();
            }

            return BadRequest(new ApiResponse(404, "معرف غير صحيح"));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.Admin},{CsRoles.Supervisor}")]
        [ProducesResponseType(typeof(YoutubeVideo), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpPost("AddYoutubeVideo")]
        public async Task<ActionResult<YoutubeVideo>> AddYoutubeVideo(YoutubeVideo youtubeVideo)
        {
            youtubeVideo.Id = 0;
            var VideoAdded = await _unitOfWork.Repository<YoutubeVideo>().AddAsync(youtubeVideo);
            await _unitOfWork.CompletedAsynk();
            return Ok(VideoAdded);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.Admin},{CsRoles.Supervisor}")]
        [ProducesResponseType(typeof(YoutubeVideo), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpPut("UpdateYoutubeVideo")]
        public async Task<ActionResult<YoutubeVideo>> UpdateYoutubeVideo(YoutubeVideo youtubeVideo)
        {
            var video = await _unitOfWork.Repository<YoutubeVideo>().GetByIdAsync(youtubeVideo.Id);
            if (video == null)
                return BadRequest(new ApiResponse(404, "معرف غير موجود"));
             
            video.DescriptionVideo = youtubeVideo.DescriptionVideo;
            video.UrlVideo = youtubeVideo.UrlVideo;
            video.VideoName = youtubeVideo.VideoName;
             _unitOfWork.Repository<YoutubeVideo>().Update(video);
            await _unitOfWork.CompletedAsynk();
            return Ok(youtubeVideo);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.Admin},{CsRoles.Supervisor}")]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(200)]
        [HttpDelete("DeleteYoutubeVideo")]
        public async Task<ActionResult<YoutubeVideo>> DeleteYoutubeVideo(int id)
        {
   
            var Video = await _unitOfWork.Repository<YoutubeVideo>().GetByIdAsync(id);
            if (Video == null)
                return BadRequest(new ApiResponse(404, "معرف غير موجود"));
            _unitOfWork.Repository<YoutubeVideo>().Delete(Video);
            await _unitOfWork.CompletedAsynk();
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(200)]
        [HttpGet("GetByIdYoutubeVideo")]
        public async Task<ActionResult<YoutubeVideo>> GetByIdYoutubeVideo(int id)
        {

            var Video = await _unitOfWork.Repository<YoutubeVideo>().GetByIdAsync(id);
            if (Video == null)
                return BadRequest(new ApiResponse(404, "معرف غير موجود"));

            return Ok(Video);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(IEnumerable<YoutubeVideo>),200)]
        [HttpGet("GetAllYoutubeVideo")]
        public async Task<ActionResult<IEnumerable<YoutubeVideo>>> GetAllYoutubeVideo()
        {
            var Videos = await _unitOfWork.Repository<YoutubeVideo>().GetAllAsync();

            return Ok(Videos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(IEnumerable<YoutubeVideo>), 200)]
        [HttpGet("GetByVideoName")]
        public async Task<ActionResult<YoutubeVideo>> GetByVideoName(string VideoName)
        {
            var spec = new YoutubeVideoSpesification(VideoName);
            var video = await _unitOfWork.Repository<YoutubeVideo>().GetByIdWithSpesificationAsync(spec);
            if(video == null)
                return BadRequest(new ApiResponse(404, "الاسم غير موجود"));

            return Ok(video);
        }

    }

}
