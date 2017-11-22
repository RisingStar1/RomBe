using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Request;
using RomBe.Entities.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Logic.Maintenance
{
    public class MaintenanceLogic
    {
        public Boolean DeleteUser(String email)
        {
            return new MaintenanceDAL().DeleteUser(email);
        }

        public HttpStatusCode Duplicate(DuplicateRequest request)
        {
            bool isCreated= new MaintenanceDAL().Duplicate(request);
            if(isCreated)
            {
                return HttpStatusCode.OK;
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
