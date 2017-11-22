using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using RomBe.Entities.Enums;
using Newtonsoft.Json;
using System.Xml.Serialization;
using RomBe.Entities.Class.Common;

namespace RomBe.Entities.Class.Request
{
    public class CreateRombeUserRequest:CreateUserObject
    {
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])\w{8,20}$")]
        public String Password { get; set; }
    }
}
