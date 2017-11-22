using Newtonsoft.Json;
using RomBe.Entities.Class.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RomBe.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        public static HttpResponseMessage GetFixedResponse(object obj)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            if (obj == null || (obj is IList && obj.GetType().IsGenericType && ((IList)obj).Count == 0))
            {
                return new HttpResponseMessage((HttpStatusCode)204);
            }
            if ((obj.GetType() == typeof(BaseResponse) || obj.GetType().IsSubclassOf(typeof(BaseResponse))) && ((BaseResponse)obj).HttpStatusCode != 0)
            {
                statusCode = ((BaseResponse)obj).HttpStatusCode;
            }

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include
            });
            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"),
                StatusCode = statusCode
            };
        }

        public static HttpResponseMessage GetFixedResponse(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new HttpResponseMessage()
            {
                StatusCode = statusCode
            };
        }

        public static HttpResponseMessage GlobalExeption()
        {
            BaseResponse response = new BaseResponse()
            {
                HttpStatusCode = HttpStatusCode.InternalServerError,
                MessageCode = "I6"
            };
            return GetFixedResponse(response);
        }
    }
}