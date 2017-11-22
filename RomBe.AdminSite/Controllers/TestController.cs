using RomBe.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RomBe.AdminSite.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            Exception test = new Exception("test");
            LoggerHelper.Error(test);
            return View();
        }
	}
}