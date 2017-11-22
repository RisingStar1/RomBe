using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Helpers;
using RomBe.Entities.Class.Request;

namespace RomBe.Entities.DAL
{
    public class MaintenanceDAL
    {
        public Boolean DeleteUser(String email)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    User userToDelete = context.Users.Where(u => u.Email == email).FirstOrDefault();
                    if (!userToDelete.IsNull())
                    {
                        List<UserDevice> listOfUserDevices = context.UserDevices.Where(ud => ud.UserId == userToDelete.UserId).ToList();
                        foreach (UserDevice item in listOfUserDevices)
                        {
                            context.UserDevices.Remove(item);
                        }

                        FacebookProvider facebookProvider = context.FacebookProviders.Where(f => f.UserId == userToDelete.UserId).FirstOrDefault();
                        if (!facebookProvider.IsNull())
                        {
                            context.FacebookProviders.Remove(facebookProvider);
                        }

                        UserEmailActivation userEmailActivation = context.UserEmailActivations.Where(u => u.UserId == userToDelete.UserId).FirstOrDefault();
                        if (!userEmailActivation.IsNull())
                        {
                            context.UserEmailActivations.Remove(userEmailActivation);
                        }

                        PregnantDetail pregnantDetail = context.PregnantDetails.Where(p => p.UserId == userToDelete.UserId).FirstOrDefault();
                        if (!pregnantDetail.IsNull())
                        {
                            context.PregnantDetails.Remove(pregnantDetail);
                        }

                        List<EmailLog> emailLogs = context.EmailLogs.Where(e => e.UserId == userToDelete.UserId).ToList();
                        foreach (EmailLog item in emailLogs)
                        {
                            context.EmailLogs.Remove(item);
                        }


                        

                        

                        List<Child> childsList = context.Children.Where(c => c.UserId == userToDelete.UserId).ToList();
                        foreach (Child item in childsList)
                        {
                            List<ChildActivity> childActivity = context.ChildActivities.Where(a => a.ChildId == item.ChildId).ToList();
                            List<ChildActivitiesHistory> childActivitiesHistory = context.ChildActivitiesHistories.Where(a => a.ChildId == item.ChildId).ToList();
                            List<ChildRealTimeSolutionActivity> childRealTimeSolutionActivity = context.ChildRealTimeSolutionActivities.Where(a => a.ChildId == item.ChildId).ToList();
                            List<ChildRealTimeSymptomsActivity> childRealTimeSymptomsActivities = context.ChildRealTimeSymptomsActivities.Where(a => a.ChildId == item.ChildId).ToList();

                            foreach (ChildRealTimeSolutionActivity sol in childRealTimeSolutionActivity)
                            {
                                context.ChildRealTimeSolutionActivities.Remove(sol);
                            }

                            foreach (ChildRealTimeSymptomsActivity sym in childRealTimeSymptomsActivities)
                            {
                                context.ChildRealTimeSymptomsActivities.Remove(sym);
                            }


                            foreach (ChildActivity activity in childActivity)
                            {
                                context.ChildActivities.Remove(activity);
                            }

                            foreach (ChildActivitiesHistory history in childActivitiesHistory)
                            {
                                context.ChildActivitiesHistories.Remove(history);
                            }

                            context.Children.Remove(item);
                        }
                        List<NotificationLog> notificationLogs = context.NotificationLogs.Where(n => n.UserId == userToDelete.UserId).ToList();
                        foreach (NotificationLog item in notificationLogs)
                        {
                            context.NotificationLogs.Remove(item);
                        }


                        context.Users.Remove(userToDelete);
                        int result = context.SaveChanges();
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

        public bool Duplicate(DuplicateRequest request)
        {
            using (RombeEntities context = new RombeEntities())
            {
                Duplicate _newDuplicate = new Entities.Duplicate()
                {
                    TaskId = request.TaskId,
                    PaginationType = request.PaginationType,
                    TaskType = request.TaskType,
                    InsertDate=DateTime.Now
                };
                context.Duplicates.Add(_newDuplicate);
                return Convert.ToBoolean(context.SaveChanges());
            }
        }

    }
}
