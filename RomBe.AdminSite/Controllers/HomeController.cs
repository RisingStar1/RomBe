using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RomBe.Entities;
using RomBe.Services.Filters;
using System.Timers;
using RomBe.Entities.DAL;

namespace RomBe.CRUD.Controllers
{
    public class HomeController : Controller
    {
        private const String COOCKIE_NAME = "RombeLoginCookie";
        private Timer _loginTimer;
        private Timer LoginTimer
        {
            get
            {
                if (_loginTimer == null)
                {
                    //59 minutes
                    _loginTimer = new Timer(1000 * 60 * 59);
                }
                return _loginTimer;
            }
        }
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
            if (HttpContext.Request.Cookies.Get(COOCKIE_NAME) != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Login(CrudUser user)
        {
            if (ModelState.IsValid)
            {
                CrudUser u = db.CrudUsers.Where(c => c.Password == user.Password && c.Username == user.Username).FirstOrDefault();
                if (u != null && HttpContext.Request.Cookies.Get(COOCKIE_NAME) != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (u != null)
                {

                    int loginId = CreateLoginTime(u.UserId);
                    //SetLoginTimer(u.UserId);


                    HttpCookie cookie = new HttpCookie(COOCKIE_NAME);
                    cookie.Values["loginTime"] = DateTime.Now.ToString();
                    cookie.Values["CreatedBy"] = u.FullName;
                    cookie.Values["loginId"] = loginId.ToString();
                    if (u.CrudUserTypeId == 1)
                    {
                        cookie.Values["IsAdmin"] = true.ToString();
                    }
                    else
                        cookie.Values["IsAdmin"] = false.ToString();

                    cookie.Expires = DateTime.Now.AddDays(2);

                    if (HttpContext.Request.Cookies.Get(COOCKIE_NAME) != null)
                    {
                        HttpContext.Response.Cookies.Remove(COOCKIE_NAME);
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
        [LoggedIn]
        public ActionResult Logout()
        {
            //remove all the kyes and coockies that we created in the login

            HttpCookie cookie = HttpContext.Request.Cookies.Get(COOCKIE_NAME);
            String loginId = cookie.Values.Get("loginId");
            String loginTime = cookie.Values.Get("loginTime");
            new CrudUserDAL().UpdateLogoutTime(int.Parse(loginId), DateTime.Parse(loginTime));
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Response.SetCookie(cookie);
            LoginTimer.Dispose();

            return RedirectToAction("Login", "Home");
        }

        private void OnTimedEvent(int userId)
        {
            new CrudUserDAL().UpdateWorkHours(userId);

        }
        private void SetLoginTimer(int userId)
        {
            // Hook up the Elapsed event for the timer. 
            LoginTimer.Elapsed += delegate { OnTimedEvent(userId); };
            LoginTimer.Enabled = true;
            LoginTimer.AutoReset = false;

        }

        private int CreateLoginTime(int userId)
        {
            return new CrudUserDAL().CreateLoginTime(userId);
        }

    }


}
