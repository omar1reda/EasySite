using AutoMapper;
using Core.Entites;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs;
using EasySite.Errors;
using EasySite.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {

            this._unitOfWork = unitOfWork;
            this._mapper = mapper;

        }


        [ProducesResponseType(typeof(DepartmentToreturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPut("AddOrUpdateDepartment")]
        public async Task<ActionResult<DepartmentToreturnDto>> AddOrUpdateDepartment([FromForm] DepartmentDto model)
        {

                var specLinkName = new DepartmentSpecification(model.LinkName,model.SiteId);

                var specById = new DepartmentSpecification( model.SiteId, model.Id);
                var department =await _unitOfWork.Repository<Department>().GetByIdWithSpesificationAsync(specById);

                model.Image = SettingesImages.UplodeFile(model.ImageFile, "Images");

                var departmentSpac = await _unitOfWork.Repository<Department>().GetByIdWithSpesificationAsync(specLinkName);

                if (department == null)
                {
                    // add

                    if (departmentSpac == null)
                    {
                        model.Id = 0;
                        var DepartmentMapped = _mapper.Map<DepartmentDto, Department>(model);
                    var DepartmentAdded=    await _unitOfWork.Repository<Department>().AddAsync(DepartmentMapped);
                        await _unitOfWork.CompletedAsynk();

                        var spec = new DepartmentSpecification(model.SiteId);
                        var Departments = await _unitOfWork.Repository<Department>().GetByIdWithSpesificationAsync(spec);
                        var Departmenttoreturn = _mapper.Map<Department, DepartmentToreturnDto>(DepartmentAdded);

                        return Ok(Departmenttoreturn);
                    }
                    else
                    {                       
                        return BadRequest(new ApiResponse(404, "رابط القسم محجوز"));
                    }
                   
                }
                else
                {
                    //Update
                    department.ShowInHedar= model.ShowInHedar;
                    department.index= model.index;
                    department.Name= model.Name;
                    department.Image= model.Image;

                    if (model.LinkName != department.LinkName)
                    {
                            if (departmentSpac != null)
                            {
                                  return BadRequest(new ApiResponse(404, "رابط القسم محجوز"));
                            }
                    }
                    department.LinkName = model.LinkName;
                    _unitOfWork.Repository<Department>().Update(department);
                    await _unitOfWork.CompletedAsynk();

                    var spec = new DepartmentSpecification(0,model.Id);
                    var Departments = await _unitOfWork.Repository<Department>().GetByIdWithSpesificationAsync(spec);
                    var DepartmentsMapped = _mapper.Map<Department, DepartmentToreturnDto>(Departments);
                    return Ok(DepartmentsMapped);

                }

        }


        [ProducesResponseType(typeof(DepartmentToreturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpGet("GetByLinkName")]
        public async Task<ActionResult<DepartmentToreturnDto>> GetByLinkName(string LinkName)
        {
            var spec = new DepartmentSpecification(LinkName);
            var department =await _unitOfWork.Repository<Department>().GetByIdWithSpesificationAsync(spec);
            if(department != null)
            {
                var departmentMapped = _mapper.Map<Department, DepartmentToreturnDto>(department);
                return Ok(departmentMapped);
            }
            else
            {
                return NotFound(new ApiResponse(400,"معرف غير موجود"));
            }
            
        }


        [ProducesResponseType(typeof(DepartmentToreturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpGet("GetByDepartmentId")]
        public async Task<ActionResult<DepartmentToreturnDto>> GetByDepartmentId(int DepartmentId)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(DepartmentId);
            if (department != null)
            {
                var departmentMapped = _mapper.Map<Department, DepartmentToreturnDto>(department);
                return Ok(departmentMapped);
            }
            else
            {
                return NotFound(new ApiResponse(400, "معرف غير موجود"));
            }

        }



        [ProducesResponseType(typeof(DepartmentToreturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpGet("GetAllDepartment")]
        public async Task<ActionResult<IEnumerable< DepartmentToreturnDto>>> GetAllDepartment(int SiteId)
        {
            var spec = new DepartmentSpecification(SiteId);
            var departments = await _unitOfWork.Repository<Department>().GetAllWithSpesificationAsync(spec);

                var departmentMapped = _mapper.Map<IEnumerable< Department>, IEnumerable<DepartmentToreturnDto>>(departments);
                return Ok(departmentMapped);
 
        }


        [ProducesResponseType(typeof(DepartmentToreturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpDelete("DeleteDepartment")]
        public async Task<ActionResult<IEnumerable<DepartmentToreturnDto>>> DeleteDepartment(int DepartmentId)
        {
           var dept = await _unitOfWork.Repository<Department>().GetByIdAsync(DepartmentId);   
            if (dept != null)
            {
                _unitOfWork.Repository<Department>().Delete(dept);
                await _unitOfWork.CompletedAsynk();

                var spec = new DepartmentSpecification(dept.SiteId);
                var departments = await _unitOfWork.Repository<Department>().GetAllWithSpesificationAsync(spec);
                var departmentMapped = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentToreturnDto>>(departments);
                return Ok(departmentMapped);
            }
            return NotFound(new ApiResponse(404, "معرف غير موجود"));

        }

    }
}
