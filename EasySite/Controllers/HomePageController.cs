using AutoMapper;
using Core.Entites;
using Core.Entites.Homepage;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Homepag;
using EasySite.Core.Entites.SittingFormOrder;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs;
using EasySite.DTOs.HomePage;
using EasySite.DTOs.HomePage.ToReturn;
using EasySite.DTOs.HomePage.ToReturnEndUser;
using EasySite.DTOs.productDTO.ToReturn;
using EasySite.Errors;
using EasySite.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IResponseCachingService responseCachingService;

        public HomePageController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IResponseCachingService responseCachingService)
        {

            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._configuration = configuration;
            this.responseCachingService = responseCachingService;
        }

        [ProducesResponseType(typeof(HomePageToreturn), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPost("AddOrUpdateHomePage")]
        public async Task<ActionResult<HomePageToreturn>> AddOrUpdateHomePage(HomePageDto homePageDto)
        {
            var SpecHomePage = new HomePageSpecification(homePageDto.SiteId);
            var homePage = await _unitOfWork.Repository<Homepage>().GetByIdWithSpesificationAsync(SpecHomePage);

            // Update
            if (homePage != null)
            {
                _unitOfWork.Repository<Homepage>().Delete(homePage);
                await _unitOfWork.CompletedAsynk();
            }
            
            // Add
            
            var homePageMappped = _mapper.Map<HomePageDto, Homepage>(homePageDto);
            homePageMappped.Id = 0;

            var homePageAdded = await _unitOfWork.Repository<Homepage>().AddAsync(homePageMappped);
            var reult = await _unitOfWork.CompletedAsynk();

            homePageMappped.DepartmentsInHomePages = _mapper.Map<IEnumerable<DepartmentsInHomePageDto>, IEnumerable<DepartmentsInHomePage>>(homePageDto.DepartmentsInHomePageDto).ToList();
            homePageMappped.DepartmentsInHomePages = homePageMappped.DepartmentsInHomePages.Select(p => { p.Id = 0; return p; }).ToList();

            homePageMappped.ProductsInHomePage = _mapper.Map<IEnumerable<ProductsInHomePageDto>, IEnumerable<ProductsInHomePage>>(homePageDto.ProductsInHomePageDto).ToList();
            homePageMappped.ProductsInHomePage = homePageMappped.ProductsInHomePage.Select(p => { p.Id = 0; return p; }).ToList();

            await _unitOfWork.Repository<DepartmentsInHomePage>().AddRangeAsync(homePageMappped.DepartmentsInHomePages);
            await _unitOfWork.Repository<ProductsInHomePage>().AddRangeAsync(homePageMappped.ProductsInHomePage);

            //Add Slideres ====>
            for (int i = 0; i < homePageDto.SliderDto.Count(); i++)
            {
                var SliderMapped = _mapper.Map<SliderDto, Slider>(homePageDto.SliderDto[i]);

                SliderMapped.Id = 0;
                SliderMapped.HomepageId = homePageAdded.Id;
                var sliderAdd = await _unitOfWork.Repository<Slider>().AddAsync(SliderMapped);
                await _unitOfWork.CompletedAsynk();

                // Mapp SliderImages =====>
                sliderAdd.SliderImage = _mapper.Map<IEnumerable<SliderImageDto>, IEnumerable<SliderImage>>(homePageDto.SliderDto.ToList()[i].SliderImageDto).ToList();
                sliderAdd.SliderImage.Select(p => p.SliderId = sliderAdd.Id);
                sliderAdd.SliderImage = sliderAdd.SliderImage.Select( p =>{ p.Id = 0; return p; }).ToList();

                for (int j = 0; j < sliderAdd.SliderImage.Count(); j++)
                {
                    sliderAdd.SliderImage.ToList()[j].Image = $"Files/Images/slider1.png";
                    if (homePageDto.SliderDto.ToList()[i].SliderImageDto.ToList()[j].ImageFile != null)
                    {
                        sliderAdd.SliderImage.ToList()[j].Image = SettingesImages.UplodeFile(homePageDto.SliderDto.ToList()[i].SliderImageDto.ToList()[j].ImageFile, "Images");
                    }
                    if (homePageDto.SliderDto.ToList()[i].SliderImageDto.ToList()[j].Image != null)
                    {
                        sliderAdd.SliderImage.ToList()[j].Image = homePageDto.SliderDto.ToList()[i].SliderImageDto.ToList()[j].Image;
                    }

                }

                await _unitOfWork.Repository<SliderImage>().AddRangeAsync(sliderAdd.SliderImage);
                await _unitOfWork.CompletedAsynk();
            }

            var HomePageToReturn =await CreateHomePageToReturn(homePageAdded.SiteId);


            return Ok(HomePageToReturn);
        }

        [HttpGet]
        private async Task<HomePageToreturn> CreateHomePageToReturn(int siteId)
        {
            var HomeSpec = new HomePageSpecification(siteId);
            var HomePageInDataBase= await _unitOfWork.Repository<Homepage>().GetByIdWithSpesificationAsync(HomeSpec);

            var HomePageToReturn = _mapper.Map<Homepage, HomePageToreturn>(HomePageInDataBase);
            HomePageToReturn.DepartmentsInHomePageDto = _mapper.Map<IEnumerable<DepartmentsInHomePage>,IEnumerable< DepartmentsInHomePageDto>>(HomePageInDataBase.DepartmentsInHomePages).ToList();
            HomePageToReturn.ProductsInHomePageDto = _mapper.Map<IEnumerable<ProductsInHomePage>, IEnumerable<ProductsInHomePageDto>>(HomePageInDataBase.ProductsInHomePage).ToList();
            HomePageToReturn.SliderOtReturnDto =_mapper.Map<IEnumerable<Slider>,IEnumerable<SliderOtReturnDto>>(HomePageInDataBase.Sliders).ToList();

            for (int i = 0; i < HomePageInDataBase.Sliders.Count(); i++)
            {
                HomePageToReturn.SliderOtReturnDto[i].SliderImageToReturnDto = _mapper.Map<IEnumerable<SliderImage>, IEnumerable<SliderImageToReturnDto>>(HomePageInDataBase.Sliders.ToList()[i].SliderImage).ToList();
            }

            return HomePageToReturn;
        }




        [ProducesResponseType(typeof(HomePageToReturnEndUser), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [HttpGet("GetHomePage")]
        public async Task<ActionResult<HomePageToReturnEndUser>> GetHomePage(int SiteId)
        {
            var HomeSpec = new HomePageSpecification(SiteId);
            var HomePageInDataBase = await _unitOfWork.Repository<Homepage>().GetByIdWithSpesificationAsync(HomeSpec);
            if(HomePageInDataBase != null)
            {
                var homePageToReturnEndUser = new HomePageToReturnEndUser();
                homePageToReturnEndUser.IsActive = HomePageInDataBase.IsActive;
                homePageToReturnEndUser.ShowInHedear = HomePageInDataBase.ShowInHedear;
                homePageToReturnEndUser.Id = HomePageInDataBase.Id;

                ///  Get Department ===> 
                if (HomePageInDataBase.DepartmentsInHomePages.Count()>0)
                {
                    for (int i = 0; i < HomePageInDataBase.DepartmentsInHomePages.Count(); i++)
                    {
                        var Departmentes =await _unitOfWork.Repository<Department>().GetAllAsync();
                        var departmtesEndUser = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentToreturnDto>>(Departmentes).ToList();

                        var DepartmentEndUser = new DepartmentsToReturnEndUser()
                        {
                            TypeItem = "Department",
                            Index = HomePageInDataBase.DepartmentsInHomePages.ToList()[i].Index,
                            DepartmentToreturnDto = departmtesEndUser
                        };

                        homePageToReturnEndUser.AllItemesInHomePage.Add(DepartmentEndUser);
                    }
                }

                //Get Products ===>
                if (HomePageInDataBase.ProductsInHomePage.Count() > 0)
                {
                    for (int i = 0; i < HomePageInDataBase.ProductsInHomePage.Count(); i++)
                    {
                        
                        var SpecProduct = new ProductSpeification( HomePageInDataBase.ProductsInHomePage.ToList()[i].sortBy.ToString() , HomePageInDataBase.ProductsInHomePage.ToList()[i].DepartmentId);
                        var products = await _unitOfWork.Repository<Product>().GetAllWithSpesificationAsync(SpecProduct);
                        if (products != null)
                        {
                            List<ProductToReturnDto> ProductsToReturn = new List<ProductToReturnDto>();

                            var productsController = new ProductsController(_unitOfWork, _mapper, _configuration, responseCachingService);
                            foreach (var item in products)
                            {
                                var ProductToReturn = await productsController.GetProductToReturn(item);
                                ProductsToReturn.Add(ProductToReturn);
                            }


                            var ProductEndUser = new ProductToReturnEndUser()
                            {
                                TypeItem= "Produc",
                                Index = HomePageInDataBase.ProductsInHomePage.ToList()[i].Index,
                                Titele = HomePageInDataBase.ProductsInHomePage.ToList()[i].Titele,
                                FormatNumber = HomePageInDataBase.ProductsInHomePage.ToList()[i].FormatNumber,
                                ProductToReturnDto = ProductsToReturn
                            };

                            homePageToReturnEndUser.AllItemesInHomePage.Add(ProductEndUser);
                        }
                    }
                }

                // Get Slider ==> 
                if (HomePageInDataBase.Sliders.Count() > 0)
                {
                    List<SliderImagesToReturnEndUser > SlidersImagesToReturnEndUser = new List<SliderImagesToReturnEndUser>();
                    for (int i = 0; i < HomePageInDataBase.Sliders.Count(); i++)
                    {
 
                        for (int j = 0; j < HomePageInDataBase.Sliders.ToList()[i].SliderImage.Count(); j++)
                        {
                            var ProductToReturn = new object();

                            // ===> object == Product
                            if (HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].TypeName == SliderRedirectToType.Product)
                            {
                                var SpecProduct = new ProductSpeification(HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].TypeId);
                                var product = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(SpecProduct);
                                if(product != null)
                                {
                                    var productsController = new ProductsController(_unitOfWork, _mapper, _configuration, responseCachingService);
                                    ProductToReturn = await productsController.GetProductToReturn(product);
                                }
                            }

                            // ===> object == Department
                            if (HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].TypeName == SliderRedirectToType.Department)
                            {
                                
                                var department = await _unitOfWork.Repository<Department>().GetByIdAsync(HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].TypeId);
                                if(department != null)
                                {
                                    ProductToReturn = _mapper.Map<Department, DepartmentToreturnDto>(department);
                                }
                                
                            }

                            // ===> object == Page
                            if (HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].TypeName == SliderRedirectToType.Page)
                            {
                                var page = await _unitOfWork.Repository<Pages>().GetByIdAsync(HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].TypeId);
                                if(page!=null)
                                {
                                    ProductToReturn = _mapper.Map<Pages, PageToReturnDto>(page);
                                }
                                
                            }

                            // ===> Create Object from SliderImageEndUser
                            var SliderImageEndUser = new SliderImagesToReturnEndUser()
                            {
                                Image = _configuration["ApiBaseUrl"] + HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].Image,
                                TypeId = HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].TypeId,
                                TypeName = HomePageInDataBase.Sliders.ToList()[i].SliderImage.ToList()[j].TypeName.ToString(),
                                ProductsOrDpurtmentOrPages = ProductToReturn,
                            };

                            SlidersImagesToReturnEndUser.Add(SliderImageEndUser);
                        }

                        // ===> Create Object from SliderEndUser
                        var SliderToreturnEndUser = new SliderToReturnEndUser()
                        {
                            TypeItem = "Slider",
                            Index = HomePageInDataBase.Sliders.ToList()[i].Index,
                            SliderImagesToReturnEndUser = SlidersImagesToReturnEndUser
                        };

                        homePageToReturnEndUser.AllItemesInHomePage.Add(SliderToreturnEndUser);

                    }

                }



                homePageToReturnEndUser.AllItemesInHomePage = homePageToReturnEndUser.AllItemesInHomePage.OrderBy(item =>
                {
                    dynamic obj = item;
                    return (int)obj.Index;
                }).ToList();

                return Ok(homePageToReturnEndUser);



            }
            return NotFound(new ApiResponse(400, "لا يوجد قصفحه رايسيه"));

        }

    }
}
