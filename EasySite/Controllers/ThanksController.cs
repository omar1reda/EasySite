using AutoMapper;
using Core.Entites;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs;
using EasySite.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ThanksController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }


        [ProducesResponseType(typeof(ThanksDto), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPut("AddOrUpdateThanks")]
        public async Task<ActionResult<ThanksDto>> AddOrUpdateThanks(ThanksDto thanksDto)
        {

            var spec = new ThanksSpesification(thanksDto.SiteId);
            var count = await _unitOfWork.Repository<Thanks>().GetCountByIdWithSpesificationAsync(spec);

            var thanksMapped = _mapper.Map<ThanksDto, Thanks>(thanksDto);
            if (count == 0)
            {

                await _unitOfWork.Repository<Thanks>().AddAsync(thanksMapped);
            }
            else if (count == 1)
            {
                var thanks = await _unitOfWork.Repository<Thanks>().GetByIdWithSpesificationAsync(spec);
                thanks.IsActive = thanksDto.IsActive;
                thanks.Title = thanksDto.Title;
                thanks.Description = thanksDto.Description;
                thanks.IsbuttonToHome = thanksDto.IsbuttonToHome;
                thanks.ShowDepartment = thanksDto.ShowDepartment;




                _unitOfWork.Repository<Thanks>().Update(thanks);
            }
            await _unitOfWork.CompletedAsynk();



            return Ok(thanksDto);

        }


        [ProducesResponseType(typeof(ThanksDto), 200)]
        [HttpGet("GetThanksBySiteId")]
        public async Task<ActionResult<ThanksDto>> GetThanksBySiteId(int SiteId)
        {

            var spec = new ThanksSpesification(SiteId);
            var Thanks = await _unitOfWork.Repository<Thanks>().GetByIdWithSpesificationAsync(spec);
            if (Thanks != null)
            {
                var ThanksMapped = _mapper.Map<Thanks, ThanksDto>(Thanks);
                return Ok(ThanksMapped);
            }
            else
                return NotFound(new ApiResponse(404, "المعرف غير موجود"));
        }
    }
}
