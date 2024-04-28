
namespace EasySite.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Massege { get; set; }

        public ApiResponse(int statusCode , string? massege = null)
        {
            StatusCode = statusCode ;
            Massege = massege ?? GetDefulteMessegeForStatuseCode(StatusCode) ;
        }

        private string? GetDefulteMessegeForStatuseCode(int statuseCode)
        {

            // 500 ==> internel Server Error 
            // 400 ==> Bad Request
            // 401 ==> UnAuthorized
            // 404 ==> Not Found

            return StatusCode switch
            {
                500 => "internel Server Error",
                400 => "Bad Request",
                401 => "You Are Not Authorized",
                404 => "Resorce is Not Found",
                _ => null 
            };
        }
    }
}
