using Newtonsoft.Json;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Common
{
    public class CreateUserObject
    {
        [EmailAddress]
        public String Email { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int LocalTime { get; set; }
        [Required]
        public UserDeviceObject UserDeviceObject { get; set; }
        public CampaignSourceEnum CampaignSource { get; set; }

        [JsonIgnore]
        public int CountryId { get; set; }
        [JsonIgnore]
        public string City { get; set; }
        [JsonIgnore]
        public string IpAddress { get; set; }
    }
}
