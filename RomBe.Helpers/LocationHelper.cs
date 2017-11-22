using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace RomBe.Helpers
{
    public class LocationHelper
    {
        public LocationResponse GetDetailsByIP()
        {
            string url = "http://ip-api.com/json/";

            string finalUrl = url + GetUserIp();

            WebClient webRequest = new WebClient();
            try
            {
                string json_response = webRequest.DownloadString(finalUrl);


                LocationResponse res = JsonConvert.DeserializeObject<LocationResponse>(json_response);

                if (res.Status.ToLower() == "success")
                {

                    return res;
                }

                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.Info(string.Format("Error in retriving location from ip {0}", ex));
                LoggerHelper.Error(ex);
                return null;
            }
        }

        public String GetUserIp()
        {
            //get the user IP
            string ipAdress = HttpContext.Current.Request.UserHostAddress;

            if (ipAdress.IsNull())
            {
                ipAdress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (ipAdress.IsNull())
                {
                    ipAdress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(':').FirstOrDefault();
                }
            }


            return ipAdress;
        }
    }

    public class LocationResponse
    {
        public string Status { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Timezone { get; set; }
    }
}