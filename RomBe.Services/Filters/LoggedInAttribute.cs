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
            if (filterContext.HttpContext.Request.Cookies["loginCookie"] == null)
            {
                filterContext.HttpContext.Session.Remove("CreatedBy");
                filterContext.HttpContext.Session.Remove("IsAdmin");
                filterContext.Result = new RedirectToRouteResult(
                                           new RouteValueDictionary 
                                   {
                                       { "action", "Login" },
                                       { "controller", "Home" }
                                   });
            }
        }

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    HttpContext ctx = HttpContext.Current;

        //    //if (filterContext.HttpContext.Session["CreatedBy"] == null)
        //    //{
        //    //    filterContext.Result = new RedirectToRouteResult(
        //    //                               new RouteValueDictionary 
        //    //                       {
        //    //                           { "action", "Login" },
        //    //                           { "controller", "Home" }
        //    //                       });
        //    //}
        //    //in case that the cookie is expire
        //    //if (filterContext.HttpContext.Request.Cookies["loginCookie"] == null)
        //    //{
        //    //    filterContext.HttpContext.Session.Remove("CreatedBy");
        //    //    filterContext.HttpContext.Session.Remove("IsAdmin");
        //    //    filterContext.Result = new RedirectToRouteResult(
        //    //                               new RouteValueDictionary 
        //    //                       {
        //    //                           { "action", "Login" },
        //    //                           { "controller", "Home" }
        //    //                       });
        //    //}
        //    //else
        //    //{
        //    //    base.OnActionExecuting(filterContext);
        //    //}
        //}
    }
}