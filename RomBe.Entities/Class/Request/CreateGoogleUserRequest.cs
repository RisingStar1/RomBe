using RomBe.Entities.Class.Common;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Request
{
    public class CreateGoogleUserRequest : CreateUserObject
    {
        public string GoogleUserId { get; set; }
        public GenderTypeEnum GenderType { get; set; }
    }
}
