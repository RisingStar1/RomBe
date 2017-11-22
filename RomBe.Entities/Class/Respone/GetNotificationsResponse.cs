using RomBe.Entities.Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Respone
{
    public class GetNotificationsResponse:BaseResponse
    {
        public List<NotificationObject> Notifications { get; set; }
        public GetNotificationsResponse()
        {
            Notifications = new List<NotificationObject>();
        }
    }
}
