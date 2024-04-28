using AutoMapper;
using Core.Entites;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Params;
using EasySite.Core.Entites.Product;
using EasySite.Core.Entites.Products;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs.productDTO;
using EasySite.DTOs.productDTO.ToReturn;
using EasySite.Errors;
using EasySite.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IResponseCachingService _responseCachingService;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IResponseCachingService responseCachingService)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._configuration = configuration;
            this._responseCachingService = responseCachingService;
        }

        /// __________________________________________  القديم واضافة الجيد بعد التحديث product  يتم حذف ال _________________________
        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct(ProductDto productDto, int id)
        {
            var ProducrByLinkName = new ProductSpeification(productDto.DepartmentId, productDto.LinkName);

            var productByLinkName = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(ProducrByLinkName);

            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (productByLinkName is not null)
            {
                if (product.LinkName != productDto.LinkName)
                {
                    return BadRequest(new ApiResponse(404, "اسم المنتج محجوز مسبقا في منتج اخر"));
                }
            }


            var ProductMapped = _mapper.Map<ProductDto, Product>(productDto);
            //ProductMapped.Image = SettingesImages.UplodeFile(productDto.MainImage, "Images");
            ProductMapped.Image = "Image";
            var productAded = await _unitOfWork.Repository<Product>().AddAsync(ProductMapped);
            await _unitOfWork.CompletedAsynk();
            List<OtherImageOfProduct> otherImage = new List<OtherImageOfProduct>();

            otherImage.Add(new OtherImageOfProduct() { Image = "Image1", ProductId = productAded.Id });
            otherImage.Add(new OtherImageOfProduct() { Image = "Image2", ProductId = productAded.Id });

            //for (var i = 0; i < productDto.OtherImage.Count(); i++)
            //{
            //    //var Image = SettingesImages.UplodeFile(productDto.OtherImage[i], "Images");
            //    otherImage.Add(new OtherImageOfProduct() { Image = "Image", ProductId = productAded.Id });
            //};

            await _unitOfWork.Repository<OtherImageOfProduct>().AddRangeAsync(otherImage);
            await _unitOfWork.CompletedAsynk();

            // Add UpSale ===>
            if (productDto.UpSalleDto.Count() > 0)
            {
                var UpSalleMapped = _mapper.Map<IEnumerable<UpSalleDto>, IEnumerable<UpSell>>(productDto.UpSalleDto);
                UpSalleMapped = UpSalleMapped.Select(s => { s.ProductId = productAded.Id; return s; });
                UpSalleMapped = UpSalleMapped.Select(s => { s.Id = 0; return s; });

                await _unitOfWork.Repository<UpSell>().AddRangeAsync(UpSalleMapped);
                await _unitOfWork.CompletedAsynk();
            }

            /// Cross Selling ====> 
            if (productDto.CrossSellingDto != null)
            {
                var Cross = _mapper.Map<CrossSellingDto, CrossSelling>(productDto.CrossSellingDto);
                Cross.ProductId = productAded.Id;
                await _unitOfWork.Repository<CrossSelling>().AddAsync(Cross);
                await _unitOfWork.CompletedAsynk();
            }


            if (productDto.ProductsDataDtos.Count() > 0)
            {
                await AddProductTransformation(productDto, productAded.Id);
            }

            var ProducrByid = new ProductSpeification(productAded.Id);
            var productFromDataBase = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(ProducrByid);

            //  Created  ====> ProductToReturn
            var ProductReturn = await GetProductToReturn(productFromDataBase);

            await DeleteProduct(id);

            await DeleteCachedResponse($"GetProductByLinkName|DepartmentId-{productDto.DepartmentId}");
            await DeleteCachedResponse($"GetAllProductInDepartment|DepartmentId-{productDto.DepartmentId}");

            return Ok(ProductReturn);

        }

        [HttpPost("AddProduct")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult<ProductToReturnDto>> AddProduct(ProductDto productDto)
        {

            var ProducrByLinkName = new ProductSpeification(productDto.DepartmentId, productDto.LinkName);

            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(ProducrByLinkName);

            if (product == null)
            {
                var ProductMapped = _mapper.Map<ProductDto, Product>(productDto);
                ProductMapped.DateCreated = DateTime.Now;
                if (ProductMapped.Count <= 0)
                {
                    ProductMapped.ProductSoldOut = true;
                }

                ProductMapped.Image = "NotFound.jfif";

                if (productDto.MainImage != null)
                {
                    ProductMapped.Image = productDto.MainImage;
                }
                if (productDto.MainImageFile != null)
                {
                    ProductMapped.Image = SettingesImages.UplodeFile(productDto.MainImageFile, "Images");
                }



                var productAded = await _unitOfWork.Repository<Product>().AddAsync(ProductMapped);
                await _unitOfWork.CompletedAsynk();
                List<OtherImageOfProduct> otherImage = new List<OtherImageOfProduct>();

                if (productDto.OtherImageFile != null)
                {
                    for (var i = 0; i < productDto.OtherImageFile.Count(); i++)
                    {
                        var image = SettingesImages.UplodeFile(productDto.OtherImageFile[i], "Images");
                        otherImage.Add(new OtherImageOfProduct() { Image = image, ProductId = productAded.Id });
                    };
                }

                if (productDto.OtherImage != null)
                {
                    for (var i = 0; i < productDto.OtherImage.Count(); i++)
                    {
                        var image = productDto.OtherImage[i];
                        otherImage.Add(new OtherImageOfProduct() { Image = image, ProductId = productAded.Id });
                    };
                }




                await _unitOfWork.Repository<OtherImageOfProduct>().AddRangeAsync(otherImage);
                await _unitOfWork.CompletedAsynk();

                // Add UpSale ===>
                if (productDto.UpSalleDto.Count() > 0)
                {
                    var UpSalleMapped = _mapper.Map<IEnumerable<UpSalleDto>, IEnumerable<UpSell>>(productDto.UpSalleDto);
                    UpSalleMapped = UpSalleMapped.Select(s => { s.ProductId = productAded.Id; return s; });
                    UpSalleMapped = UpSalleMapped.Select(s => { s.Id = 0; return s; });

                    await _unitOfWork.Repository<UpSell>().AddRangeAsync(UpSalleMapped);
                    await _unitOfWork.CompletedAsynk();
                }

                // Add Cross Selling ====> 
                if (productDto.CrossSellingDto != null)
                {
                    var Cross = _mapper.Map<CrossSellingDto, CrossSelling>(productDto.CrossSellingDto);
                    Cross.ProductId = productAded.Id;
                    await _unitOfWork.Repository<CrossSelling>().AddAsync(Cross);
                    await _unitOfWork.CompletedAsynk();
                }

                //// الخصائص المتعدده للمنتج
                if (productDto.ProductsDataDtos.Count() > 0)
                {

                    await AddProductTransformation(productDto, productAded.Id);

                }

                var ProducrByid = new ProductSpeification(productAded.Id);
                var productFromDataBase = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(ProducrByid);

                //  Created  ====> ProductToReturn
                var ProductReturn = await GetProductToReturn(productFromDataBase);

                await DeleteCachedResponse($"GetAllProductInDepartment|DepartmentId-{productDto.DepartmentId}");


                return Ok(ProductReturn);

            }
            else
            {
                return BadRequest(new ApiResponse(404, "رابط المنتج محجوز"));
            }


        }

        [HttpPost]
        private async Task AddProductTransformation(ProductDto productDto, int id)
        {

            foreach (var ProductVirant in productDto.Products_VariantsDto)
            {
                var productVarint = new Product_Variants() { VariantsName = ProductVirant.VariantsName, ProductId = id };
                var productVarintAdded = await _unitOfWork.Repository<Product_Variants>().AddAsync(productVarint);
                await _unitOfWork.CompletedAsynk();

                // Add  ==> ( Product_Variant_OptionsDto )
                foreach (var ProductOption in ProductVirant.Product_Variant_OptionsDto)
                {
                    var productOption = new Product_Variant_Options() { OptionName = ProductOption.OptionName, Product_VariantsId = productVarintAdded.Id };
                    var productOptionAdded = await _unitOfWork.Repository<Product_Variant_Options>().AddAsync(productOption);
                    await _unitOfWork.CompletedAsynk();

                }
            }

            // ==> all Options 
            var specOption = new ProductVirantOptionSpeification(id);
            var Options = await _unitOfWork.Repository<Product_Variant_Options>().GetAllWithSpesificationAsync(specOption);


            foreach (var productData in productDto.ProductsDataDtos)
            {
                var ProdactData = new ProductData()
                {
                    Count = productData.Count,
                    Sale = productData.Sale,
                    Price = productData.Price,
                    ProductSoldOut = productData.Count > 0 ? productData.ProductSoldOut = false : productData.ProductSoldOut = true,
                    ProductId = id
                };


                // Add  ==> ( ProductData )
                var ProductDataAdded = await _unitOfWork.Repository<ProductData>().AddAsync(ProdactData);
                await _unitOfWork.CompletedAsynk();

                foreach (var Option in Options)
                {
                    //==>> 2
                    foreach (var productOption in productData.Product_Variant_OptionsDto)
                    {
                        if (Option.OptionName == productOption.OptionName)
                        {
                            var productVariantOptions_ProductData = new product_Variant_Options_ProductData()
                            {
                                ProductDataId = ProductDataAdded.Id,
                                Product_Variant_OptionsId = Option.Id
                            };
                            // Add  ==> ( product_Variant_Options_ProductData )
                            await _unitOfWork.Repository<product_Variant_Options_ProductData>().AddAsync(productVariantOptions_ProductData);
                            await _unitOfWork.CompletedAsynk();
                        }

                    }
                }

            }

        }


        private async Task<ProductToReturnDto> ColGetProductToReturn(Product productFromDataBase)
        {
            return await GetProductToReturn(productFromDataBase);
        }

        [HttpPost]
        public async Task<ProductToReturnDto> GetProductToReturn(Product productFromDataBase, string? EndUser = "")
        {
            var ProductReturn = _mapper.Map<Product, ProductToReturnDto>(productFromDataBase);

            // Mapp Images ===>
            if (productFromDataBase.OtherImagesOfProduct.Count() > 0)
            {
                ProductReturn.OtherImagesOfProduct = productFromDataBase.OtherImagesOfProduct.ToList();
                foreach (var item in ProductReturn.OtherImagesOfProduct)
                {
                    item.Image = $"{_configuration["ApiBaseUrl"]}{item.Image}";
                }
            }

            // Mapp UpSell ===>
            if (productFromDataBase.UpSells.Count() > 0)
            {
                ProductReturn.UpSalleToReturn = _mapper.Map<IEnumerable<UpSell>, IEnumerable<UpSalleToReturn>>(productFromDataBase.UpSells).ToList();
                ProductReturn.UpSalleToReturn = ProductReturn.UpSalleToReturn.Select(s => { s.UnitPrice = s.TotlePrice / s.Count; return s; }).ToList();
            }


            //// Cross Salling ===>
            if (productFromDataBase.CrossSelling != null)
            {
                ProductReturn.CrossSellingToReturnDto = _mapper.Map<CrossSelling, CrossSellingToReturnDto>(productFromDataBase.CrossSelling);
                if (EndUser != "")
                {
                    var spec = new ProductSpeification(productFromDataBase.Id);
                    var ProductIsShowInCrossSalling = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(spec);
                    ProductReturn.CrossSellingToReturnDto.ProductToRedirect = await ColGetProductToReturn(ProductIsShowInCrossSalling);
                    ProductReturn.CrossSellingToReturnDto.ProductToRedirect.CrossSellingToReturnDto = null;
                }
            }


            // Product Data ===>
            if (productFromDataBase.ProductData.Count() > 0 && productFromDataBase.Product_VariantsS.Count() > 0)
            {
                ProductReturn.ProductDataToReturnDto = _mapper.Map<IEnumerable<ProductData>, IEnumerable<ProductDataToReturnDto>>(productFromDataBase.ProductData).ToList();
                for (int i = 0; i < productFromDataBase.ProductData.Count(); i++)
                {
                    var OptinsReturn = new List<Product_Variant_OptionsToReturnDto>();

                    for (int j = 0; j < productFromDataBase.ProductData.ToList()[i].product_Variant_Options_ProductData.Count(); j++)
                    {
                        var Option = productFromDataBase.ProductData.ToList()[i].product_Variant_Options_ProductData.ToList()[j].Product_Variant_Option;
                        var OptionMapped = _mapper.Map<Product_Variant_Options, Product_Variant_OptionsToReturnDto>(Option);
                        OptinsReturn.Add(OptionMapped);
                    }
                    ProductReturn.ProductDataToReturnDto[i].Product_Variant_OptionsToReturnDto = OptinsReturn;

                }

                ProductReturn.Product_VariantsToReturnDto = _mapper.Map<IEnumerable<Product_Variants>, IEnumerable<Product_VariantsToReturnDto>>(productFromDataBase.Product_VariantsS).ToList();
                for (var i = 0; i < ProductReturn.Product_VariantsToReturnDto.Count(); i++)
                {
                    ProductReturn.Product_VariantsToReturnDto[i].Product_Variant_OptionsToReturnDto = _mapper.Map<IEnumerable<Product_Variant_Options>, IEnumerable<Product_Variant_OptionsToReturnDto>>(productFromDataBase.Product_VariantsS.ToList()[i].Product_Variant_OptionsS).ToList();
                }


            }


            return ProductReturn;
        }

        private async Task DeleteCachedResponse(string WordKey)
        {
            await _responseCachingService.DeleteCachAsync(WordKey);
        }


        [HttpGet("GetProductById")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int ProductId)
        {
            var ProducrByid = new ProductSpeification(ProductId);
            var productFromDataBase = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(ProducrByid);
            if (productFromDataBase != null)
            {
                //  Created  ====> ProductToReturn
                var ProductReturn = await GetProductToReturn(productFromDataBase);
                return Ok(ProductReturn);
            }
            else
            {
                return NotFound(new ApiResponse(400, "المعرف فير موجود"));
            }

        }



        [CachedAttribute(10)]
        [HttpGet("GetProductByLinkName")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ProductToReturnDto>> GetProductByLinkName(int DepartmentId, string LinkName)
        {
            var ProducrByid = new ProductSpeification(DepartmentId, LinkName);
            var productFromDataBase = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(ProducrByid);
            if (productFromDataBase != null)
            {
                //  Created  ====> ProductToReturn
                var ProductReturn = await GetProductToReturn(productFromDataBase, "EndUser");

                return Ok(ProductReturn);
            }
            else
            {
                return NotFound(new ApiResponse(400, "اسم اللينك غير موجود"));
            }

        }



        [HttpDelete("DeleteProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult<int>> DeleteProduct(int ProductId)
        {
            var ProducrByid = new ProductSpeification(ProductId);
            var productFromDataBase = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(ProducrByid);

            if (productFromDataBase != null)
            {
                //  Created  ====> ProductToReturn
                await _unitOfWork.Repository<ProductData>().DeletRangeAsync(productFromDataBase.ProductData);
                await _unitOfWork.CompletedAsynk();

                _unitOfWork.Repository<Product>().Delete(productFromDataBase);
                await _unitOfWork.CompletedAsynk();
                await DeleteCachedResponse($"GetProductByLinkName|DepartmentId-{productFromDataBase.DepartmentId}");
                await DeleteCachedResponse($"GetAllProductInDepartment|DepartmentId-{productFromDataBase.DepartmentId}");

                return Ok(200);
            }
            else
            {
                return NotFound(new ApiResponse(400, "المعرف فير موجود"));
            }

        }


        [HttpGet("GetAllProduct")]
        [ProducesResponseType(typeof(IEnumerable<ProductToReturnDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetAllProduct(int siteId, [FromQuery] QureyParams productParams)
        {
            var Spec = new ProductSpeification(siteId, 0, productParams);
            var AllProduct = await _unitOfWork.Repository<Product>().GetAllWithSpesificationAsync(Spec);

            if (AllProduct != null)
            {

                List<ProductToReturnDto> ProductsToReturn = new List<ProductToReturnDto>();

                foreach (var product in AllProduct)
                {
                    var ProducteReturn = await GetProductToReturn(product);
                    ProductsToReturn.Add(ProducteReturn);
                }

                return Ok(ProductsToReturn);
            }
            return NotFound(new ApiResponse(400, "معرف غير موجود"));
        }


        [CachedAttribute(10)]
        [HttpGet("GetAllProductInDepartment")]
        [ProducesResponseType(typeof(IEnumerable<ProductToReturnDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetAllProductInDepartment(int DepartmentId, [FromQuery] QureyParams productParams)
        {
            var Spec = new ProductSpeification(0, DepartmentId, productParams);
            var ProductInDepurtment = await _unitOfWork.Repository<Product>().GetAllWithSpesificationAsync(Spec);

            if (ProductInDepurtment != null)
            {

                List<ProductToReturnDto> ProductsToReturn = new List<ProductToReturnDto>();

                foreach (var product in ProductInDepurtment)
                {
                    var ProducteReturn = await GetProductToReturn(product);
                    ProductsToReturn.Add(ProducteReturn);
                }

                return Ok(ProductsToReturn);
            }
            return NotFound(new ApiResponse(400, "معرف غير موجود"));
        }


    }


}






