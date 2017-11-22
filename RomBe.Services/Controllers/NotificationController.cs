using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Helpers;
using RomBe.Logic.Notification;
using RomBe.Logic.PushNotification;
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
    public class NotificationController : BaseApiController
    {
        [HttpPost]
        [ResponseType(typeof(BaseResponse))]
        public async Task<HttpResponseMessage> SubscribeNotification([FromBody] SubscribeNotificationRequest request)
        {
            try
            {
                LoggerHelper.Info(request);
                BaseResponse response = await new NotificationLogic().SubscribeNotification(request);
                return GetFixedResponse(response.HttpStatusCode);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, request);
                return GlobalExeption();
            }
        }

        [HttpDelete]
        [ResponseType(typeof(BaseResponse))]
        public async Task<HttpResponseMessage> UnSubscribeNotification()
        {
            try
            {
                BaseResponse response = await new NotificationLogic().UnSubscribeNotification();
                return GetFixedResponse(response.HttpStatusCode);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }

        [HttpGet]
        [ResponseType(typeof(GetNotificationsResponse))]
        public async Task<HttpResponseMessage> GetNotifications()
        {
            try
            {
                return GetFixedResponse(await new NotificationLogic().GetNotifications());
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e);
                return GlobalExeption();
            }
        }

        [HttpPost]
        [ResponseType(typeof(BaseResponse))]
        public async Task<HttpResponseMessage> UpdateNotification(int notificationId)
        {
            try
            {
                LoggerHelper.Info(string.Format("notificationId: {0}",notificationId));
                return GetFixedResponse(await new NotificationLogic().UpdateNotification(notificationId));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, notificationId);
                return GlobalExeption();
            }
        }
    }
}
