using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Helpers
{
    public class SystemConfigurationHelper
    {
        public static bool IsProduction
        {
            get
            {
                return ConfigurationManager.AppSettings["IS_PRODUCTION"].ToLower() == "true";
            }
        }
        public static string IphonePushCertificate
        {
            get
            {
                return ConfigurationManager.AppSettings["IphonePushCertification"];
            }
        }
        public static string IphonePushCertificateDev
        {
            get
            {
                return ConfigurationManager.AppSettings["IphonePushCertificateDev"];
            }
        }
        public static string IphonePushPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["IphonePushPassword"];
            }
        }
        public static string AndroidAPIKey
        {
            get
            {
                return ConfigurationManager.AppSettings["AndroidAPIKey"];
            }

        }
        public static string SmtpServerAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpServerAddress"];
            }
        }
        public static string EmailFromAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailFromAddress"];
            }
        }
        public static string EmailFromPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailFromPassword"];
            }
        }
        public static int SmtpServerPort
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["SmtpServerPort"]);
            }
        }
        public static string EmailActivationUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailActivationUrl"];
            }
        }
        public static string TimelineRealTimeTasksCahceKey
        {
            get
            {
                return ConfigurationManager.AppSettings["TimelineRealTimeTasksCahceKey"];
            }
        }
        public static string TimelineProActiveTasksCahceKey
        {
            get
            {
                return ConfigurationManager.AppSettings["TimelineProActiveTasksCahceKey"];
            }
        }
        public static string ConfigurationnCahceKey
        {
            get
            {
                return ConfigurationManager.AppSettings["ConfigurationnCahceKey"];
            }
        }
        public static int CacheExpirationInDays
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CacheExpirationInDays"]);
            }
        }
        public static string TimelinePreviousWeekContentUrl 
        {
            get
            {
                return ConfigurationManager.AppSettings["TimelinePreviousWeekContentUrl"];
            }
        }

        public static string TimelineNextWeekContentUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["GetNextWeeksTimelineContent"];
            }
        }
        public static string BaseApiUrl 
        {
            get
            {
                if(IsProduction)
                {
                    return ConfigurationManager.AppSettings["BaseApiUrl_Prod"];
                }
                else
                {
                    return ConfigurationManager.AppSettings["BaseApiUrl_Dev"];
                }
            }
        }

        public static string InstrumentationKey
        {
            get
            {
                if (IsProduction)
                {
                    return ConfigurationManager.AppSettings["InstrumentationKeyProd"];
                }
                else
                {
                    return ConfigurationManager.AppSettings["InstrumentationKeyDev"];
                }
            }
        }

        #region Message Codes

        public static string SucceedToRegisterViaRombeMessageCode
        {
            get
            {
                return ConfigurationManager.AppSettings["SucceedToRegisterViaRombeMessageCode"];
            }
        }

        public static string InternalErrorMessageCode
        {
            get
            {
                return ConfigurationManager.AppSettings["InternalErrorMessageCode"];
            }
        }

        public static string EmailIsExistMessageCode
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailIsExistMessageCode"];
            }
        }

        public static string CurrentPasswordNotMatchCode
        {
            get
            {
                return ConfigurationManager.AppSettings["CurrentPasswordNotMatchCode"];
            }
        }


        public static string EmailNotVerifidMessageCode
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailNotVerifidMessageCode"];
            }
        }
        
        #endregion Message Codes

    }
}
