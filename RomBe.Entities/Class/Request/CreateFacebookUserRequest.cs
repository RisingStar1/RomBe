using Newtonsoft.Json;
using RomBe.Entities.Class.Common;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Request
{
    public class CreateFacebookUserRequest:CreateUserObject
    { 
        public string FacebookUserId { get; set; }
        public DateTime? BirthDate { get; set; }
        public GenderTypeEnum GenderType { get; set; }
    }
}
