using RomBe.Entities.Class.Common;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Response
{
    public class GetUserDetailsResponse : BaseResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? LastPeriodDate { get; set; }
        public GenderTypeEnum? Gender { get; set; }
        public LanguagesEnum Language { get; set; }
        public int? Country { get; set; }
        public List<ChildObject> ChildsList { get; set; }
        public string Email { get; set; }
        public PreferencesObject Preferences { get; set; }
        public int UserId { get; set; }

        public GetUserDetailsResponse()
        {
            Preferences = new PreferencesObject();
        }
    }
    public class PreferencesObject
    {
        public bool NotificationsEnabled { get; set; }
    }


}
