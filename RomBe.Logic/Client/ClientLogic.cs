using RomBe.Entities;
using RomBe.Entities.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Logic.ClientLogic
{
    public class ClientLogic
    {
        public Client FindClient(string clientId)
        {
            return new ClientDAL().FindClient(clientId);
        }
    }
}
