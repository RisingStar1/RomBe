using RomBe.Entities.Class.Respone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using RomBe.Helpers;
using RomBe.Logic.Config;
using RomBe.Entities.Class.Timeline;
using RomBe.Entities.DAL;
using System.Threading.Tasks;
using RomBe.Logic.Timeline;

namespace RomBe.Services
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = SystemConfigurationHelper.InstrumentationKey;


            RedisCacherHelper.Init();

            new ConfigLogic().SetConifgInMemoryCache();
            new TimelineLogic().SetProActiveTasksListInMemoryCache();
            new TimelineLogic().SetRealTimeTasksListInMemoryCache();


        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}