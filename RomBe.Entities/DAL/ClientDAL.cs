using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.DAL
{
    public class ClientDAL
    {
        public Client FindClient(string clientId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    return context.Clients.Find(clientId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
