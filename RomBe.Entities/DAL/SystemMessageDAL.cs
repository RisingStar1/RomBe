using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RomBe.Entities.DAL
{
    public class SystemMessageDAL
    {
        public async Task<SystemMessage> GetSystemMessageAsync(string messageCode)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return await context.SystemMessages.Where(s => s.MessageCode == messageCode).FirstOrDefaultAsync();
            }
        }
    }
}
