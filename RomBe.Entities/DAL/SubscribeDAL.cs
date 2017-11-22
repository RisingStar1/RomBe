using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.DAL
{
    public class SubscribeDAL
    {
        public bool Create(string email)
        {
            using (RombeEntities context = new RombeEntities())
            {
                Subscribe newSubscribe=new Subscribe()
                {
                    Email=email,
                    InsertDate=DateTime.Now
                };

                context.Subscribes.Add(newSubscribe);
                return Convert.ToBoolean(context.SaveChanges());
            }
        }
    }
}
