using EasySite.Core.Entites;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.SignalR;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs;
using EasySite.DTOs.SignalRDto;
using EasySite.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EasySite.SignalR
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub:Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public ChatHub(IUnitOfWork  unitOfWork , UserManager<AppUser> userManager , IConfiguration configuration)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task CreateGroup(string UserId)
        {
            var EmailUser = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
            var userFromDataBase = await _userManager.FindByEmailAsync(EmailUser);
            // ==>   لو اللي بيعمل الشات هو الادمن
            if (UserId != null)
            {
                userFromDataBase = await _userManager.FindByIdAsync(UserId);
            }

            var groupName = userFromDataBase.UserName;

            var roleUser = await _userManager.GetRolesAsync(userFromDataBase);
            // ==>  لبتاع اليوزر userName تغير ال  Supervisor لو المستخدم 
            if (roleUser.FirstOrDefault() == CsRoles.Supervisor)
            {
                var user = await _userManager.FindByIdAsync(userFromDataBase.MarketerId);
                groupName= user.UserName;
            }

            var specGroup = new GroupSpecification(groupName,0);
            var group= await _unitOfWork.Repository<Group>().GetByIdWithSpesificationAsync(specGroup);

            if(group == null)
            {
                var GroupAdded = await _unitOfWork.Repository<Group>().AddAsync(new Group() { Name = groupName });
                await _unitOfWork.CompletedAsynk();

                var AllUsers = _userManager.Users;

                foreach (var Us in AllUsers)
                {
                    var Allrole = await _userManager.GetRolesAsync(Us);
                    var role = Allrole.FirstOrDefault();
                    if (role == CsRoles.Admin || role == CsRoles.Manager || Us.Id == userFromDataBase.Id)
                    {
                        // ==> فالجروب Admins  بتضيف المستخدم وكل ال
                        await _unitOfWork.Repository<UsersGroups>().AddAsync(new UsersGroups() { GroupId = GroupAdded.Id, AppUserId = Us.Id });
                        await _unitOfWork.CompletedAsynk();
                        /// ===> ConnictionId لو اي مستخدم منهم فاتح نت تضيفه الي الجروب عن طريق ال
                        if (Us.ConnictionId_SignalR != null)
                        {
                            await Groups.AddToGroupAsync(Us.ConnictionId_SignalR, GroupAdded.Name );
                        }
                    }
                }

                await Clients.OthersInGroup(groupName).SendAsync("ReceiveCreateGroup", groupName, userFromDataBase.UserName, GroupAdded.Id);
            }  
            else //   لو الجروب موجود بالفعل
            {
                // ==> فالجروب Admins  بتضيف المستخدم وكل ال
                await _unitOfWork.Repository<UsersGroups>().AddAsync(new UsersGroups() { GroupId = group.Id, AppUserId = userFromDataBase.Id });
                await _unitOfWork.CompletedAsynk();

                await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);

            }

        }

        public async Task SendMessageIngroup(string? message , IFormFile? image, int groupId)
        {
            var EmailUser = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
            var userFromDataBase = await _userManager.FindByEmailAsync(EmailUser);

            if (userFromDataBase != null)
            {
                
                var mess = new Message()
                {
                    messageText = message,
                    Date = DateTime.Now,
                    AppUserId = userFromDataBase.Id ,
                    statusMessage = StatusMessage.Been_Sent ,
                    GroupId = groupId 
                };

                if (image != null)
                {
                    mess.messageType = MessageType.File;
                    mess.messageText = SettingesImages.UplodeFile(image, "Images");
                }
                var messageAdded =  await _unitOfWork.Repository<Message>().AddAsync(mess);
                await _unitOfWork.CompletedAsynk();

                var MessageDto = new MessageDto()
                {
                   messageText= messageAdded.messageText,
                   Date= messageAdded.Date,
                   Id = messageAdded.Id,
                   UserName= userFromDataBase.UserName,
                   statusMessage = StatusMessage.Been_Sent.ToString(),
                   Role = _userManager.GetRolesAsync(userFromDataBase).Result.FirstOrDefault()
                };

                if(mess.messageType == MessageType.File)
                {
                    MessageDto.messageType = MessageType.File.ToString();
                    MessageDto.messageText = $"{_configuration["ApiBaseUrl"]}{MessageDto.messageText}";
                }

                var group = await _unitOfWork.Repository<Group>().GetByIdAsync(groupId);
                if(group != null) 
                {
                    await Clients.Group(group.Name).SendAsync("ReceiveMessageInGroup", MessageDto);
                }
                
            }
        }

        public async Task ChangeMessageStatusToWatched(string groupName)
        {
            var EmailUser = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
            var userFromDataBase = await _userManager.FindByEmailAsync(EmailUser);
            if(userFromDataBase != null)
            {
                // ===> تبليغ ان المستخدم شاهد الرساله
                await Clients.OthersInGroup(groupName).SendAsync("Receive_Been_Sent", StatusMessage.Been_Sent.ToString());

                // ===>   Was_Seen <== تغيير حالة الرساله الي 
                userFromDataBase.Messages = userFromDataBase.Messages.Select(m => { m.statusMessage = StatusMessage.Was_Seen; return m; }).ToList();
                await _userManager.UpdateAsync(userFromDataBase);

            }
        }

        public override async Task OnConnectedAsync()
        {
            var EmailUser = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(EmailUser);
            if (user != null)
            {
                // ===>   was delivered  <== تغيير حالة الرساله الي 
                user.Messages = user.Messages.Select(m => { m.statusMessage = StatusMessage.Was_Delivered; return m; }).ToList();

                user.ConnictionId_SignalR = Context.ConnectionId;
                await _userManager.UpdateAsync(user);

                var specGroups = new GroupSpecification(user.Id);
                var UserGroups = await _unitOfWork.Repository<Group>().GetAllWithSpesificationAsync(specGroups);

                foreach (var group in UserGroups)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
                    // ===> تبليغ ان المستخدم استلم الرساله
                    await Clients.OthersInGroup(group.Name).SendAsync("ReceiveDelivered" , StatusMessage.Was_Delivered.ToString() );

                    /// ==> المستخدم نشط
                    await Clients.OthersInGroup(group.Name).SendAsync("User_Active_Now", "Active now");
                }
            }

        }

        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var EmailUser = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
            var user =await _userManager.FindByEmailAsync(EmailUser);
            if (user!=null)
            {
                user.ConnictionId_SignalR = null;
               await _userManager.UpdateAsync(user);

                var specGroups = new GroupSpecification(user.Id);
                var UserGroups = await _unitOfWork.Repository<Group>().GetAllWithSpesificationAsync(specGroups);

                foreach (var group in UserGroups)
                {
                    /// ==> اخر ظهور للمستخدم
                    await Clients.OthersInGroup(group.Name).SendAsync("User_Last_Seen", DateTime.Now);
                }
            }
        }
    }
}
