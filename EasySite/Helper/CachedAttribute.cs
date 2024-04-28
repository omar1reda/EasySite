using EasySite.Core.I_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;
using System.Text;

namespace EasySite.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int expire;


        public CachedAttribute(int Expire)
        {
            expire = Expire;

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var CachingService = context.HttpContext.RequestServices.GetRequiredService<IResponseCachingService>();

            var CachKey = GEnrateCachKet(context.HttpContext.Request);

            ///get Data From Redise
            var CachData = await CachingService.GetCachAsync(CachKey);
            if(CachData  != null)
            {
                context.Result = new ContentResult()
                {
                    Content = CachData,
                    StatusCode = 200,
                    ContentType = "application/json"
                };
                return;
            }

             var ObjectToTeturnFromEndPoint=await  next.Invoke();
            if(ObjectToTeturnFromEndPoint.Result is OkObjectResult Result)
            {
                
                await CachingService.CachResponsAsync(CachKey, Result.Value, TimeSpan.FromHours(expire));
            }
        }

        private string GEnrateCachKet(HttpRequest request)
        {
            var CachKey = new StringBuilder();
            CachKey.Append(request.Path);

            foreach (var (Key,Value) in request.Query.OrderBy(m=>m.Key))
            {
                CachKey.Append($"|{Key}-{Value}");
            }

            return CachKey.ToString();
        }

    }
}
