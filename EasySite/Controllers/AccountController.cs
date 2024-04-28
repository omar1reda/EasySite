using AutoMapper;
using Core.Entites;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Enums;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs;
using EasySite.DTOs.ManagrDto;
using EasySite.DTOs.User;
using EasySite.Errors;
using EasySite.Helper.SendEmail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailSettings _mailSettings;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IHttpContextAccessor httpContextAccessor , UserManager<AppUser> userManager , SignInManager<AppUser> signInManager, IMailSettings mailSettings , ITokenService tokenService , IMapper mapper  , IUnitOfWork unitOfWork)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._mailSettings = mailSettings;
            this._tokenService = tokenService;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<UserDto>> Login(LoginDto Model )
        {

            //string userIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            //string userAgent = Request.Headers["User-Agent"];
            //string userMacAddress = Request.Headers["User-Agent"].ToString();

            var userByEmail = await _userManager.FindByEmailAsync(Model.EmailOrUserName);
            var userByUserName = await _userManager.FindByNameAsync(Model.EmailOrUserName);

            if (userByEmail != null)
            {

                var signInResult = await _signInManager.CheckPasswordSignInAsync(userByEmail, Model.Password,false);
                if (signInResult.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(userByUserName);
                    if (role.FirstOrDefault() == CsRoles.Manager)
                    {
                        var user = await _userManager.FindByIdAsync(userByUserName.MangerId);
                        if (user != null)
                        {
                            userByEmail.AmountDue = user.AmountDue;
                            userByEmail.YourAmount = user.YourAmount;
                            userByEmail.UserType = user.UserType;
                            userByEmail.IsActive = user.IsActive;
                            userByEmail.freeTrial = user.freeTrial;
                            userByEmail.Verification = user.Verification;
                            userByEmail.EndDate = user.EndDate;
                            userByEmail.StartDate = user.StartDate;
                            userByEmail.AccountLockMessage = user.AccountLockMessage;
                            userByEmail.Duration = user.Duration;

                        }
                    }

                    var userDto = await CreateUserDto(userByEmail);
                    return Ok(userDto);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, $"الباسورد غير صحيح"));
                }

            }
            else if (userByUserName!=null)
            {
              var  signInResult = await _signInManager.CheckPasswordSignInAsync(userByUserName, Model.Password, false);
                if (signInResult.Succeeded)
                {
                    var role = await _userManager.GetRolesAsync(userByUserName);
                    if (role.FirstOrDefault() == CsRoles.Manager) 
                    {
                        var user = await _userManager.FindByIdAsync(userByUserName.MangerId);
                        if (user != null)
                        {
                            userByUserName.AmountDue = user.AmountDue;
                            userByUserName.YourAmount=user.YourAmount;
                            userByUserName.UserType = user.UserType;
                            userByUserName.IsActive= user.IsActive;
                            userByUserName.freeTrial = user.freeTrial;
                            userByUserName.Verification = user.Verification;
                            userByUserName.EndDate = user.EndDate;
                            userByUserName.StartDate = user.StartDate;
                            userByUserName.AccountLockMessage = user.AccountLockMessage;
                            
                        }
                    }
                    var userDto = await CreateUserDto(userByUserName);
                    return Ok(userDto);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, $"الباسورد غير صحيح"));
                }
            }
            else
            { 
                return NotFound(new ApiResponse(404, $" {Model.EmailOrUserName} غير صحيح"));
            }



        }



        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDto model)
        {

          var user=  await _userManager.FindByEmailAsync(model.Email);
            var userByUserName = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
               if (userByUserName != null) return BadRequest(new ApiResponse(400, $"يوزر موجود بالفعل"));

                var Usedto = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                };
                if(model.MarketerId != null)
                {
                    Usedto.MangerId = model.MarketerId;
                }
                var Result = await _userManager.CreateAsync(Usedto, model.Password);
                if(Result.Succeeded)
                {
                    var userAdded = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(userAdded, CsRoles.User);
                    await SendCode(model.Email );
                    return Ok();

                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest(new ApiResponse(400, $"الايميل موجود بالفعل"));
            }
        }

        [HttpPost("SendCode")]
        public async Task SendCode(string Email )
        {
            Random random = new Random();
            var randomNumber = random.Next(100000, 999999);

            var email = new Email()
            {
                Subject = "رمز التحقق الخاص بك - Your verification code",
                Body = "رمز التحقق الخاص بك هو " + randomNumber,
                To = Email
            };
            _mailSettings.SendEmail(email);
            var user = await _userManager.FindByEmailAsync(Email);
            user.VerificationCode = randomNumber;
            await _userManager.UpdateAsync(user); 
        }


        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpPost("CheckVerificationCode")]
        public async Task<ActionResult<UserDto>> CheckVerificationCode(VerificationDto verificationDto)
        {
            var userData = await _userManager.FindByEmailAsync(verificationDto.Email);

                if (userData.VerificationCode == verificationDto.VerificationCode)
                {
                        userData.Verification = true;
                        await _userManager.UpdateAsync(userData);

                       
                        var userDto = await CreateUserDto(userData);
                        return Ok(userDto);
                }
                else
                {
                return BadRequest(new ApiResponse(400, $"رمز التحقق غير صحيح"));
            }         
                   

        }


        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpPost("ResetPasswordEnterEmail")]
        public async Task<IActionResult> ResetPasswordEnterEmail (string Email)
        {

            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                await SendCode(Email);
                return Ok();
            }
            else
            { return BadRequest(new ApiResponse(400, $"الايميل غير موجود")); }
        }


        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpPost("ResetPasswordEnterCode")]
        public async Task<ActionResult> ResetPasswordEnterCode(VerificationDto verificationDto)
        {
            var userData = await _userManager.FindByEmailAsync(verificationDto.Email);

            if (userData.VerificationCode == verificationDto.VerificationCode)
            {             
                return Ok();
            }
            else
            {
                return BadRequest(new ApiResponse(400, $"كود غير صحيح"));
            }
        }


        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpPut("ResetPasswordEnterNewPassword")]
        public async Task<ActionResult<UserDto>> ResetPasswordEnterNewPassword(RegisterDto registerDto)
        {
            var user =await _userManager.FindByEmailAsync(registerDto.Email);

            if (registerDto.Password == registerDto.RePassword )
            {

                var result = await _userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddPasswordAsync(user, registerDto.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateAsync(user);
                    }
                }

                var userDto = await CreateUserDto(user);
                return Ok(userDto);
            }
            else { return BadRequest(new ApiResponse(400, $"كلمتان المرور غير متطابقتان")); }
        }



        [ProducesResponseType( 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.User)]
        [HttpPut("UpdateEmail")]
        public async Task<ActionResult<UserDto>> UpdateEmail(UpdateEmailDto model)
        {
            var User = await _userManager.FindByIdAsync(model.UserId);

            if (User != null)
            {
                var resultChek = await _signInManager.CheckPasswordSignInAsync(User, model.Password,false);
                if (resultChek.Succeeded)
                {
                    var ChekEmail = await _userManager.FindByEmailAsync(model.NewEmail);
                    if(ChekEmail == null)
                    {
                        User.Email = model.NewEmail;
                        var result = await _userManager.UpdateAsync(User);
                        var userDto = await CreateUserDto(User);
                        return Ok(userDto);
                    }
                    else/// الاميل موجود بالفعل
                    {
                        return BadRequest(new ApiResponse(400, $"الايمل موجود بالفعل"));
                    }
;
                }
                else {

                    return BadRequest(new ApiResponse(400, $"باسورد غير صحيح"));
                } 
              
            }
            else
            {
                return BadRequest(new ApiResponse(400, $"معرف غير صحيح"));
            }
        }

        [ProducesResponseType( typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =CsRoles.User)]
        [HttpPut("UpdatePassword")]
        public async Task<ActionResult<UserDto>> UpdatePassword( UpdatePasswordDto passwordDto)
        {
            var user =await _userManager.FindByIdAsync(passwordDto.UserId);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, passwordDto.Password, false);
                if (result.Succeeded)
                {
                   if (passwordDto.NewPassword == passwordDto.NewRePassword)
                    {
                        var Remove = await _userManager.RemovePasswordAsync(user);
                        if (Remove.Succeeded)
                        {
                           var Addresult = await _userManager.AddPasswordAsync(user, passwordDto.NewPassword);
                            if (Addresult.Succeeded)
                            {
                                await _userManager.UpdateAsync(user);
                               var userDto= await CreateUserDto(user);
                                return Ok(userDto);
                            }
                            else { return BadRequest(); }
                        }
                        else { return BadRequest(); }
                    }
                    else { return BadRequest(new ApiResponse(400, $"كلمتان المرور غير متطابقتان")); }
                }
                else { return BadRequest(new ApiResponse(400, $"باسورد غير صحيح")); }
            }
            else { return BadRequest(new ApiResponse(400, $"معرف غير صحيح")); }

        }


        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.User)]
        [HttpGet("FreeTrial")]
        public async Task<ActionResult<UserDto>> freeTrial(string UserId)
        {
            var user =await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                if(user.freeTrial == FreeTrial.DidntStart)
                {
                    user.StartDate = DateTime.Now;
                    user.Duration = 10;
                    user.EndDate = DateTime.Now.AddDays((int) user.Duration);
                    
                    user.UserType = TypeUser.FreeTrial;
                    user.freeTrial = FreeTrial.Active;
                    await _userManager.UpdateAsync(user);
                    
                  
                  var userDto=  await CreateUserDto(user);

                    return Ok(userDto);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "المستخدم اخذ فتره مجانيه مسبقا"));
                }

            }
            else
            {
                return NotFound( new ApiResponse(404 , "معرف غير صحيح"));
            }
           
           
        }


        [ProducesResponseType(typeof(UserDto), 200)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("CheckEndFreeTrial")]
        public async Task<ActionResult<UserDto>> CheckEndFreeTrial(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            while (DateTime.Now < user.EndDate)
            {
                // انتظار 6 ساعات قبل التحقق مرة أخرى
                System.Threading.Thread.Sleep(20000000);
            };
            user.Sites.Select(s=>s.IsActive= false).ToList();
            user.UserType = TypeUser.Basic;
            user.freeTrial = FreeTrial.Finish;
            await _userManager.UpdateAsync(user);

            var UserDto= await CreateUserDto(user);
            return Ok(UserDto);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.User)]
        [HttpGet("GetYourCommission")]
        public async Task<ActionResult<CommissionDto>> GetYourCommission() 
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var commissionDto = new CommissionDto()
            {
                Commission = user.Commission
            };
            var UsersReferralsDto = _userManager.Users.Where(u => u.MarketerId == user.Id).Select(z =>  new ReferralsDto()
            {
                DateCreated=(DateTime) z.DateCreated,
                UserCommission = z.AllAmountPaid/5,
                 UserName=z.UserName
            });

            commissionDto.ReferralsesDto = UsersReferralsDto.ToList();

            return Ok(commissionDto);
        }

        [HttpGet]
        private async Task<UserDto> CreateUserDto(AppUser user)
        {
            var UserDTO = new UserDto()
            {
                Id = user.Id,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                Email = user.Email,
                UserName = user.UserName,
                Type = user.UserType.ToString(),
                AmountDue = user.AmountDue,
                YourAmount = user.YourAmount,
                Verification = user.Verification,
                Token = await _tokenService.GetToken(user),
                freeTrial = user.freeTrial.ToString(),
                DateCreated = user.DateCreated,
                IsActive = user.IsActive,
                AccountLockMessage = user.AccountLockMessage,
                StartDate = user.StartDate,
                EndDate = user.EndDate,
                Duration = user.Duration,
            };
            return UserDTO;
        }


        [ProducesResponseType(typeof(managerPermitionsToReturn), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpPost("AddManger")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.User)]
        public async Task<ActionResult<managerPermitionsToReturn>> AddManger(ManagerPermitionsDto managerPermitionsDto)
        {
            var mangerByUserName = await _userManager.FindByNameAsync(managerPermitionsDto.UserName);
            var mangerByEmail = await _userManager.FindByEmailAsync(managerPermitionsDto.Email);
            var userDataBase =await _userManager.FindByIdAsync(managerPermitionsDto.UserId);
            if (mangerByUserName != null)
                return BadRequest(new ApiResponse(400, "مستخدم من قبل UserName ال"));

            if (mangerByEmail != null)
                return BadRequest(new ApiResponse(400, "مستخدم من قبل Email ال"));
            if (userDataBase != null)
            {
                if(userDataBase.UserType != TypeUser.Pro)
                {
                    return BadRequest(new ApiResponse(400, "  Pro لا يمكن استخدام هذه الاضافه حتي تقوم بالترقيه الي النسخه ال "));
                }


                var user = new AppUser()
                {
                    Email = managerPermitionsDto.Email,
                    UserName = managerPermitionsDto.UserName,
                    Verification = true,
                    DateCreated = DateTime.Now,
                    MangerId = managerPermitionsDto.UserId
                };
                var UserAdded = await _userManager.CreateAsync(user, managerPermitionsDto.Password);
                if (UserAdded.Succeeded)
                {
                    var manger = await _userManager.FindByNameAsync(managerPermitionsDto.UserName);
                    await _userManager.AddToRoleAsync(manger, CsRoles.Manager);
                    
                    var Premation = _mapper.Map<PermitionsDto, Permitions>(managerPermitionsDto.PermitionsDto);
                    Premation.Id = 0;
                    Premation.AppUserId = manger.Id;
                    var PremationAdded = await _unitOfWork.Repository<Permitions>().AddAsync(Premation);
                    await _unitOfWork.CompletedAsynk();


                    var PerimationMapper = _mapper.Map<Permitions, PermitionsDto>(PremationAdded);
                   var MangerPerimationMapper = new managerPermitionsToReturn()
                    {
                        Id=manger.Id,
                        UserName = manger.UserName,
                        Email = manger.Email,
                        DateCreated = (DateTime)manger.DateCreated,
                        PermitionsDto=PerimationMapper
                    };
                    return Ok(MangerPerimationMapper);
                }

                return BadRequest(new ApiResponse(400, $"{UserAdded.Errors}"));

            }

            return BadRequest(new ApiResponse(400, "معرف غير صحيح"));


        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpDelete("DeleteManger")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.User)]
        public async Task<ActionResult> DeleteManger(string ManagerId)
        {
            var UserManager = await _userManager.FindByIdAsync(ManagerId);
            if (UserManager != null)
            {
                await _userManager.DeleteAsync(UserManager);
                return Ok();
            }
            return BadRequest(new ApiResponse(400, "معرف غير موجود"));

        }


        [ProducesResponseType( typeof( IEnumerable<managerPermitionsToReturn>),200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpGet("GetAllManger")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.User)]
        public async Task<ActionResult<IEnumerable<managerPermitionsToReturn>>> GetAllManger(string UserId)
        {
            var userSpec = new AppUserSpesification(UserId);
            var UsersManager = await _unitOfWork.Repository<AppUser>().GetAllWithSpesificationAsync(userSpec);
            if (UsersManager != null)
            {
                  var UserMangerToReturn = UsersManager.Select(u =>

                  new managerPermitionsToReturn()
                  {
                      Id = u.Id,
                      DateCreated = (DateTime)u.DateCreated,
                      Email = u.Email,
                      UserName = u.UserName,
                      PermitionsDto = _mapper.Map<Permitions, PermitionsDto>(u.Permition)
                  });
                return Ok(UserMangerToReturn);
            }

            return BadRequest(new ApiResponse(400, "معرف غير موجود"));

        }


        [ProducesResponseType(typeof(managerPermitionsToReturn), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [HttpPut("UpdatePermitions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = CsRoles.User)]
        public async Task<ActionResult<PermitionsDto>> UpdatePermitions(managerPermitionsToReturn managerPermitionsReturn)
        {
            var manger = await _userManager.FindByIdAsync(managerPermitionsReturn.Id);
            if(manger.Email != managerPermitionsReturn.Email)
            {
                var mangerByEmail = await _userManager.FindByEmailAsync(managerPermitionsReturn.Email);
                if(mangerByEmail != null)
                {
                    return BadRequest(new ApiResponse(400, "مستخدم من قبل Email ال"));
                }
            }

            
            if (manger.UserName != managerPermitionsReturn.UserName)
            {
                var mangerByUserName = await _userManager.FindByNameAsync(managerPermitionsReturn.UserName);
                if (mangerByUserName != null)
                {
                    return BadRequest(new ApiResponse(400, "مستخدم من قبل UserName ال"));
                }
            }

            manger.Email = managerPermitionsReturn.Email;
            manger.UserName = managerPermitionsReturn.UserName;
            if (manger != null)
            {
                manger.Permition = _mapper.Map<PermitionsDto, Permitions>(managerPermitionsReturn.PermitionsDto);
                await _userManager.UpdateAsync(manger);
                await _unitOfWork.CompletedAsynk();

                return Ok(managerPermitionsReturn);
            }
            return BadRequest(new ApiResponse(400 , "معرف غير صحيح"));

        }


   
    }
}
