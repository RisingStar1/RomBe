using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Common
{
    public class IsUserExistObject
    {
        public bool IsExist { get; set; }
        public RegistartionSourceTypeEnum UserRegistartionSourceType { get; set; }
        public string FacebookUserId { get; set; }
        public string GoogleUserId { get; set; }
        public bool IsActiveUser { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
