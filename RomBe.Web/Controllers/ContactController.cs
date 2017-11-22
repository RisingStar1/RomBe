using RomBe.Entities.Class.Request;
using RomBe.Helpers;
using RomBe.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RomBe.Web.Controllers
{
    public class ContactController : BaseApiController
    {
        [HttpPost]
        //[ResponseType(typeof(GetUserDetailsResponse))]
        public async Task<HttpResponseMessage> SendEmail([FromBody]ContactRequest request)
        {
            try
            {

                return GetFixedResponse(await new EmailLogic().SendContactUsEmailAsync(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e,request);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
