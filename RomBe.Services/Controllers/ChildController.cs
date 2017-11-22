using RomBe.Entities;
using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Class.Response;
using RomBe.Helpers;
using RomBe.Logic.ChildLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RomBe.Services.Controllers
{
    [Authorize]
    public class ChildController : BaseApiController
    {
        [HttpPost]
        [ResponseType(typeof(GetUserDetailsResponse))]
        public async Task<HttpResponseMessage> CreateChild([FromBody] ChildObject request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(await new ChildLogic().CreateChild(request));

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpPost]
        [ResponseType(typeof(GetUserDetailsResponse))]
        public async Task<HttpResponseMessage> UpdateChild([FromBody]ChildObject request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(await new ChildLogic().UpdateChild(request));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpDelete]
        [ResponseType(typeof(GetUserDetailsResponse))]
        public async Task<HttpResponseMessage> DeleteChild([FromBody]DeleteChildRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                return GetFixedResponse(await new ChildLogic().DeleteChild(request));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }


    }
}
