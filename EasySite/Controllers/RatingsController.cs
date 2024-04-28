using AutoMapper;
using Core.Entites.Product;
using EasySite.Core.Entites.Params;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs.productDTO;
using EasySite.DTOs.productDTO.ToReturn;
using EasySite.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingsController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        [HttpPost("AddRating")]
        [ProducesResponseType(typeof(RatingDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RatingDto>> AddRating(RatingDto ratingDto)
        {
            ratingDto.Id = 0;
            var RatingMapped= _mapper.Map<RatingDto, Ratings>(ratingDto);
            RatingMapped.DateCreated = DateTime.Now;
            var RatibAdded =await _unitOfWork.Repository<Ratings>().AddAsync(RatingMapped);
            await _unitOfWork.CompletedAsynk();

            var specProduct = new ProductSpeification(ratingDto.ProductId);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(specProduct);

            var SpecRating = new RatingSpesification(product.Id, 0);
            var ratings = await _unitOfWork.Repository<Ratings>().GetAllWithSpesificationAsync(SpecRating);

          
                var resule = (double) ratings.Sum(p => p.Count) / ratings.Count();

               product.Rating = Math.Round(resule, 1);
            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompletedAsynk();

            ratingDto.Id = RatibAdded.Id;
            return Ok(ratingDto);
        }

        [HttpPut("UpdateRating")]
        [ProducesResponseType(typeof(RatingDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult<RatingDto>> UpdateRating(RatingDto ratingDto)
        {
            var spec= new RatingSpesification(ratingDto.Id);
            var ratting=await _unitOfWork.Repository<Ratings>().GetByIdWithSpesificationAsync(spec);
            if (ratting is not null )
            {
                ratting.Id = ratingDto.Id;
                ratting.Count = ratingDto.Count;
                ratting.DateCreated = ratting.DateCreated;
                ratting.ShowRating = ratingDto.ShowRating;
                ratting.UerName = ratingDto.UerName;
                  
           
                _unitOfWork.Repository<Ratings>().Update(ratting);
                await _unitOfWork.CompletedAsynk();
                return Ok(ratingDto);
            }

            return BadRequest(new ApiResponse(404, "معرف غير موجود"));
          
        }


        [HttpDelete("DeleteRating")]
        [ProducesResponseType( 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult> DeleteRating(List<RatingDto> ratingDtos)
        {
            var retingeMapped = _mapper.Map<IEnumerable<RatingDto>,IEnumerable<Ratings>>(ratingDtos);
              await  _unitOfWork.Repository<Ratings>().DeletRangeAsync(retingeMapped);
              await _unitOfWork.CompletedAsynk();
              return Ok();

        }



        [HttpGet("GetAllRatingAndByProductId")]
        [ProducesResponseType(typeof(IEnumerable<Ratings>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<ActionResult<IEnumerable<Ratings>>> GetAllRatingAndByProductId ([FromQuery] QureyParams qureyParams , int? productId)
        {

            var ratingSpec = new RatingSpesification(qureyParams , productId);
            var ratings = await _unitOfWork.Repository<Ratings>().GetAllWithSpesificationAsync(ratingSpec);
            
            if (ratings.Count()>0)
            {
              var  RatingsMapped = _mapper.Map<IEnumerable<Ratings>, IEnumerable<RatingDto>>(ratings);
                return Ok(RatingsMapped);
            }
 
            List<Ratings> RatingsOtReturn = new List<Ratings>();
            return Ok(RatingsOtReturn);
        }


        [HttpGet("GetAllRatingEndUser")]
        [ProducesResponseType(typeof(RatingToReturn), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<RatingToReturn>> GetAllRatingByProductId( int productId)
        {
            var ratingSpecByProductId = new RatingSpesification( productId,0);
            var ratings = await _unitOfWork.Repository<Ratings>().GetAllWithSpesificationAsync(ratingSpecByProductId);

            var RatingToReturn = new RatingToReturn();
            if (ratings.Count() > 0)
            {

                RatingToReturn.Count = ratings.Count();
                var Groupes = ratings.GroupBy(p => p.Count);
                foreach ( var Item in Groupes )
                {
                    var count = Item.Count();
                    double result = (double)count / RatingToReturn.Count * 100;
                    var CtrRating = new CTRrating()
                    {
                        RatingNumber = Item.Key,
                        Percent = (int)Math.Floor(result)  
                    };
                    RatingToReturn.CTRrating.Add(CtrRating);
                }

                RatingToReturn.ratingDto = _mapper.Map<IEnumerable<Ratings>, IEnumerable<RatingDto>>(ratings).ToList();
                
            }

            return Ok(RatingToReturn);
        }


    }
}
