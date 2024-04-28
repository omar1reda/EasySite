using EasySite.Core.Entites;
using EasySite.Core.Entites.SignalR;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs.productDTO.ToReturn;
using EasySite.DTOs.SignalRDto;
using EasySite.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public SignalRController(IUnitOfWork unitOfWork , UserManager<AppUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        [HttpGet("GetAllGroups")]
        [ProducesResponseType(typeof(IEnumerable<GroupDto>), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetAllGroups()
        {
              var email = User.FindFirst(ClaimTypes.Email)?.Value;
              var user = await _userManager.FindByEmailAsync(email);
            /// == >   ترتيب الجروبات حسب اخر رساله جت ف الجروب
            var UserGroupsDto = user.UsersGroups.Select(g => g.Group).OrderBy(o=>o.Messages.OrderByDescending(m=>m.Date).FirstOrDefault().Date)
                .Select(g=>new GroupDto{ Id = g.Id, name= g.Name});

            return Ok(UserGroupsDto);
        }


        [HttpGet("GetAllMessagesInGroup")]
        [ProducesResponseType(typeof(IEnumerable<MessageDto>), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetAllMessagesInGroup(int groupId)
        {
            var SpecMessages = new MessagsSpesification(groupId);
            var Messages = await _unitOfWork.Repository<Message>().GetAllWithSpesificationAsync(SpecMessages);

            Messages = Messages.OrderBy(m => m.Date);

            var MessagesDto = Messages.Select(m =>
                             new MessageDto {
                                 Id = m.Id,
                                 Date = m.Date,
                                 messageText = m.messageText,
                                 messageType = m.messageType.ToString() ,
                                 UserName= m.AppUser.UserName ,
                                 statusMessage = m.statusMessage.ToString(),
                                 Role = _userManager.GetRolesAsync(m.AppUser).Result.FirstOrDefault()
                             });

            return Ok(MessagesDto);
        }
    }
}
