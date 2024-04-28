using EasySite.Core.Entites.SittingFormOrder;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs;
using EasySite.DTOs.HomePage.ToReturnEndUser;
using EasySite.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SittingFormOrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SittingFormOrderController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [ProducesResponseType(typeof(SittingFormOrder), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPut("AddOrUpdateSittingFormOrder")]
        public async Task<ActionResult<SittingFormOrder>> CreateOrUpdat(SittingFormOrder sittingForm)
        {
            var SettingSpec = new SittingFormOrderSpesification(sittingForm.SiteId);
            var sitting = await _unitOfWork.Repository<SittingFormOrder>().GetByIdWithSpesificationAsync(SettingSpec);
            /// Add
            if (sitting == null)
            {
                sittingForm.Id = 0;
                sittingForm.Email.Id= 0;
                sittingForm.FullName.Id = 0;
                sittingForm.Address.Id = 0;
                sittingForm.Country.Id = 0;
                sittingForm.Phone.Id = 0;
                sittingForm.Government.Id = 0;
                var SittingAded=  await _unitOfWork.Repository<SittingFormOrder>().AddAsync(sittingForm);
                await _unitOfWork.CompletedAsynk();
                return Ok(SittingAded);
            }
            else // Update
            {
                sitting.Phone = sittingForm.Phone;
                sitting.Email = sittingForm.Email;
                sitting.Government = sittingForm.Government;
                sitting.FullName = sittingForm.FullName;
                sitting.Country = sittingForm.Country;
                sitting.Address = sittingForm.Address;

                _unitOfWork.Repository<SittingFormOrder>().Update(sitting);
                await _unitOfWork.CompletedAsynk();

                return Ok(sitting);
            }

        }


        //public async Task<ActionResult<SittingFormOrder>> SittingFormOrderMapped(SittingFormOrder  sittingFormOrder)
        //{

        //}



        [ProducesResponseType(typeof(SittingFormOrderDto), 200)]
        [HttpGet("GetSittingFormOrder")]
        public async Task<ActionResult<SittingFormOrderDto>> GetSittingFormOrder(int SiteId)
        {
            var Spec= new SittingFormOrderSpesification(SiteId);
            var sitting= await _unitOfWork.Repository<SittingFormOrder>().GetByIdWithSpesificationAsync(Spec);
            if (sitting == null)
                return BadRequest(new ApiResponse(400, "Id غير موجود"));

            // sort ==>
            var SittingDto = new SittingFormOrderDto() { Id = sitting.Id, SiteId = sitting.SiteId };
            SittingDto.Items.Add(sitting.Country);
            SittingDto.Items.Add(sitting.Government);
            SittingDto.Items.Add(sitting.Email);
            SittingDto.Items.Add(sitting.FullName);
            SittingDto.Items.Add(sitting.Phone);
            SittingDto.Items.Add(sitting.Address);

            // var sortedObjects = ListSitting.OrderBy(l => l.Index).ToList();


            SittingDto.Items = SittingDto.Items.OrderBy(item =>
            {
                dynamic obj = item;
                return (int)obj.Index;
            }).ToList();

            return Ok(SittingDto);
        }
    }
}
