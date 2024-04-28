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
    public class SocialMediaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SocialMediaController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(SocialMediaDto), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPut("AddOrUpdateSocialMedia")]
        public async Task<ActionResult<SocialMediaDto>> AddOrUpdateSocialMedia(SocialMediaDto socialMediaDto)
        {

            var spec = new SocialMediaDtoSpesification(socialMediaDto.SiteId);
            var count = await _unitOfWork.Repository<SocialMedia>().GetCountByIdWithSpesificationAsync(spec);

            var SocialMapped = _mapper.Map<SocialMediaDto, SocialMedia>(socialMediaDto);
            if (count == 0)
            {

                await _unitOfWork.Repository<SocialMedia>().AddAsync(SocialMapped);
            }
            else if (count == 1)
            {
                var Social = await _unitOfWork.Repository<SocialMedia>().GetByIdWithSpesificationAsync(spec);
                Social.TikTok = socialMediaDto.TikTok;
                Social.YouTube = socialMediaDto.YouTube;
                Social.Facebook = socialMediaDto.Facebook;
                Social.Instagram = socialMediaDto.Instagram;
                Social.Twitter = socialMediaDto.Twitter;
                Social.LinkedIn = socialMediaDto.LinkedIn;
                Social.IsActive = socialMediaDto.IsActive;

                _unitOfWork.Repository<SocialMedia>().Update(Social);
            }
            await _unitOfWork.CompletedAsynk();



            return Ok(socialMediaDto);

        }


        [ProducesResponseType(typeof(SocialMediaDto), 200)]
        [HttpGet("GetSocialMediaBySiteId")]
        public async Task<ActionResult<SocialMediaDto>> GetSocialMediaBySiteId(int SiteId)
        {

            var spec = new SocialMediaDtoSpesification(SiteId);
            var Social = await _unitOfWork.Repository<SocialMedia>().GetByIdWithSpesificationAsync(spec);
            if (Social != null)
            {
                var SocialMapped = _mapper.Map<SocialMedia, SocialMediaDto>(Social);
                return Ok(SocialMapped);
            }
            else
                return NotFound(new ApiResponse(404, "المعرف غير موجود"));
        }
    }
}
