using RomBe.Entities.Class.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Helpers;
using System.Data.Entity;
using RomBe.Entities.Class.Respone;
using RomBe.Entities.Class.Common;
using RomBe.Entities.Enums;

namespace RomBe.Entities.DAL
{
    public class NotificationDAL
    {
        public async Task<Boolean> UnSubscribeNotificationAsync(int userId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    User userToUpdate = context.Users.Where(u => u.UserId == userId).FirstOrDefault();
                    if (!userToUpdate.IsNull())
                    {
                        userToUpdate.SendNotifications = false;
                        userToUpdate.UpdateDate = DateTime.Now;
                        int result = await context.SaveChangesAsync().ConfigureAwait(false);
                        return Convert.ToBoolean(result);
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void MarkNotificaionAsFaild(List<string> pushTokensId, string errorMessage)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    foreach (string pushToken in pushTokensId)
                    {
                        int userId = context.UserDevices.Where(u => u.PushToken == pushToken).Select(u => u.UserId).FirstOrDefault();
                        NotificationLog log = context.NotificationLogs.Where(n => n.UserId == userId).OrderBy(n => n.SentDate).FirstOrDefault();
                        log.IsSucceeded = false;
                        log.ErrorMessage = errorMessage;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsNotificationAlreadySent(int userId,int notificationId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return context.NotificationLogs.Where(a => a.UserId == userId && a.NotificationId == notificationId).Any();
            }
        }

        public async Task<List<NotificationObject>> GetNotificationsListByUserId(int userId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return await (from notification in context.Notifications
                              join log in context.NotificationLogs on notification.NotificationId equals log.NotificationId
                              where log.UserId == userId && log.IsSucceeded.Value
                              orderby log.SentDate descending
                              select new NotificationObject()
                              {
                                  IsRead=log.IsRead,
                                  Message=notification.Text,
                                  TaskId=notification.TaskId,
                                  TaskType=(TaskTypeEnum)notification.TaskTypeId,
                                  Title=notification.Title,
                                  ChildId=log.ChildId,
                                  NotificationId=log.NotificationLogId
                              }).ToListAsync();

            }
        }
        
        public async Task<bool> UpdateNotification(int notificationId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                 NotificationLog recoredToUpdate= context.NotificationLogs.Where(n => n.NotificationLogId == notificationId).FirstOrDefault();
                if(!recoredToUpdate.IsNull())
                {
                    recoredToUpdate.IsRead = true;
                    return await context.SaveChangesAsync() > 0;
                }
                return false;
            }
            
        }

    }
}
