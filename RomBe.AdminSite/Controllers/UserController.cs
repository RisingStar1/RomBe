using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomBe.Entities;
using RomBe.AdminSite.Logic;

namespace RomBe.AdminSite.Controllers
{
    public class UserController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /User/
        public ActionResult Index()
        {
            return View(db.Users.Where(u => u.SendNotifications).ToList());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.InsertDate = DateTime.Now;
                user.UpdateDate = DateTime.Now;

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UpdateDate = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult SendPush(FormCollection frm)
        {
            List<string> temp = frm.AllKeys.Where(c => c.StartsWith("SendTo_")).ToList();
            List<string> _usersToSendTo = new List<string>();
            foreach (string item in temp)
            {
                _usersToSendTo.Add(item.Split('_')[1]);
            }
            if (_usersToSendTo.Count > 0)
            {
                if (frm["SendRegularPushMessage"] != null)
                {
                    new SendPushLogic().Send(_usersToSendTo);
                }
                else
                {
                    string _customMessageBody = frm["TxtCustomMessageBody"];
                    string _customMessageTitle = frm["TxtCustomMessageTitle"];
                    if (!string.IsNullOrEmpty(_customMessageBody))
                    {
                        new SendPushLogic().Send(_usersToSendTo, _customMessageBody, _customMessageTitle);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
