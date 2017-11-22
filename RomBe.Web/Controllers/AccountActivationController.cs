using RomBe.Helpers;
using RomBe.Logic.UserLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RomBe.Web.Controllers
{
    public class AccountActivationController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage Activation(string activationCode)
        {
            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri("http://www.rombe.me/AccountActivation/ActivationFailed.html");   
            try
            {
                HttpStatusCode statusCode =new UserLogic().AccountActivation(activationCode);
                if (statusCode == HttpStatusCode.OK)
                {
                    response.Headers.Location = new Uri("http://www.rombe.me/AccountActivation/ActivationSuccesfully.html");   
                }
               
                return response;
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e,activationCode);
                return response;

            }
        }
    }
}
