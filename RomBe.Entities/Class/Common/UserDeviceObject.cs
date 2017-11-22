using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Common
{
    public class UserDeviceObject
    {
        public String DeviceId { get; set; }
        public String DeviceName { get; set; }
        public OperatingSystemTypeEnum DeviceOS { get; set; }
        public String DeviceOsVersion { get; set; }
        public String PushToken { get; set; }
    }
}
