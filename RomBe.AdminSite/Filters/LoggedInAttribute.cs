using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RomBe.Services.Filters
{
    public class LoggedIn : AuthorizeAttribute //ActionFilterAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            //in case that the cookie is expire
            if (filterContext.HttpContext.Request.Cookies["RombeLoginCookie"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                                           new RouteValueDictionary 
                                   {
                                       { "action", "Login" },
                                       { "controller", "Home" }
                                   });

                
            }
        }

        
    }
}