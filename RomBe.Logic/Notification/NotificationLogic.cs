using RomBe.Entities.Class;
using RomBe.Entities.Class.Request;
using RomBe.Entities.DAL;
using RomBe.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Helpers;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Class.Common;
using System.Net;
using RomBe.Logic.PushNotification;
using RomBe.Entities.Enums;
using RomBe.Logic.Timeline;
using RomBe.Entities.Class.Timeline;

namespace RomBe.Logic.Notification
{
    public class NotificationLogic
    {
        public async Task<BaseResponse> SubscribeNotification(SubscribeNotificationRequest request)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                response.HttpStatusCode = HttpStatusCode.BadRequest;

                int? userId = new AuthenticationHelper().GetCurrentUserId();
                if (!userId.HasValue)
                {
                    return response;
                }

                User _currentUser = new UserDAL().GetUser(userId.Value);
                if (_currentUser.IsNull())
                {
                    return response;
                }

                LoggerHelper.Info(string.Format("User Id: {0}", userId));
                Boolean updateResult = await new UserDeviceDAL().CreateUserDeviceAsync(request.UserDeviceObject, userId.Value).ConfigureAwait(false);

                if (updateResult)
                {
                    response.HttpStatusCode = HttpStatusCode.OK;
                }

                return response;



            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse> UnSubscribeNotification()
        {
            try
            {
                int? _currentUserId = new AuthenticationHelper().GetCurrentUserId();
                BaseResponse response = new BaseResponse();
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                if (!_currentUserId.HasValue)
                {
                    return response;
                }
                User _currentUser = new UserDAL().GetUser(_currentUserId.Value);
                if (_currentUser.IsNull())
                {
                    return response;
                }

                Boolean updateResult = await new NotificationDAL().UnSubscribeNotificationAsync(_currentUserId.Value).ConfigureAwait(false);
                if (updateResult)
                {
                    response.HttpStatusCode = HttpStatusCode.OK;
                }

                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse> UnSubscribeNotification(int userId)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                User _currentUser = new UserDAL().GetUser(userId);
                if (_currentUser.IsNull())
                {
                    return response;
                }

                Boolean updateResult = await new NotificationDAL().UnSubscribeNotificationAsync(userId).ConfigureAwait(false);
                if (updateResult)
                {
                    response.HttpStatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetNotificationsResponse> GetNotifications()
        {
            try
            {
                GetNotificationsResponse response = new GetNotificationsResponse();
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                int? _userId = new AuthenticationHelper().GetCurrentUserId();
                if (!_userId.HasValue)
                {
                    return response;
                }
                User _currentUser = new UserDAL().GetUser(_userId.Value);
                if (_currentUser.IsNull())
                {
                    return response;
                }
                List<NotificationObject> _notificationsList = await new NotificationDAL().GetNotificationsListByUserId(_userId.Value);
                TimelineLogic _timelineLogic = new TimelineLogic();
                TimelineDAL _timelineDAL = new TimelineDAL();
                foreach (NotificationObject item in _notificationsList)
                {
                    ChildActivity _currentItemActivity = await _timelineDAL.GetActiviy(item.ChildId, item.TaskId, item.TaskType);
                    if (item.TaskType == TaskTypeEnum.RealTime)
                    {
                        RealTimeObject _currentRealTimeItem = _timelineLogic.RealTimeTaskList.Where(r => r.Id == item.TaskId).FirstOrDefault();
                        if (!_currentItemActivity.IsNull())
                        {
                            _currentRealTimeItem.TaskStatus = (TaskStatusEnum)_currentItemActivity.TaskStatusId;
                            _timelineLogic.MarkContentInRealTime(_currentRealTimeItem, _currentItemActivity.ChildId);
                        }

                        item.Task = _currentRealTimeItem;
                    }
                    else
                    {
                        ProActiveObject _currentProActiveItem = _timelineLogic.ProActiveTasksList.Where(p => p.Id == item.TaskId).FirstOrDefault();
                        if (!_currentItemActivity.IsNull())
                        {
                            _currentProActiveItem.IsDone = true;
                            _currentProActiveItem.TaskStatus = (TaskStatusEnum)_currentItemActivity.TaskStatusId;
                        }

                        item.Task = _currentProActiveItem;
                    }
                }
                response.HttpStatusCode = HttpStatusCode.OK;
                response.Notifications = _notificationsList;
                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse> UpdateNotification(int notificationId)
        {
            BaseResponse response = new BaseResponse();
            bool updateResult = await new NotificationDAL().UpdateNotification(notificationId);
            if (updateResult)
            {
                response.HttpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                LoggerHelper.Info(notificationId);
                response.HttpStatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }
    }
}
