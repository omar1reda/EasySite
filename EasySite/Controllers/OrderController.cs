using AutoMapper;
using Core.Entites;
using Core.Entites.orders;
using Core.Entites.Product;
using EasySite.Core.Entites;
using EasySite.Core.Entites.Baskets;
using EasySite.Core.Entites.Enums;
using EasySite.Core.Entites.Order;
using EasySite.Core.Entites.orders;
using EasySite.Core.Entites.Params;
using EasySite.Core.Entites.Product;
using EasySite.Core.I_Repository;
using EasySite.Core.Spesifications;
using EasySite.DataSeeding;
using EasySite.DTOs.OrdersDto;
using EasySite.DTOs.productDTO;
using EasySite.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork, IMapper mapper , IBasketRepository basketRepository , IConfiguration configuration, UserManager<AppUser> userManager)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._basketRepository = basketRepository;
            this._configuration = configuration;
            this._userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [ProducesResponseType( 200)]
        [HttpPut("BlockFakeOrders")]
        public async Task<ActionResult> BlockFakeOrders(BlockFakeOrdersDto blockFakeOrdersDto )
        {
            var spec = new SiteSpesification(blockFakeOrdersDto.SiteId);
            var site = await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(spec);

            site.IsBlockFakeOrders= blockFakeOrdersDto.IsBlockFakeOrders;
            site.MassegeBlockFakeOrders = blockFakeOrdersDto.MassegeBlockFakeOrders;
            site.TimeBlockFakeOrders= blockFakeOrdersDto.TimeBlockFakeOrders;

            _unitOfWork.Repository<Site>().Update(site);
            await _unitOfWork.CompletedAsynk();
            return Ok();
        }


        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(OrderToReturnDto), 200)]
        [HttpPost("AddOrderFromForm")]
        public async Task<ActionResult<OrderToReturnDto>> AddOrderFromForm(OrderFromForm orderForm , string? Utm_SourceCampaign, string? Utm_NameCampaign)
        {
            var SiteSpec = new SiteSpesification(orderForm.SiteId);
            var site = await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(SiteSpec);

            if (site == null)
                return BadRequest(new ApiResponse(400, "معرف الموقع غير صحيح"));

            // ===> Activation Block Fake Orders ====>>> 
            var IpAdders = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
           

            if ((bool) site.IsBlockFakeOrders)
            {
               
                var SpecOrderByIpAddres = new OrderSpesification(IpAdders, site.Id);
                var ordersByIPAddress = await _unitOfWork.Repository<Order>().GetAllWithSpesificationAsync(SpecOrderByIpAddres);
                if(ordersByIPAddress.Count()>=2)
                {
                    foreach (var orderIp in ordersByIPAddress)
                    {                       
                        if(orderIp.OrderDate.AddMinutes((int) site.TimeBlockFakeOrders) >= DateTime.Now )
                        {
                            return BadRequest(new ApiResponse(400,site.MassegeBlockFakeOrders));
                        }
                    }
                }

            }


 
            var user = await _userManager.FindByIdAsync(site.AppUserId);


            var items = _mapper.Map<IEnumerable< OrderItemDto>,IEnumerable< OrderItem>>(orderForm.OrderItemDto).ToList();

            for (int i = 0; i < items.Count(); i++)
            {
                if (orderForm.OrderItemDto[i].productDataInOrderDto != null)
                {
                    items[i].ProductDataInOrder = new ProductDataInOrder();
                    items[i].ProductDataInOrder.productDataId = orderForm.OrderItemDto[i].productDataInOrderDto.productDataId;
                    items[i].ProductDataInOrder.VariantOptionsOrder = _mapper.Map<IEnumerable<VariantOptionOrderDto>, IEnumerable<VariantOptionOrder>>(orderForm.OrderItemDto[i].productDataInOrderDto.VariantOptionOrderDto).ToList();

                }
            }
         

          

            var order = new Order()
            {
                Address = orderForm.Address,
                Country = orderForm.Country,
                Email = orderForm.Email,
                FullName = orderForm.FullName,
                Phone = orderForm.Phone,
                NumberShippingDays = orderForm.Government.NumberShippingDays,
                Government = orderForm.Government.GovernmentName,
                ShippingPrice = orderForm.Government.ShippingPrice,
                OrderItems = items,
                TotalPrice = orderForm.TotalPrice(),
                SiteId = orderForm.SiteId,
                IpAddres = IpAdders,
                Utm_NameCampaign= Utm_NameCampaign,
                Utm_SourceCampaign= Utm_SourceCampaign,
            };

            if(user.UserType== TypeUser.Pro&& user.YourAmount > 0)
            {
                order.PaymentMade= true;
                user.YourAmount = user.YourAmount - .02;
            }
            if (user.UserType == TypeUser.Pro && user.AmountDue >= 0 && user.YourAmount <= 0)
            {
                order.PaymentMade = false;
                user.AmountDue = user.AmountDue + .02;
            }
            if (user.UserType == TypeUser.Basic)
            {
                order.PaymentMade = true;
                user.YourAmount = user.YourAmount - .02;
            }

            await _userManager.UpdateAsync(user);

            var OrderAdded=  await  _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.CompletedAsynk();

            //   التي تم طلبها وثم التعديل علي عدد المخذون product استرجاع جميع ال
            foreach (var Productitem in OrderAdded.OrderItems)
            {
                // تقليل 1 من عدد المخزون
                await AfterCreatingOrder(Productitem);
            }
            var orderToReturn = await CreateOrderToReturn(OrderAdded);
            if (orderToReturn != null)
                return Ok(orderToReturn);

            return BadRequest(new ApiResponse(404, "مشكله في تكوين السله"));
        }

        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(OrderToReturnDto),200)]
        [HttpPost("AddOrderFromBasket")]
        public async Task<ActionResult<OrderToReturnDto>> AddOrderFromBasket(OrderFromBasketDto orderBasket , string? Utm_SourceCampaign, string? Utm_NameCampaign)
        {
            var SiteSpec = new SiteSpesification(orderBasket.SiteId);
            var site = await _unitOfWork.Repository<Site>().GetByIdWithSpesificationAsync(SiteSpec);
            if (site == null)
                return BadRequest(new ApiResponse(400, "معرف الموقع غير صحيح"));


            // ===> Activation Block Fake Orders ====>>> 
            var IpAdders = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            if ((bool)site.IsBlockFakeOrders)
            {

                var SpecOrderByIpAddres = new OrderSpesification(IpAdders, site.Id);
                var ordersByIPAddress = await _unitOfWork.Repository<Order>().GetAllWithSpesificationAsync(SpecOrderByIpAddres);
                if (ordersByIPAddress.Count() >= 2)
                {
                    foreach (var orderIp in ordersByIPAddress)
                    {
                        if (orderIp.OrderDate.AddMinutes((int)site.TimeBlockFakeOrders) >= DateTime.Now)
                        {
                            return BadRequest(new ApiResponse(400, site.MassegeBlockFakeOrders));
                        }
                    }
                }

            }


            var user = await _userManager.FindByIdAsync(site.AppUserId);

            var customerBasket =await _basketRepository.GetBasketAsync(orderBasket.BasketId);
            //var Items = new List<OrderItem>();
            if (customerBasket == null)
                return BadRequest(new ApiResponse(404, "id لا يوجد سله بهذا ال "));    

            var Items = _mapper.Map<IEnumerable<BasketItem>, IEnumerable<OrderItem>>(customerBasket.BasketItems).ToList();
            for (int i = 0; i < customerBasket.BasketItems.Count(); i++)
            {
                if (customerBasket.BasketItems[i].productDataBasket != null)
                {
                    Items[i].Id= 0 ;
                    Items[i].ProductDataInOrder = new ProductDataInOrder();
                    Items[i].ProductDataInOrder.productDataId = customerBasket.BasketItems.ToList()[i].productDataBasket.productDataId;
                    Items[i].ProductDataInOrder.VariantOptionsOrder = _mapper.Map<IEnumerable<VariantOptionBasket>, IEnumerable<VariantOptionOrder>>(customerBasket.BasketItems[i].productDataBasket.VariantOptions).ToList();                  
                }
            }

           
            var order = new Order()
            {
                Address = orderBasket.Address,
                Country = orderBasket.Country,
                Email = orderBasket.Email,
                FullName = orderBasket.FullName,
                Phone = orderBasket.Phone,
                NumberShippingDays = orderBasket.Government.NumberShippingDays,
                Government = orderBasket.Government.GovernmentName,
                ShippingPrice = orderBasket.Government.ShippingPrice,
                OrderItems = Items,
                TotalPrice = customerBasket.TotalPrice,  
                SiteId = orderBasket.SiteId,
                IpAddres= IpAdders,
                Utm_NameCampaign= Utm_NameCampaign,
                Utm_SourceCampaign= Utm_SourceCampaign,
            };


            if (user.UserType == TypeUser.Pro && user.YourAmount > 0)
            {
                order.PaymentMade = true;
                user.YourAmount = user.YourAmount - .02;
            }
            if (user.UserType == TypeUser.Pro && user.AmountDue >= 0 && user.YourAmount <= 0)
            {
                order.PaymentMade = false;
                user.AmountDue = user.AmountDue + .02;
            }
            if (user.UserType == TypeUser.Basic)
            {
                order.PaymentMade = true;
                user.YourAmount = user.YourAmount - .02;
            }

            await _userManager.UpdateAsync(user);
            await _unitOfWork.CompletedAsynk();

            var OrderAdded = await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.CompletedAsynk();
            await _basketRepository.DeleteBasketAsync(orderBasket.BasketId);
            
            //   التي تم طلبها وثم التعديل علي عدد المخذون product استرجاع جميع ال
            foreach (var Productitem in OrderAdded.OrderItems)
            {
                // تقليل 1 من عدد المخزون
                await AfterCreatingOrder(Productitem);
            }
            

            var orderToReturn = await CreateOrderToReturn(OrderAdded);
            if (orderToReturn != null)
                return Ok(orderToReturn);

            return BadRequest(new ApiResponse(404, "مشكله في تكوين السله"));
        }

        private async Task AfterCreatingOrder(OrderItem orderItem)
        {    
           
            var ProductSpec = new ProductSpeification(orderItem.ProductId);
            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpesificationAsync(ProductSpec);
            if (product != null)
            {
                // ليس متعدد الخصائص product  هذ ال
                if (product.Product_VariantsS.Count() == 0)
                {
                    // اذا وافق علي تتبع المخزون

                    // Update in Count Product
                    if (product.InventoryTracking)
                    {
                        if (product.Count > orderItem.Count)
                        {
                            product.Count = product.Count - orderItem.Count;
                        }
                        else
                        {
                            product.Count = 0;
                        }
                    }
                        
                    if (product.Disableproduct && product.Count<=0)
                    {
                        product.ProductSoldOut = true;
                    }

                    product.Count = product.Count >= orderItem.Count ? product.Count - orderItem.Count : product.Count = 0;
                    _unitOfWork.Repository<Product>().Update(product);
                    await _unitOfWork.CompletedAsynk(); 
                }
                // هذا المنتج متعدد الخصائص
                else
                {
                    var ProductDataSpec = new ProductDataSpecifacation(orderItem.ProductDataInOrder.productDataId,0);
                    var productData = await _unitOfWork.Repository<ProductData>().GetByIdWithSpesificationAsync(ProductDataSpec);
                    if (productData!=null)
                    {
                        // Update in Count product Data
                        if (product.InventoryTracking)
                        {
                            if (productData.Count > orderItem.Count)
                            {
                                productData.Count = productData.Count - orderItem.Count;
                            }
                            else
                            {
                                productData.Count = 0;
                            }
                        }

                        if (product.Disableproduct && productData.Count == 0)
                        {
                            productData.ProductSoldOut = true;
                        }

                        _unitOfWork.Repository<ProductData>().Update(productData);
                        await _unitOfWork.CompletedAsynk();
                    }
                }
            }
        }

        private async Task<OrderToReturnDto?> CreateOrderToReturn(Order order)
        {
            if (order == null)
                return null;

            var ItemsToReturn = _mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemDto>>(order.OrderItems).ToList();
            for (int i = 0; i < order.OrderItems.Count(); i++)
            { ///// create Base Linke from AppSitting ===>
                ItemsToReturn[i].ImageUrl = $"{_configuration["ApiBaseUrl"]}{ItemsToReturn[i].ImageUrl}";
                for (int j = 0; j < order.OrderItems.ToList()[i].ProductDataInOrder.VariantOptionsOrder.Count(); j++)
                {
                    ItemsToReturn[i].productDataInOrderDto = new productDataInOrderDto();
                    ItemsToReturn[i].productDataInOrderDto.productDataId= order.OrderItems.ToList()[i].ProductDataInOrder.productDataId;
                    ItemsToReturn[i].productDataInOrderDto.VariantOptionOrderDto = _mapper.Map<IEnumerable<VariantOptionOrder>, IEnumerable<VariantOptionOrderDto>>(order.OrderItems.ToList()[i].ProductDataInOrder.VariantOptionsOrder).ToList();
                }
               
            }
            var orderToReturnDto = new OrderToReturnDto()
            {
                 Id = order.Id,
                 Address= order.Address,
                 Country=order.Country,
                 Email=order.Email,
                 FullName=order.FullName,
                 Phone=order.Phone,
                 Government=order.Government,
                 NumberShippingDays=order.NumberShippingDays,
                 OrderDate = order.OrderDate,
                 Status = order.Status.ToString(),
                 TotalPrice = order.TotalPrice,
                 ShippingPrice=order.ShippingPrice,
                 OrderItemDto= ItemsToReturn,
                 SiteId=order.SiteId,
                 IsWatched=order.IsWatched,
                 Utm_SourceCampaign=order.Utm_SourceCampaign,
                 Utm_NameCampaign=order.Utm_NameCampaign,
            };

            return orderToReturnDto;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [ProducesResponseType(typeof(IEnumerable<OrderToReturnDto>), 200)]
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetAllOrders(int SiteId,[FromQuery] QureyParamsOrder qureyParamsOrder)
        {
           
            var spec = new OrderSpesification(SiteId, qureyParamsOrder);
            var OrderSpec = await _unitOfWork.Repository<Order>().GetAllWithSpesificationAsync(spec);

            List<OrderToReturnDto> OrderList = new List<OrderToReturnDto>();
            if(OrderSpec == null)
                return Ok(OrderList);

            foreach (var order in OrderSpec)
            {
                  var OrderToReturn= await  CreateOrderToReturn(order);
                OrderList.Add(OrderToReturn);
            }

            return Ok(OrderList);

        }

        [HttpDelete("DeleteOrders")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        public async Task<bool> DeleteOrder(IEnumerable<OrderToReturnDto> orders)
        {
            foreach (var item in orders)
            {
                var spec = new OrderSpesification(item.Id);
                var order = await _unitOfWork.Repository<Order>().GetByIdWithSpesificationAsync(spec);
                if (order != null)
                {
                    _unitOfWork.Repository<Order>().Delete(order);
                }               
            }
            await _unitOfWork.CompletedAsynk();

            return true;
        }
         
        [HttpGet("GetOrderById")]
    
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int OrderId)
        {
            var spec = new OrderSpesification(OrderId);
            var order= await  _unitOfWork.Repository<Order>().GetByIdWithSpesificationAsync(spec);
            if(order == null)
                return NotFound(new ApiResponse(404,"المعرف غير صحيح"));

            var orderToReturn = await CreateOrderToReturn(order);
            return Ok(orderToReturn);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpPut("UpdateOrder")]
        public async Task<ActionResult<OrderToReturnDto>> UpdateOrder(OrderUpdatedDto orderUpdated)
        {
            var spec = new OrderSpesification(orderUpdated.OrderId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpesificationAsync(spec);
            if (order == null)
                return NotFound(new ApiResponse(404, "المعرف غير صحيح"));

            order.IsWatched = true;
            order.Status = (StatusOrder) Enum.Parse(typeof(StatusOrder), orderUpdated.Status);
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompletedAsynk();

           var orderToReturn = await CreateOrderToReturn(order);
            return Ok(orderToReturn);
        }


        [ProducesResponseType( 400)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{CsRoles.User},{CsRoles.Manager}")]
        [HttpGet("GetResultsOrders")]
        public async Task<ActionResult<ResultsOrdersDto>> GetResultsOrders(int SiteId)
        {

            var OrderSpec = new OrderSpesification(SiteId,0);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpesificationAsync(OrderSpec);
            var resultsOrdersDto = new ResultsOrdersDto();
            if (orders != null)
            {
                // التاريخ الحالي
                DateTime currentDate = DateTime.Now;

                // تحديد يوم بداية الأسبوع (عادةً يكون الاثنين)
                DayOfWeek startOfWeek = DayOfWeek.Monday;

                // حساب بداية الأسبوع
                int diff = (7 + (currentDate.DayOfWeek - startOfWeek)) % 7;
                var startOfWeekDate = currentDate.AddDays(-1 * diff-2).Date;
                // حساب بداية الشهر 
                var startOfMounthDate = new DateTime(currentDate.Year, currentDate.Month, 1);


                resultsOrdersDto.TotleOrders = orders.Count();
                resultsOrdersDto.TotalOrdersLastWeek = orders.Where(o => o.OrderDate >= startOfWeekDate).Count();
                resultsOrdersDto.TotalOrdersLastMonth = orders.Where(o => o.OrderDate >= startOfMounthDate).Count();
                resultsOrdersDto.NewOrders = orders.Where(o => o.IsWatched == false).Count();

                resultsOrdersDto.TotalSales = orders.Sum(o => o.TotalPrice);
                resultsOrdersDto.TotalSalesLastMonth = orders.Where(o => o.OrderDate >= startOfMounthDate).Sum(o => o.TotalPrice);
                resultsOrdersDto.TotalSalesLastWeek = orders.Where(o => o.OrderDate >= startOfWeekDate).Sum(o => o.TotalPrice);

                return Ok(resultsOrdersDto);
            }

            return BadRequest(new ApiResponse(400 ,"معرف غير صحيح"));    
         
        }


    }
}
