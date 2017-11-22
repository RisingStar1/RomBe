using RomBe.Entities.Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Respone
{
    public class GetFieldsToShowResponse:BaseResponse
    {
        public Dictionary<string, bool> WelcomeScreen { get; set; }
        public Dictionary<string, bool> LoginScreen { get; set; }
        public Dictionary<string, bool> SignupScreen { get; set; }
        public Dictionary<string, bool> MainScreen { get; set; }
        public Dictionary<string, bool> SettingsScreen { get; set; }
        public Dictionary<string, bool> ProfileScreen { get; set; }
        public Dictionary<string, bool> NotificationsScreen { get; set; }
        public Dictionary<string, bool> MissingDetailsScreen { get; set; }

    }
}
