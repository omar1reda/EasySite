using AutoMapper;
using Core.Entites;
using EasySite.Core.Entites;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs;
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
    public class TransactionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public TransactionsController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        //[ProducesResponseType(typeof(ApiResponse), 400)]
        //[ProducesResponseType(typeof(TransactionDto), 200)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        //[HttpPost("AddTransaction")]
        //public async Task<ActionResult<TransactionDto>> AddTransaction(TransactionDto transactionDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var Email = User.FindFirstValue(ClaimTypes.Email);
        //        var user = await _userManager.FindByEmailAsync(Email);
        //        if (user.Id == transactionDto.AppUserId)
        //        {
        //            var transactionMapped = _mapper.Map<TransactionDto, Transactions>(transactionDto);
        //            transactionMapped.TransactionTime = DateTime.Now;
        //            await _unitOfWork.Repository<Transactions>().AddAsync(transactionMapped);
        //            await _unitOfWork.CompletedAsynk();


        //            var trancactionMapp = _mapper.Map<Transactions, TransactionAllDataDto>(transactionMapped);
        //            return Ok(trancactionMapp);
        //        }
        //        return BadRequest(new ApiResponse(400, "المعرف غير صحيح"));

        //    }
        //    else
        //    {
        //        return BadRequest(new ApiResponse(400, "البيانات غير مكتمله"));
        //    }

        //}


        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(TransactionDto), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpGet("GetAllTransactionByUserId")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllTransactionByUserId(string UserId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Role = User.FindFirstValue(ClaimTypes.Role);

            var user =await _userManager.FindByEmailAsync(Email);

                if(Role == CsRoles.User)
                {
                    var spec = new TransactionSpesification(user.Id);
                    var Transaction = await _unitOfWork.Repository<Transactions>().GetAllWithSpesificationAsync(spec);

                    var TransactionMapped = _mapper.Map<IEnumerable<Transactions>, IEnumerable<TransactionAllDataDto>>(Transaction);
                    return Ok(TransactionMapped);
                }
                else if(Role == CsRoles.Manager)
                {
                    var spec = new TransactionSpesification(user.MangerId);
                    var Transaction = await _unitOfWork.Repository<Transactions>().GetAllWithSpesificationAsync(spec);

                    var TransactionMapped = _mapper.Map<IEnumerable<Transactions>, IEnumerable<TransactionAllDataDto>>(Transaction);
                    return Ok(TransactionMapped);
                }
                
          
            else
                return NotFound(new ApiResponse(400, "المعرف غير موجود"));

        }




    }
}
