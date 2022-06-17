using System.Net;

namespace PropMng.Web.Data
{
    public class ServicesResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T GetT { get; set; }
    }
}
