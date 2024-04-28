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
    public class ShippingAreasController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public ShippingAreasController(IMapper mapper , IUnitOfWork unitOfWork , UserManager<AppUser> userManager)
        {
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        [HttpGet("GetAllShipingAreas")]
        [ProducesResponseType(typeof(IEnumerable<ShippingGovernoratesPricesToReturnDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ShippingGovernoratesPricesToReturnDto>>> GetAllShipingAreas(int SiteId)
        {
            var Spec= new SiteSpesification(SiteId);
            var site =await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(Spec);
            var Shipping = site.ShippingGovernoratesPrices;
            if(Shipping is not null)
            {
                var ShippingMapped = _mapper.Map<IEnumerable<ShippingGovernoratesPrices>, IEnumerable<ShippingGovernoratesPricesToReturnDto>>(Shipping);
                return Ok(ShippingMapped);
            }
            else
            {
                return NotFound(new ApiResponse(400, "المعرف غير موجود"));
            }

        }




        [ProducesResponseType(typeof(IEnumerable<ShippingGovernoratesPricesToReturnDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPost("AddShipingAreas")]
        public async Task<ActionResult<IEnumerable<ShippingGovernoratesPricesToReturnDto>>> AddShipingAreas(bool IsActive, IEnumerable<ShippingGovernoratesPricesDto> model)
        {

            if (ModelState.IsValid)
            {
                var ShippingMapped = _mapper.Map<IEnumerable<ShippingGovernoratesPricesDto>, IEnumerable<ShippingGovernoratesPrices>>(model);
                foreach(var item in ShippingMapped)
                {
                   
                 await   _unitOfWork.Repository<ShippingGovernoratesPrices>().AddAsync(item);
                }
                await _unitOfWork.CompletedAsynk();

                var spec = new ShippingGovernoratesSpesification(model.FirstOrDefault().SiteId);
                var ShippingAreas=await _unitOfWork.Repository<ShippingGovernoratesPrices>().GetAllWithSpesificationAsync(spec);

                ShippingAreas.FirstOrDefault().Site.ShippingAreasIsActive= IsActive;
                _unitOfWork.Repository<Site>().Update(ShippingAreas.FirstOrDefault().Site);

                await _unitOfWork.CompletedAsynk();

                var ShippingAreasMapped = _mapper.Map<IEnumerable<ShippingGovernoratesPrices>, IEnumerable<ShippingGovernoratesPricesToReturnDto>>(ShippingAreas);
                return Ok(ShippingAreasMapped);

            }
            else
            {
                return BadRequest(new ApiResponse(400, "البيانات غير مكتمله"));
            }

              
        }


        [ProducesResponseType(typeof(ShippingGovernoratesPricesToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpDelete("DeleteShipingArea")]
        public async Task<ActionResult<IEnumerable<ShippingGovernoratesPricesToReturnDto>>> DeleteShipingArea( IEnumerable<ShippingGovernoratesPricesToReturnDto> model)
        {

            var ShippingMapped = _mapper.Map<IEnumerable<ShippingGovernoratesPricesToReturnDto>, IEnumerable<ShippingGovernoratesPrices>>(model);
            foreach (var item in ShippingMapped)
            {
                _unitOfWork.Repository<ShippingGovernoratesPrices>().Delete(item);
            }
            await _unitOfWork.CompletedAsynk();



            var Shipping = await _unitOfWork.Repository<ShippingGovernoratesPrices>().GetAllAsync();
            var ShippingMapDto= _mapper.Map<IEnumerable< ShippingGovernoratesPrices>,IEnumerable< ShippingGovernoratesPricesToReturnDto>>(Shipping);

            return Ok(ShippingMapDto);

            


        }

    }
}
