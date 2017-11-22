using RomBe.Entities.Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RomBe.Entities.Enums;

namespace RomBe.Entities.Class.Request
{
    public class FacebookLoginRequest
    {
        [Required]
        public string FacebookToken { get; set; }
        [Required]
        public UserDeviceObject UserDeviceObject { get; set; }
        public CampaignSourceEnum CampaignSource { get; set; }
    }
}
