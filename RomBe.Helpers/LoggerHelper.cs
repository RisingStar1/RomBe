using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Helpers
{
    public static class LoggerHelper
    {

        static LoggerHelper()
        {
            //Logger = new Log4NetLogger();
            Logger = new TelemetryClient();
        }

        public static TelemetryClient Logger { get; private set; }
        //public static ILogger Logger { get; private set; }

        public static void Error(Exception exp,Object request)
        {
            Logger.TrackTrace(new SerializeObjectHelper().ObjectToJson(request));
            Logger.TrackException(exp);
        }
        public static void Error(Exception exp)
        {
            Logger.TrackException(exp);
        }

        public static void Info(Object request)
        {
            Logger.TrackTrace(new SerializeObjectHelper().ObjectToJson(request));
        }
    }
}
