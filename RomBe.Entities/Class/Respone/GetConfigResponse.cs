using RomBe.Entities.Class.Common;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Respone
{
    public class GetConfigResponse : BaseResponse
    {
        public List<SystemMessageObject> SystemMessagesList { get; set; }
        public GetConfigResponse()
        {
            SystemMessagesList = new List<SystemMessageObject>();
        }
    }


    public class SystemMessageObject
    {
        public String Code { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public String OKButtonText { get; set; }
        public String CancelButtonText { get; set; }
       
    }
}
