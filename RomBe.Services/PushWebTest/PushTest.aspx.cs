using RomBe.Logic.PushNotification;
using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using RomBe.Entities;
using MoreLinq;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Class.Request;
using RomBe.Logic.Timeline;
using RomBe.Entities.Class.Common;
using System.Threading.Tasks;
using RomBe.Entities.DAL;
using RomBe.Entities.Class.Timeline;
using RomBe.Entities.Enums;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace Dreamworks.Services.PushWebTest
{
    public partial class PushTest : System.Web.UI.Page
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
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void OnClickSendPushIphone(object sender, EventArgs e)
        {
            LoggerHelper.Info("test push from web - Iphone");
            PushNotificationLogic pushLogic = new PushNotificationLogic();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("Type", TxtTypeIphone.Text);
            dictionary.Add("message", TxtMessageIphone.Text);
            pushLogic.Apple(TxtDeviceTokenIphone.Text, dictionary);
            pushLogic.StopService();
            LoggerHelper.Info(String.Format("push sent to {0} with message {1} and type {2}", TxtDeviceTokenIphone.Text, TxtMessageIphone.Text, TxtTypeIphone.Text));

            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Push Sent');", true);
        }
        protected void OnClickSendPushAndroid(object sender, EventArgs e)
        {
            LoggerHelper.Info("test push from web - Andoird");
            List<DataObject> _childsList = GetChildByEmail();
            List<ChildObject> _childsAgesList = new List<ChildObject>();


            //Build child list with age calculate
            foreach (DataObject child in _childsList)
            {
                _childsAgesList.Add(new ChildObject()
                {
                    ChildId = child.Child.ChildId,
                    ChildAgeInWeeks = CalculateUserAgeInWeeks(child.Child.ChildId)
                });
            }
            List<GetTimelineDateResponse> _getTimelineDateResponseList = new List<GetTimelineDateResponse>();




            //get all timeline for current child list (per week)
            foreach (ChildObject child in _childsAgesList)
            {
                GetTimelineDateResponse _timeLine = GetTimelineDateForChild(child.ChildId, child.ChildAgeInWeeks.Value);

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
                                _notificaion = CreateRealTimeNotification(_realTimeTemp);

                                break;

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
                                    _notificaion = CreateTipNotification(proActiveTemp);
                                }
                                else
                                {
                                    _notificaion = CreateQuestionAndAnswerNotification(proActiveTemp);
                                }
                                break;

                            }
                        }

                    }
                    catch (Exception exp)
                    {
                        LoggerHelper.Error(exp);

                    }
                    finally
                    {
                        if (!_notificaion.IsNull())
                        {

                            int logId = CreateNotificationLog(_currentUser.User.UserId, _notificaion.NotificationId, _currentUser.Child.ChildId);
                            //create notification
                            AndroidJsonObject _androidJsonObject = new AndroidJsonObject()
                            {
                                Message = _notificaion.Text.Replace("[BabyName]", _currentUser.Child.FirstName).Replace("[babyname]", _currentUser.Child.FirstName),
                                Title = _notificaion.Title,
                                NotificationId = logId,
                                TaskId = _notificaion.TaskId,
                                TaskType = (TaskTypeEnum)_notificaion.TaskTypeId,
                                ChildId = _currentUser.Child.ChildId,
                                IsActionable=ChkIsActionable.Checked
                            };
                            

                            foreach (UserDevice device in _currentUser.UserDevices)
                            {
                                PushNotification.Android(_androidJsonObject, device.PushToken);
                            }


                        }
                    }
                }
            }

            PushNotification.StopService();


            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Push Sent');", true);
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
                        Text = GetFormatedText(task.Question),
                        Title = task.TaskCategory,
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
                        Text = GetFormatedText(task.Body),
                        Title = task.Subject,
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
                        Text = GetFormatedText(task.Body),
                        Title = task.Subject,
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

        private GetTimelineDateResponse GetTimelineDateForChild(int childId, int week)
        {
            GetTimelineContentPaginationRequest _request = new GetTimelineContentPaginationRequest();
            _request.ChildId = childId;
            _request.MinWeeks = week;
            _request.MaxWeeks = week;
            return new TimelineLogic().InitTimelineWithOutValidation(_request);

        }
        private List<DataObject> GetChildByEmail()
        {
            using (RombeEntities context = new RombeEntities())
            {


                return (from child in context.Children
                        join user in context.Users on child.UserId equals user.UserId
                        //join device in context.UserDevices on user.UserId equals device.UserId
                        where user.Email == TxtEmail.Text
                        select new DataObject
                        {
                            Child = child,
                            UserDevices = user.UserDevices.Where(a => a.UserId == user.UserId).ToList(),
                            User = user
                        }).DistinctBy(a => a.Child.ChildId).ToList();

            }
        }

        private int? CalculateUserAgeInWeeks(int childId)
        {
            DateTime? _childBirthDate = new ChildDAL().GetChildBirthDate(childId);
            if (_childBirthDate.HasValue)
            {
                double temp = Math.Round((double)((DateTime.Now - _childBirthDate.Value).TotalDays / 7));
                return Convert.ToInt32(temp);
            }
            else
            {
                return null;
            }
        }

        private string GetFormatedText(string textToFormat)
        {
            string[] split = textToFormat.Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

            if (split.Length >= 4)
            {
                List<string> temp = split.Take(4).ToList();
                return string.Format("{0} {1} {2} {3}...", temp[0], temp[1], temp[2], temp[3]);
            }
            else
            {
                return textToFormat;
            }
        }

    }
    class DataObject
    {
        public User User { get; set; }
        public Child Child { get; set; }
        public List<UserDevice> UserDevices { get; set; }
    }
    class ChildObject
    {
        public int ChildId { get; set; }
        public int? ChildAgeInWeeks { get; set; }
    }
}