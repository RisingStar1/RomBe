﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RomBe.Services.Filters
{
    public class IsAdmin : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {


            if (filterContext.HttpContext.Session["IsAdmin"] == null || (bool)filterContext.HttpContext.Session["IsAdmin"] == false)

                filterContext.Result = new RedirectToRouteResult(
                                           new RouteValueDictionary 
                                   {
                                       { "action", "Index" },
                                       { "controller", "Home" }
                                   });
        }


    }
}