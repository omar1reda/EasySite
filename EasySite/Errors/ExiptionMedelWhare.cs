using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace EasySite.Errors
{
    public class ExiptionMedelWhare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExiptionMedelWhare> _logger;
        private readonly IHostEnvironment _environment;

        public ExiptionMedelWhare(RequestDelegate Next ,ILogger<ExiptionMedelWhare> logger , IHostEnvironment environment)
        {
            _next = Next;
            this._logger = logger;
            this._environment = environment;
        }

        public async Task InvokeAsync(HttpContext Context)
        {
            try
            {
             await _next.Invoke(Context);

            }
            catch(Exception ex)
            { 
                _logger.LogError(ex, ex.Message);
                Context.Response.ContentType = "application/json";
                Context.Response.StatusCode = 500;

                    var Respons = _environment.IsDevelopment() ? new ApiExiptionRespons(500, ex.Message, ex.StackTrace.ToString())
                                                               : new ApiExiptionRespons(500);

                    var option = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var JsonRespons= JsonSerializer.Serialize(Respons, option);

                    Context.Response.WriteAsync(JsonRespons);

              


            }
        }
    }
}
