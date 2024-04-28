namespace EasySite.Errors
{
    public class ApiExiptionRespons:ApiResponse
    {
        public string? Detalse { get; set; }
        public ApiExiptionRespons(int StetusCode , string? Message = null , string? detalse = null):base(StetusCode, Message)
        {
            Detalse = detalse;
        }
    }
}
