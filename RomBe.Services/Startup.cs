using RomBe.Services.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.Optimization;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security;
using RomBe.Helpers;

[assembly: OwinStartup(typeof(RomBe.Services.Startup))]

namespace RomBe.Services
{
    public class Startup
    {   
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(config);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            if (!SystemConfigurationHelper.IsProduction)
            {
                app.UseHandlerAsync((request, response, next) =>
                {
                    if (request.Path == "/") //app.Map using a regex exclude list would be better here so it doesn't fire for every request
                {
                        response.StatusCode = 301;
                        response.SetHeader("Location", "/Help");
                        return Task.Run(() => { });
                    }

                    return next();
                });

                app.Map("/help", appbuilder => app.UseHandlerAsync((request, response, next) => next()));
            }
            
            
            app.UseWebApi(config);
            
            
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions() {
            
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(120),
                Provider = new RombeAuthorizationServerProvider(),
                RefreshTokenProvider = new RombeRefreshTokenProvider()
                 

            };
            
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);


            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            //{
            //    AccessTokenProvider=new GuidProvider()
            //});

        }

        
        
    }

   




}