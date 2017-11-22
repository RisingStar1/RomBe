using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RomBe.Entities;
using RomBe.Services.Filters;

namespace RomBe.CRUD.Controllers
{
    public class HomeController : Controller
    {
        private const String COOCKIE_NAME = "loginCookie";
        private RombeEntities db = new RombeEntities();
        [LoggedIn]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(CrudUser user)
        {
            if (ModelState.IsValid)
            {
                CrudUser u = db.CrudUsers.Where(c => c.Password == user.Password && c.Username == user.Username).FirstOrDefault();
                if (u != null && Session["CreatedBy"] != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (u != null)
                {
                    //HttpContext.Session.Timeout = 1;
                    if (u.CrudUserTypeId == 1)
                        Session.Add("IsAdmin", true);
                    else
                        HttpContext.Session.Add("IsAdmin", false);

                    HttpContext.Session.Add("CreatedBy", u.FullName);


                    HttpCookie cookie = new HttpCookie("loginCookie");
                    cookie.Values["loginTime"] = DateTime.Now.ToString();
                    cookie.Expires = DateTime.Now.AddHours(1);

                    if (HttpContext.Request.Cookies.Get(COOCKIE_NAME) != null)
                    {
                        HttpContext.Request.Cookies.Remove(COOCKIE_NAME);
                    }
                    HttpContext.Response.SetCookie(cookie);

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(user);
        }

        [LoggedIn]
        [IsAdmin]
        public ActionResult Admin()
        {
            ViewBag.Title = "Admin Page";

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            //remove all the kyes and coockies that we created in the login
            Session.Remove("CreatedBy");
            Session.Remove("IsAdmin");
            
            HttpCookie cookie = new HttpCookie(COOCKIE_NAME);
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            
            return RedirectToAction("Login", "Home");
        }
    }


}
