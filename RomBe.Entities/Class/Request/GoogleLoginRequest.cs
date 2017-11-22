using RomBe.Entities.Class.Common;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Request
{
    public class GoogleLoginRequest
    {
        [Required]
        public string GoogleToken { get; set; }
        [Required]
        public UserDeviceObject UserDeviceObject { get; set; }
        public CampaignSourceEnum CampaignSource { get; set; }
    }
}
