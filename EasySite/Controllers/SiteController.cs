using AutoMapper;
using Core.Entites;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.SittingFormOrder;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DataSeeding.TextData;
using EasySite.DTOs;
using EasySite.DTOs.HomePage.ToReturn;
using EasySite.DTOs.productDTO;
using EasySite.DTOs.productDTO.ToReturn;
using EasySite.Errors;
using EasySite.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IResponseCachingService _responseCachingService;
        private readonly HttpClient _httpClient;

        public SiteController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager , IConfiguration configuration, IResponseCachingService responseCachingService, HttpClient httpClient)
        {

            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
            this._responseCachingService = responseCachingService;
            this._httpClient = httpClient;
        }


        [ProducesResponseType(typeof(SiteNoFileDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User}")]
        [HttpPost("AddSite")]
        public async Task<ActionResult<SiteNoFileDto>> AddSite( SiteDto siteDto)
        {
            siteDto.LinkName= siteDto.LinkName.ToLower();

            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);

            var SiteSpec = new SiteSpesification(user.Id);
            var count = await _unitOfWork.Repository<Site>().GetCountByIdWithSpesificationAsync(SiteSpec);

            var SiteByLinkNameSpec = new SiteSpesification(siteDto.LinkName, 0);
            var SiteByLinkName = await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(SiteByLinkNameSpec);

            if(SiteByLinkName != null)
            {
                return BadRequest(new ApiResponse(400," رابط الموقع محجوز"));
            }

            if (user.UserType == TypeUser.Pro || user.freeTrial == FreeTrial.Active)
            {

                var site = _mapper.Map<SiteDto, Site>(siteDto);
                site.AppUserId = user.Id;

                site.Logo = "Files/Images/LogoStore.webp";
                if (siteDto.Logo is not null)
                {
                    site.Logo = SettingesImages.UplodeFile(siteDto.Logo, "Images");
                }

                if (siteDto.MiniIcon is not null)
                {
                    site.MiniIcon = SettingesImages.UplodeFile(siteDto.MiniIcon, "Images");
                }



                if (user.freeTrial == FreeTrial.Active && user.UserType != TypeUser.Pro)
                {
                    if (count >= 1)
                    {
                        return BadRequest(new ApiResponse(400, " لا يمكن اضافة اكثر من موقع خلال التجربه المجانيه"));
                    }
                }

                if (count >= 5)
                {
                    return BadRequest(new ApiResponse(400, " لا يمكن اضافة اكثر من 5 مواقع"));
                }

               var siteAdded= await _unitOfWork.Repository<Site>().AddAsync(site);
                await _unitOfWork.CompletedAsynk();

                // اضافة المنتجات والاقسام
                var ObjectesJson =  await AppApplecationSeed.GetObjectesSeedReturnJson();
                await AdddepartmentAndProductsAndHomePage(ObjectesJson, siteAdded.Id);
                var siteNoFileMapped = _mapper.Map<Site, SiteNoFileDto>(siteAdded);
                return Ok(siteNoFileMapped);

            }
            else
            {
                return BadRequest(new ApiResponse(400, "اولا Pro لا يمكن اضافة موقع قم بالترقيه الي ال"));
            }


        }

        private async Task AdddepartmentAndProductsAndHomePage(ObjectesSeedProductRatingDepartment Obj, int siteId )
        {
            int DepartmentId = 0;
            int ProductId = 0;

            var homePageController= new HomePageController(_unitOfWork,_mapper,_configuration, _responseCachingService);
            // Get Token From Request ===>
            string token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            /// Send Token In Request ===>
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            for(int i = 0; i < Obj.departments.Count(); i++)
            {
                Obj.departments[i].SiteId = siteId;
                var departmentAdded = await _unitOfWork.Repository<Department>().AddAsync(Obj.departments[i]);
                await _unitOfWork.CompletedAsynk();
                DepartmentId = departmentAdded.Id;  //====>

                for (int k = 0; k < Obj.ListOfListProduct[i].Count(); k++)
                    {
                        Obj.ListOfListProduct[i][k].DepartmentId = departmentAdded.Id;
                        Obj.ListOfListProduct[i][k].SiteId = siteId;

                        // ==> Add Product ===>
                        var response = await _httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}api/Products/AddProduct", Obj.ListOfListProduct[i][k]);
                        var ProductAdded = await response.Content.ReadFromJsonAsync<ProductToReturnDto>();

                    ProductId = ProductAdded.Id; //===> 

                    // Add Ratings ==> 
                       Obj.ratings = Obj.ratings.Select(r => { r.ProductId = ProductAdded.Id; return r; }).ToList();
                        Obj.ratings = Obj.ratings.Select(r => { r.Id = 0; return r; }).ToList();
                        Obj.ratings = Obj.ratings.Select(r => { r.DateCreated = DateTime.Now; return r; }).ToList();
                        await _unitOfWork.Repository<Ratings>().AddRangeAsync(Obj.ratings);
                        await _unitOfWork.CompletedAsynk();
                    }
            }

            // Add Pages ===>
            Obj.pages = Obj.pages.Select(page => { page.SiteId = siteId; return page; }).ToList();
            await _unitOfWork.Repository<Pages>().AddRangeAsync(Obj.pages);
            await _unitOfWork.CompletedAsynk();

            Obj.homePageDto.SiteId = siteId;
            Obj.homePageDto.ProductsInHomePageDto.FirstOrDefault().DepartmentId = DepartmentId;

            Obj.homePageDto.SliderDto.FirstOrDefault().SliderImageDto.ToList()[0].TypeId = ProductId;
            Obj.homePageDto.SliderDto.FirstOrDefault().SliderImageDto.ToList()[1].TypeId = DepartmentId;
            Obj.homePageDto.SliderDto.FirstOrDefault().SliderImageDto.ToList()[2].TypeId = ProductId - 1;

            //  Add Home Page ===>
           await _httpClient.PostAsJsonAsync($"{_configuration["ApiBaseUrl"]}api/HomePage/AddOrUpdateHomePage", Obj.homePageDto);

            // Add Sitting Order Form ====>
            Obj.sittingFormOrder.SiteId = siteId;
            await _unitOfWork.Repository<SittingFormOrder>().AddAsync(Obj.sittingFormOrder);
            await _unitOfWork.CompletedAsynk();
        }


        [ProducesResponseType(typeof(SiteNoFileDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPut("UpdatSite")]
        public async Task<ActionResult<SiteNoFileDto>> UpdatSite(SiteDto siteDto, int SiteId)
        {
            var sitSpec = new SiteSpesification(SiteId);
            var site = await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(sitSpec);

            var SiteByLinkNameSpec = new SiteSpesification(siteDto.LinkName, 0);
            var SiteByLinkName = await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(SiteByLinkNameSpec);

            if (SiteByLinkName != null)
            {
                if (SiteByLinkName.Id != SiteId)
                {
                    return BadRequest(new ApiResponse(400, " رابط الموقع محجوز"));
                }
            }


            if (site is not null)
            {
                site.Logo = "Files/Images/LogoStore.webp";
                if (siteDto.Logo is not null)
                {
                    site.Logo = SettingesImages.UplodeFile(siteDto.Logo, "Images");
                }

                if (siteDto.MiniIcon is not null)
                {
                    site.MiniIcon = SettingesImages.UplodeFile(siteDto.MiniIcon, "Images");
                }

                site.LinkName= siteDto.LinkName;
                site.TitleSite = siteDto.TitleSite;
                site.colorSite = siteDto.colorSite;
                site.WhatsApp = siteDto.WhatsApp;
                site.Coll_Phone = siteDto.Coll_Phone;
                site.ShippingPrice = siteDto.ShippingPrice;
                site.Currency = (currency)Enum.Parse(typeof(currency), siteDto.Currency);
                site.ColorTextHedar = siteDto.ColorTextHedar;
                site.FontType = (FontType)Enum.Parse(typeof(FontType), siteDto.FontType);
                site.HeaderCode= siteDto.HeaderCode;

            }
            else
            {
                return NotFound(new ApiResponse(404, "المعرف غير موجود"));
            }

            _unitOfWork.Repository<Site>().Update(site);
            await _unitOfWork.CompletedAsynk();
            var SiteNotFile = _mapper.Map<Site, SiteNoFileDto>(site);

            return Ok(SiteNotFile);

        }



        //[ProducesResponseType(bool, 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User}")]
        [HttpDelete("DeleteSite")]
        public async Task<ActionResult> DeleteSite(int SiteId)
        {
            var sitSpec = new SiteSpesification(SiteId);
            var site = await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(sitSpec);

            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (site != null && site.AppUser.Id == user.Id)
            {
               foreach(var product in site.Products)
                {
                    var spec = new ProductDataSpecifacation(product.Id);
                    var productData = await _unitOfWork.Repository<ProductData>().GetAllWithSpesificationAsync(spec);

                   await _unitOfWork.Repository<ProductData>().DeletRangeAsync(productData);
                    await _unitOfWork.CompletedAsynk();
                }

                _unitOfWork.Repository<Site>().Delete(site);
                await _unitOfWork.CompletedAsynk();

                return Ok();
            }
            else
            {
                return NotFound(new ApiResponse(404, "المعرف غير موجود"));
            }
        }



        [ProducesResponseType(typeof(SiteNoFileDto), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpGet("GetAllSitByUserId")]
        public async Task<ActionResult<IEnumerable<SiteNoFileDto>>> GetAllSitByUserId()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if(role == CsRoles.User)
            {
                var siteSpec = new SiteSpesification(user.Id);
                var sites = await _unitOfWork.Repository<Site>().GetAllWithSpesificationAsync(siteSpec);
                var UserMapped = _mapper.Map<IEnumerable<Site>, IEnumerable<SiteNoFileDto>>(sites);
                return Ok(UserMapped);
            }
            else if (role == CsRoles.Manager)
            {
                var siteSpec = new SiteSpesification(user.MangerId);
                var sites = await _unitOfWork.Repository<Site>().GetAllWithSpesificationAsync(siteSpec);
                var UserMapped = _mapper.Map<IEnumerable<Site>, IEnumerable<SiteNoFileDto>>(sites);
                return Ok(UserMapped);
            }
            return NotFound(new ApiResponse(404, "المعرف غير موجود"));
        }



        [ProducesResponseType(typeof(SiteNoFileDto), 200)]
        [HttpGet("GetSiteById")]
        public async Task<ActionResult<SiteNoFileDto>> GetSiteById(int siteId)
        {
            var Spec = new SiteSpesification(siteId);
            var site = await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(Spec);
            if (site == null)
                return BadRequest(new ApiResponse(400, "معرف غير صحيح"));
            var SiteMapp = _mapper.Map<Site, SiteNoFileDto>(site);
            return Ok(SiteMapp);
        }



        ////////////////////////////////////////////////// Blocked Numbers /////////////////////////////////////////////////////////

        [ProducesResponseType(typeof(IEnumerable<BlockedNumbersToReturnDto>), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPost("AddBlocedNumber")]
        public async Task<ActionResult<IEnumerable<BlockedNumbersToReturnDto>>> AddBlocedNumber(BlockedNumbersDto blockedNumbersDto)
        {
            //blockedNumbersDto.SiteId = 0;
            var BolckMapped = _mapper.Map<BlockedNumbersDto, BlockedNumbers>(blockedNumbersDto);
            await _unitOfWork.Repository<BlockedNumbers>().AddAsync(BolckMapped);
            await _unitOfWork.CompletedAsynk();

            var spec = new BlockedNumbersSpecification(blockedNumbersDto.SiteId);
            var BlockNumber = await _unitOfWork.Repository<BlockedNumbers>().GetAllWithSpesificationAsync(spec);

            var BlokedToReturn = _mapper.Map<IEnumerable<BlockedNumbers>, IEnumerable<BlockedNumbersToReturnDto>>(BlockNumber);
            return Ok(BlokedToReturn);

        }

        [ProducesResponseType(typeof(IEnumerable<BlockedNumbersToReturnDto>), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpDelete("DeleteBlocedNumber")]
        public async Task<ActionResult<IEnumerable<BlockedNumbersToReturnDto>>> DeleteBlocedNumber(int BlockedNumberId)
        {
            var Numbre = await _unitOfWork.Repository<BlockedNumbers>().GetByIdAsync(BlockedNumberId);
            if (Numbre != null)
            {
                _unitOfWork.Repository<BlockedNumbers>().Delete(Numbre);
                await _unitOfWork.CompletedAsynk();

                var spec = new BlockedNumbersSpecification(Numbre.SiteId);
                var BlockNumber = await _unitOfWork.Repository<BlockedNumbers>().GetAllWithSpesificationAsync(spec);

                var BlokedToReturn = _mapper.Map<IEnumerable<BlockedNumbers>, IEnumerable<BlockedNumbersToReturnDto>>(BlockNumber);
                return Ok(BlokedToReturn);
            }
            else
            {
                return NotFound(new ApiResponse(400, "معرف غير صحيح"));
            }


        }

        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(IEnumerable<BlockedNumbersToReturnDto>), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpGet("GetAllBlocedNumber")]
        public async Task<ActionResult<IEnumerable<BlockedNumbersToReturnDto>>> GetAllBlocedNumber(int SiteId)
        {
            var spec = new BlockedNumbersSpecification(SiteId);
            var BlockNumber = await _unitOfWork.Repository<BlockedNumbers>().GetAllWithSpesificationAsync(spec);

            var BlokedToReturn = _mapper.Map<IEnumerable<BlockedNumbers>, IEnumerable<BlockedNumbersToReturnDto>>(BlockNumber);
            return Ok(BlokedToReturn);

        }

        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(BlockedNumbersToReturnDto), 200)]
        [HttpGet("CheckPhoneNumber")]
        public async Task<ActionResult<BlockedNumbersToReturnDto>> CheckPhoneNumber(string PhoneNumber)
        {

            var spec = new BlockedNumbersSpecification(PhoneNumber);
            var BlockNumber = await _unitOfWork.Repository<BlockedNumbers>().GetByIdWithSpesificationAsync(spec);
            if (BlockNumber != null)
            {
                var BlokedToReturn = _mapper.Map<BlockedNumbers, BlockedNumbersToReturnDto>(BlockNumber);
                return Ok(BlokedToReturn);
            }
            else
            {
                return NotFound(new ApiResponse(400, "الرقم غير محظور"));
            }


        }

    }
}
