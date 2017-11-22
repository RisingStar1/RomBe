using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Enums;
using RomBe.Helpers;
using RomBe.Logic.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace RomBe.Services.Controllers
{
    public class ConfigController : BaseApiController
    {
        [HttpGet]
        [ResponseType(typeof(GetConfigResponse))]
        public HttpResponseMessage GetConfig(LanguagesEnum language)
        {
            try
            {
                return GetFixedResponse(new ConfigLogic().GetConfig(language));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }

        [HttpGet]
        [ResponseType(typeof(IsConfigUpdatedResponse))]
        public HttpResponseMessage IsConfigUpdated(int lastUpdateCheck)
        {
            try
            {
                return GetFixedResponse(new ConfigLogic().IsConfigUpdated(lastUpdateCheck));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }
        [HttpGet]
        [ResponseType(typeof(IsVersionValidResponse))]
        public HttpResponseMessage IsVersionValid(OperatingSystemTypeEnum operatingSystemType, string currentVersion)
        {
            try
            {
                return GetFixedResponse(new ConfigLogic().IsVersionValid(operatingSystemType, currentVersion));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }
    }
}
