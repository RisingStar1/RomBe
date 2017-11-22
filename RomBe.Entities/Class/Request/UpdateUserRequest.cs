using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Response;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Request
{
    public class UpdateUserRequest
    {
        public DateTime? BirthDate { get; set; }
        public GenderTypeEnum? Gender { get; set; }
        public int? Country { get; set; }
        public PregnantObject PregnantObject { get; set; }
        public LanguagesEnum? Language { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
