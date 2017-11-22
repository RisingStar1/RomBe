using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RomBe.AdminSite
{
    public class AdminHelper
    {
        private const String COOCKIE_NAME = "RombeLoginCookie";
        public String GetCreatedBy()
        {
            if (HttpContext.Current.Request.Cookies.Get(COOCKIE_NAME) != null)
            {
                return HttpContext.Current.Request.Cookies.Get(COOCKIE_NAME).Values["CreatedBy"];
            }
            else return "Admin";
        }
    }
}