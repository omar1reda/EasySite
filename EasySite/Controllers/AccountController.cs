using Core.Entites;
using EasySite.Core.Entites;
using EasySite.Core.I_Repository;
using EasySite.DTOs;
using EasySite.Helper.SendEmail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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


        public AccountController(IHttpContextAccessor httpContextAccessor , UserManager<AppUser> userManager , SignInManager<AppUser> signInManager, IMailSettings mailSettings , ITokenService tokenService )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._mailSettings = mailSettings;
            this._tokenService = tokenService;
     
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto Model )
        {

            string userIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string userAgent = Request.Headers["User-Agent"];
            string userMacAddress = Request.Headers["User-Agent"].ToString();

            var user = await _userManager.FindByEmailAsync(Model.Email);


            if (user != null)
            {
                
                 var result= await _signInManager.CheckPasswordSignInAsync(user, Model.Password,false);              
                if(result.Succeeded)
                {
                    
                    var UserDTO = new UserDto()
                    {
                        Email = user.Email,
                        UserName = user.UserName,
                        Type = user.Type,
                        AmountDue = user.AmountDue,
                        AmountPaid = user.AmountPaid,
                        Verification=user.Verification,
                        Token = await _tokenService.GetToken(user)
                    };

                    return Ok(UserDTO);
                }
                else 
                {
                    return Unauthorized(); 
                }
            }
            else
            { 
                return Unauthorized();
            }
           
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {

          var user=  await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {

                var Usedto = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0]

                };

                var Result = await _userManager.CreateAsync(Usedto, model.Password);
                if(Result.Succeeded)
                {                  
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
                return BadRequest();
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

        [HttpPost("CheckVerificationCode")]
        public async Task<ActionResult<UserDto>> CheckVerificationCode(VerificationDto verificationDto)
        {
            var userData = await _userManager.FindByEmailAsync(verificationDto.Email);

                if (userData.VerificationCode == verificationDto.VerificationCode)
                {
                        userData.Verification = true;
                        await _userManager.UpdateAsync(userData);

                        var UserDTO = new UserDto()
                        {
                            Email = userData.Email,
                            UserName = userData.UserName,
                            Type = userData.Type,
                            AmountDue = userData.AmountDue,
                            AmountPaid = userData.AmountPaid,
                            Verification = userData.Verification,
                            Token = await _tokenService.GetToken(userData)
                        };
                        return Ok(UserDTO);
                }
                else
                {
                    return BadRequest();
                }         
                   

        }

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
            { return BadRequest(); }
        }


        [HttpPost("ResetPasswordEnterCode")]
        public async Task<IActionResult> ResetPasswordEnterCode(VerificationDto verificationDto)
        {
            var userData = await _userManager.FindByEmailAsync(verificationDto.Email);

            if (userData.VerificationCode == verificationDto.VerificationCode)
            {             
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

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


                var UserDTO = new UserDto()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Type = user.Type,
                    AmountDue = user.AmountDue,
                    AmountPaid = user.AmountPaid,
                    Verification = user.Verification,
                    Token = await _tokenService.GetToken(user)
                };
                return Ok(UserDTO);
            }
            else { return BadRequest(); }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateEmail")]
        public async Task<ActionResult> UpdateEmail(UpdateEmailDto model)
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
                        return Ok();
                    }
                    else/// الاميل موجود بالفعل
                    {
                        return BadRequest();
                    }
;
                }
                else {
                    
                    return BadRequest();
                } 
              
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdatePassword")]
        public async Task<ActionResult> UpdatePassword( UpdatePasswordDto passwordDto)
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
                                return Ok();
                            }
                            else { return BadRequest(); }
                        }
                        else { return BadRequest(); }
                    }
                    else { return BadRequest(); }
                }
                else { return BadRequest(); }
            }
            else { return BadRequest(); }

        }

    }
}
