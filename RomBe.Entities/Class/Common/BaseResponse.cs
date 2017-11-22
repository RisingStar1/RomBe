using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Common
{
    public class BaseResponse
    {
        public String MessageCode { get; set; }
        [JsonIgnore]
        public HttpStatusCode HttpStatusCode { get; set; }

    }
}
