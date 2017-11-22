using RomBe.Entities.Class.Request;
using RomBe.Helpers;
using RomBe.Logic.Maintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RomBe.Services.Controllers
{
    public class MaintenanceController : BaseApiController
    {
        [HttpDelete]
        public HttpResponseMessage DeleteUser(String email)
        {
            try
            {
                return GetFixedResponse(new MaintenanceLogic().DeleteUser(email));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }

        [HttpPost]
        public HttpResponseMessage Duplicate(DuplicateRequest request)
        {
            try
            {
                return GetFixedResponse(new MaintenanceLogic().Duplicate(request));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }
    }
}

//