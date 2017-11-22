using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using RomBe.Entities.Class.Common;
using RomBe.Helpers;

namespace RomBe.Entities.DAL
{
    public class EmailDAL
    {
        public async Task<ActivationEmailObject> GetActivationEmailDateAsync(string email)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return await (from user in context.Users
                              join ea in context.UserEmailActivations on user.UserId equals ea.UserId
                              where user.Email == email
                              select new ActivationEmailObject
                              {
                                  Email = user.Email,
                                  ActivationCode = ea.UserAccountActivationKey
                              }).FirstOrDefaultAsync();
            }
        }
        public async Task CreateEmailLog(string email)
        {
            using (RombeEntities context = new RombeEntities())
            {
                User _currentUser = context.Users.Where(u => u.Email == email).FirstOrDefault();
                if (_currentUser.IsNull())
                {
                    return;
                }

                EmailLog _newEmailLog = new EmailLog();
                _newEmailLog.UserId = _currentUser.UserId;
                _newEmailLog.SendDate = DateTime.Now;
                context.EmailLogs.Add(_newEmailLog);
                await context.SaveChangesAsync();
            }
        }
       
    }
}
