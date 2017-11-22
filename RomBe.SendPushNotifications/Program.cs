using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Entities;
using System.Data.Entity;
using RomBe.Entities.DAL;
using RomBe.Logic.Timeline;
using RomBe.Entities.Class.Request;
using RomBe.Entities.Class.Respone;
using RomBe.Helpers;
using RomBe.Entities.Class.Timeline;
using RomBe.Entities.Enums;
using RomBe.Logic.PushNotification;
using MoreLinq;
using RomBe.Entities.Class.Common;
using System.Threading;

namespace RomBe.SendPushNotifications
{
    class Program
    {
        PushNotificationLogic _pushNotificationLogic;
        PushNotificationLogic PushNotification
        {
            get
            {
                if (_pushNotificationLogic == null)
                {
                    _pushNotificationLogic = new PushNotificationLogic();
                }
                return _pushNotificationLogic;
            }
        }
        static void Main(string[] args)
        {
            string webJobName = Environment.GetEnvironmentVariable("WEBJOBS_NAME");

            RedisCacherHelper.Init();
            Program _program = new Program();
            List<ChildObject> _childsAgesList = new List<ChildObject>();
            List<DataObject> _childsList = _program.GetChildList(webJobName);

            
            //Build child list with age calculate
            foreach (DataObject child in _childsList)
            {
                _childsAgesList.Add(new ChildObject()
                {
                    ChildId = child.Child.ChildId,
                    ChildAgeInWeeks = _program.CalculateUserAgeInWeeks(child.Child.ChildId,child.User.LocalTime.Value)
                });
            }

            //for every week I take the childs in the week
            //and for each of them I build is current timeline
            for (int week = 0; week <= 54; week++)
            {
                List<GetTimelineDateResponse> _getTimelineDateResponseList = new List<GetTimelineDateResponse>();
                List<int> tempChildListIds = _childsAgesList.Where(a => a.ChildAgeInWeeks.Value == week).Select(a => a.ChildId).ToList();

                _childsList.Where(c => tempChildListIds.Contains(c.Child.ChildId));

                //get all timeline for current child list (per week)
                foreach (int childId in tempChildListIds)
                {
                    GetTimelineDateResponse _timeLine = _program.GetTimelineDateForChild(childId, week);

                    _getTimelineDateResponseList.Add(_timeLine);
                }

                foreach (GetTimelineDateResponse item in _getTimelineDateResponseList)
                {
                    DataObject _currentUser = _childsList.Where(c => c.Child.ChildId == item.ChildId).FirstOrDefault();
                    NotificationDAL _notificationDAL = new NotificationDAL();
                    RealTimeObject _realTimeTemp = null;
                    ProActiveObject proActiveTemp = null;
                    foreach (object task in item.TimelineContent.FirstOrDefault().WeekItems)
                    {
                        Notification _notificaion = null;
                        try
                        {
                            if (task.GetType() == typeof(RealTimeObject))
                            {
                                _realTimeTemp = task as RealTimeObject;
                                if (!_realTimeTemp.IsDone)
                                {
                                    _notificaion = _program.CreateRealTimeNotification(_realTimeTemp);
                                    if (_notificationDAL.IsNotificationAlreadySent(_currentUser.User.UserId, _notificaion.NotificationId))
                                    {
                                        _notificaion = null;
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                proActiveTemp = task as ProActiveObject;
                                if (!proActiveTemp.IsDone)
                                {
                                    //create notification
                                    if (proActiveTemp.Type == TaskTypeEnum.Tip)
                                    {
                                        _notificaion = _program.CreateTipNotification(proActiveTemp);
                                    }
                                    else
                                    {
                                        _notificaion = _program.CreateQuestionAndAnswerNotification(proActiveTemp);
                                    }
                                    if (_notificationDAL.IsNotificationAlreadySent(_currentUser.User.UserId, _notificaion.NotificationId))
                                    {
                                        _notificaion = null;
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(string.Format("Error message: {0} Child Id: {1} ", e.Message, item.ChildId));
                            continue;
                        }
                        finally
                        {
                            if (!_notificaion.IsNull())
                            {

                                int logId = _program.CreateNotificationLog(_currentUser.User.UserId, _notificaion.NotificationId, _currentUser.Child.ChildId);
                                ////create notification
                                AndroidJsonObject _androidJsonObject = new AndroidJsonObject()
                                {
                                    Message = _notificaion.Text.Replace("[BabyName]", _currentUser.Child.FirstName).Replace("[babyname]", _currentUser.Child.FirstName),
                                    Title = _notificaion.Title,
                                    NotificationId = logId,
                                    TaskId = _notificaion.TaskId,
                                    TaskType = (TaskTypeEnum)_notificaion.TaskTypeId,
                                    ChildId = _currentUser.Child.ChildId
                                };


                                foreach (UserDevice device in _currentUser.UserDevices)
                                {
                                    _program.PushNotification.Android(_androidJsonObject, device.PushToken);
                                    Console.WriteLine("Push sent to: {0} with logId: {1} and childId: {2}", device.PushToken, logId, _currentUser.Child.ChildId);
                                }

                                DataObject _childToDelete = _childsList.Where(c => c.Child.ChildId == item.ChildId).FirstOrDefault();
                                if (!_childToDelete.IsNull())
                                {
                                    _childsList.Remove(_childToDelete);
                                }
                            }
                        }
                    }
                }



            }
            _program.PushNotification.StopService();
        }

        private List<DataObject> GetChildList(string timeZoneName)
        {
            Timezone currentTimeZone = null;
            List<int> localTimes = new List<int>();
            using (RombeEntities context = new RombeEntities())
            {
                if (!string.IsNullOrEmpty(timeZoneName))
                {
                    currentTimeZone = context.Timezones.Where(t => t.TimezoneName == timeZoneName).FirstOrDefault();
                }
                else
                {
                    currentTimeZone = new Timezone();
                    currentTimeZone.TimezoneStart = -12;
                    currentTimeZone.TimezoneEnd = 14;
                }

                for(int i=currentTimeZone.TimezoneStart;i<=currentTimeZone.TimezoneEnd;i++)
                {
                    localTimes.Add(i);
                }


                return (from child in context.Children
                        join user in context.Users on child.UserId equals user.UserId
                        where user.SendNotifications && (localTimes.Contains(user.LocalTime.Value))

                        select new DataObject
                        {
                            Child = child,
                            UserDevices = user.UserDevices.Where(a => a.UserId == user.UserId).ToList(),
                            User = user
                        }).DistinctBy(a => a.Child.ChildId).ToList();

            }
        }
        private int? CalculateUserAgeInWeeks(int childId,int userLocalTime)
        {
            DateTime? _childBirthDate = new ChildDAL().GetChildBirthDate(childId);
            if (_childBirthDate.HasValue)
            {
                double temp = Math.Truncate((double)((DateTime.UtcNow.AddHours(userLocalTime) - _childBirthDate.Value).TotalDays / 7));
                return Convert.ToInt32(temp);
            }
            else
            {
                return null;
            }
        }
        private GetTimelineDateResponse GetTimelineDateForChild(int childId, int week)
        {
            GetTimelineContentPaginationRequest _request = new GetTimelineContentPaginationRequest();
            _request.ChildId = childId;
            _request.MinWeeks = week;
            _request.MaxWeeks = week;
            return new TimelineLogic().InitTimelineWithOutValidation(_request);

        }
        private Notification CreateRealTimeNotification(RealTimeObject task)
        {
            using (RombeEntities context = new RombeEntities())
            {
                Notification notificationExist = context.Notifications.Where(n => n.TaskId == task.Id && n.TaskTypeId == (int)TaskTypeEnum.RealTime).FirstOrDefault();
                if (notificationExist.IsNull())
                {
                    Notification newNotification = new Notification()
                    {
                        TaskId = task.Id,
                        TaskTypeId = (int)TaskTypeEnum.RealTime,
                        Text = GetFormatedText(task.Question, 500),
                        Title = GetFormatedText(task.TaskCategory, 70),
                        NotificationTypeId = (int)TaskTypeEnum.RealTime
                    };
                    context.Notifications.Add(newNotification);
                    context.SaveChanges();
                    return newNotification;
                }
                return notificationExist;
            }
        }
        private Notification CreateTipNotification(ProActiveObject task)
        {
            using (RombeEntities context = new RombeEntities())
            {
                Notification notificationExist = context.Notifications.Where(n => n.TaskId == task.Id && n.TaskTypeId == (int)TaskTypeEnum.Tip).FirstOrDefault();
                if (notificationExist.IsNull())
                {
                    Notification newNotification = new Notification()
                    {
                        TaskId = task.Id,
                        TaskTypeId = (int)TaskTypeEnum.Tip,
                        Text = GetFormatedText(task.Body, 500),
                        Title = GetFormatedText(task.Subject, 70),
                        NotificationTypeId = (int)TaskTypeEnum.Tip
                    };
                    context.Notifications.Add(newNotification);
                    context.SaveChanges();
                    return newNotification;
                }
                return notificationExist;
            }
        }
        private Notification CreateQuestionAndAnswerNotification(ProActiveObject task)
        {
            using (RombeEntities context = new RombeEntities())
            {
                Notification notificationExist = context.Notifications.Where(n => n.TaskId == task.Id && n.TaskTypeId == (int)TaskTypeEnum.QuestionAndAnswer).FirstOrDefault();
                if (notificationExist.IsNull())
                {
                    Notification newNotification = new Notification()
                    {
                        TaskId = task.Id,
                        TaskTypeId = (int)TaskTypeEnum.QuestionAndAnswer,
                        Text = GetFormatedText(task.Body, 500),
                        Title = GetFormatedText(task.Subject, 70),
                        NotificationTypeId = (int)TaskTypeEnum.QuestionAndAnswer
                    };
                    context.Notifications.Add(newNotification);
                    context.SaveChanges();
                    return newNotification;
                }
                return notificationExist;
            }
        }
        private int CreateNotificationLog(int userId, int notificaionId, int childId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                NotificationLog log = new NotificationLog()
                {
                    UserId = userId,
                    NotificationId = notificaionId,
                    SentDate = DateTime.Now,
                    IsSucceeded = true,
                    ChildId = childId
                };
                context.NotificationLogs.Add(log);
                context.SaveChanges();
                return log.NotificationLogId;
            }
        }

        private string GetFormatedText(string textToFormat, int lengthLimit)
        {
            string[] split = textToFormat.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (split.Length >= 4)
            {
                List<string> temp = split.Take(4).ToList();
                return string.Format("{0} {1} {2} {3}...", temp[0], temp[1], temp[2], temp[3]);
            }
            else
            {
                if (textToFormat.Length > lengthLimit)
                {
                    return textToFormat.Substring(0, 47) + "...";
                }
                else
                {
                    return textToFormat;
                }
            }
        }


    }

    class ChildObject
    {
        public int ChildId { get; set; }
        public int? ChildAgeInWeeks { get; set; }
    }

    class DataObject
    {
        public User User { get; set; }
        public Child Child { get; set; }
        public List<UserDevice> UserDevices { get; set; }
    }

}
