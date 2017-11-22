using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Helpers
{
    public static class AdminLoggerMessageFormat
    {
        public static String GetMessage(String controller,String action, String message)
        {
            return String.Format("Controller: {0} Action: {1} Message: {2}",controller, action, message);
        }
    }
}
