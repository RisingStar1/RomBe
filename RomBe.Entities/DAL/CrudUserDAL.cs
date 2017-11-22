using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.DAL
{
    public class CrudUserDAL
    {
        public void UpdateWorkHours(int userId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    CrudUser user = context.CrudUsers.Where(c => c.UserId == userId).FirstOrDefault();
                    if (user != null)
                    {
                        user.WorkingHoursInMinutes += 60;
                        user.UpdateDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CreateLoginTime(int userId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    CrudUsersLogin newLogin = new CrudUsersLogin();
                    newLogin.UserId = userId;
                    newLogin.LoginTime = DateTime.Now;
                    context.CrudUsersLogins.Add(newLogin);
                    context.SaveChanges();
                    return newLogin.CrudUsersLoginsId;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateLogoutTime(int loginId,DateTime loginTime)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    CrudUsersLogin login = context.CrudUsersLogins.Where(c => c.CrudUsersLoginsId == loginId).FirstOrDefault();
                    if (login != null)
                    {
                        login.LogoutTime = DateTime.Now;
                    }
                    CrudUser user = context.CrudUsers.Where(c => c.UserId == login.UserId).FirstOrDefault();
                    if (user != null)
                    {
                        user.WorkingHoursInMinutes += Convert.ToInt32(DateTime.Now.Subtract(loginTime).TotalMinutes);
                        user.UpdateDate = DateTime.Now;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
