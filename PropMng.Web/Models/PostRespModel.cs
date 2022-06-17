using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;

namespace PropMng.Web.Models
{
    public class PostRespModel
    {
        public PostRespModel()
        {
            StatusCode = HttpStatusCode.OK;
        }
        public HttpStatusCode StatusCode { get; set; }
        public object GetT { get; set; }
        public bool IsSucceed { get; set; }
        public List<int> Errors { get; set; }
        public bool IsNotOk => StatusCode != HttpStatusCode.OK;

        public T GetDsrlzT<T>()
        {
            var a = GetT;
            if (GetT == null) return default;
            return JsonConvert.DeserializeObject<T>(GetT.ToString());
        }
    }
}
