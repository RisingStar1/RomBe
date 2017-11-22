using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using RomBe.Logic.Facebook;
using System.Globalization;
using System.Data.Entity;
using RomBe.Helpers;
using RomBe.Logic;
using RomBe.Entities.DAL;
using RomBe.Entities.Class.Timeline;
using Microsoft.ApplicationServer.Caching;

using StackExchange.Redis;
using Newtonsoft.Json;
using RomBe.Logic.Timeline;
using RomBe.Entities;
using RomBe.Services.Providers;
using Microsoft.Owin;
using System.IO;
using System.Text;
using System.Web;
using RomBe.Logic.Google;

namespace RomBe.Services.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : BaseApiController
    {

        [HttpGet]
        [ResponseType(typeof(String))]
        public HttpResponseMessage GetMe()
        { return null; }


        [HttpGet]
        public HttpResponseMessage Test()
        {
            try
            {
                new GoogleLogic().GetUserDetails(new Entities.Class.Request.GoogleLoginRequest()
                {
                    GoogleToken= "ya29.RQJDOu2VFuzIjsrgrXI2fxBDpMbj6aJiEsmTpd7nUkvgKw6OkH5GniGlsiI8suYW8J7g"

                });

                //new EmailLogic().SendContactUsEmailAsync(new Entities.Class.Request.ContactRequest()
                //{
                //    Email="cohenalGmail.com",
                //    Message="Hi!",
                //    Name="Almog Cohen",
                //    Subject="This is great"
                //});
                //RedisCacherHelper.Init();
                //  RedisCacherHelper.Remove(SystemConfigurationHelper.TimelineProActiveTasksCahceKey);
                //RedisCacherHelper.Remove(SystemConfigurationHelper.TimelineRealTimeTasksCahceKey);
                //RedisCacherHelper.Remove(SystemConfigurationHelper.ConfigurationnCahceKey);
                return null;
            }
            catch (Exception e)
            {
                return GlobalExeption();
            }

        }


        [HttpGet]
        //[Authorize]
        public async Task test1()
        {
            try
            {
                //var addr = new System.Net.Mail.MailAddress();
                //bool result = new ValidationHelper().IsValidEmail("j.@server1.proseware.com");
                await new EmailLogic().SendActivationEmailAsync("cohenal@gmail.com");
            }
            catch (Exception)
            {
            }
            //LoggerHelper.Logger.Info("sdasdas");

            //DateTime birthDate = DateTime.Parse("10/07/2014");

            //(int)((DateTime.Now - birthDate).TotalDays / 365.242199);

            //String result = new RomBe.Helpers.AuthenticationHelper().GetUserIdFromHttpContext();




            //FacebookLogic facebookLogic = new FacebookLogic();
            //facebookLogic.GetUserDetails("CAACEdEose0cBAAZAMZBOFtDFKDNJZBVqNwCM7NZAZB95UuZA35qQD1AATAV46ZC0Ycrr2ZA8aR4KCFWm4v2FLSd6ruYGupoWRrAxiqwtpAP2sHwAUX2cgrPy6V7UsOZAqBHmwmJZC08N1LhnShSKKG9aguNkwvoELlS4OIUr1OhnNgWN8lmrVrKE3BiNTpirvnCclYEkzFiZB3SZCsbBnZCy9MZAYFGQpdraVB3OgZD");

        }



    }

}
