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
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public PagesController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {

            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        [HttpPost("AddPage")]
        [ProducesResponseType(typeof(IEnumerable<PageToReturnDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<PageToReturnDto>>> AddPage(PageDto pageDto)
        {
            if(ModelState.IsValid)
            {
                var page = _mapper.Map<PageDto, Pages>(pageDto);
                await _unitOfWork.Repository<Pages>().AddAsync(page);
                await _unitOfWork.CompletedAsynk();

                var spec = new PagesSpesification(pageDto.SiteId);
                var pages=   await _unitOfWork.Repository<Pages>().GetAllWithSpesificationAsync(spec);
                var pagesMapped = _mapper.Map<IEnumerable< Pages>,IEnumerable< PageToReturnDto>>(pages);
                return Ok(pagesMapped);
            }
            return BadRequest(new ApiResponse(404, "البيانات غير مكتمله"));
        }



        [HttpGet("GetAllPages")]
        [ProducesResponseType(typeof(IEnumerable<PageToReturnDto>), 200)]
        public async Task<ActionResult<IEnumerable<PageToReturnDto>>> GetAllPages(int siteId)
        {
          
                var spec = new PagesSpesification(siteId);
                var pages = await _unitOfWork.Repository<Pages>().GetAllWithSpesificationAsync(spec);
                var pagesMapped = _mapper.Map<IEnumerable<Pages>, IEnumerable<PageToReturnDto>>(pages);
                 return Ok(pagesMapped);
        }




        [ProducesResponseType(typeof(IEnumerable<PageToReturnDto>), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpDelete("DeletePage")]
        public async Task<ActionResult<IEnumerable<BlockedNumbersToReturnDto>>> DeletePage(int PageId)
        {
            var page = await _unitOfWork.Repository<Pages>().GetByIdAsync(PageId);
            if (page != null)
            {
                _unitOfWork.Repository<Pages>().Delete(page);
                await _unitOfWork.CompletedAsynk();

                var spec = new PagesSpesification(page.SiteId);
                var pages = await _unitOfWork.Repository<Pages>().GetAllWithSpesificationAsync(spec);

                var pagesToReturn = _mapper.Map<IEnumerable<Pages>, IEnumerable<PageToReturnDto>>(pages);
                return Ok(pagesToReturn);
            }
            else
            {
                return NotFound(new ApiResponse(400, "معرف غير صحيح"));
            }


        }


        [HttpPut("UpdatePage")]
        [ProducesResponseType(typeof(PageToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult<PageToReturnDto>> UpdatePage(PageToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                var page = _mapper.Map<PageToReturnDto, Pages>(model);
                 _unitOfWork.Repository<Pages>().Update(page);
                await _unitOfWork.CompletedAsynk();

                var spec = new PagesSpesification(model.SiteId);
                var pages = await _unitOfWork.Repository<Pages>().GetByIdAsync(model.Id );
                var pageMapped = _mapper.Map<Pages, PageToReturnDto>(pages);
                return Ok(pageMapped);
            }
            return BadRequest(new ApiResponse(404, "البيانات غير مكتمله"));
        }


        [HttpGet("GetPageById")]
        [ProducesResponseType(typeof(IEnumerable<PageToReturnDto>), 200)]
        public async Task<ActionResult<IEnumerable<PageToReturnDto>>> GetPageById(int pageId)
        {
            var page = await _unitOfWork.Repository<Pages>().GetByIdAsync(pageId);
            var pageMapped = _mapper.Map<Pages, PageToReturnDto>(page);
            return Ok(pageMapped);
        }
    }
}
