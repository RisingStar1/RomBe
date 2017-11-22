using RomBe.Entities.Class.Common;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Common
{
    public class NotificationObject: AndroidJsonObject
    {
        public bool IsRead { get; set; }
        public object Task { get; set; }
    }
}
