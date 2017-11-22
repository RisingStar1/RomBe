using Newtonsoft.Json.Linq;
using RomBe.Entities.Class.Request;
using RomBe.Helpers;
using RomBe.Logic.Subscribe;
using RomBe.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RomBe.Web.Controller
{
    public class SubscribeController : BaseApiController
    {
        [HttpPost]
        //[ResponseType(typeof(GetUserDetailsResponse))]
        public async Task<HttpResponseMessage> Subscribe([FromBody]SubscribeRequest request)
        {
            try
            {
                              
                return GetFixedResponse(await new SubscribeLogic().Subscribe(request.Email));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e,request);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
