using System.Net;

namespace NewsParserApi.Models
{
    public class BaseResponse<T>
    {
        public string Description { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
    }
}
