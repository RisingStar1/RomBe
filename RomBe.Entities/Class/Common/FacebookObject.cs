using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Common
{
    public class FacebookObject
    {
        [Required]
        public string FacebookToken { get; set; }
        public string FacebookUserId { get; set; }
        public DateTime? BirthDate { get; set; }
        public GenderTypeEnum GenderType { get; set; }
    }
}
