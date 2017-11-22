using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomBe.Entities;
using RomBe.Services.Filters;

namespace RomBe.AdminSite.Controllers.Management
{
    [IsAdmin]
    public class CrudUserController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /CrudUser/
        public ActionResult Index()
        {
            var crudusers = db.CrudUsers.Include(c => c.CrudUserType);
            return View(crudusers.ToList());
        }

        // GET: /CrudUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrudUser cruduser = db.CrudUsers.Find(id);
            if (cruduser == null)
            {
                return HttpNotFound();
            }
            return View(cruduser);
        }

        // GET: /CrudUser/Create
        public ActionResult Create()
        {
            ViewBag.CrudUserTypeId = new SelectList(db.CrudUserTypes, "CrudUserTypeId", "UserType");
            return View();
        }

        // POST: /CrudUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Username,Password,FullName,UserId,CrudUserTypeId")] CrudUser cruduser)
        {
            if (ModelState.IsValid)
            {
                cruduser.InsertDate = DateTime.Now;
                cruduser.UpdateDate = DateTime.Now;

                db.CrudUsers.Add(cruduser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CrudUserTypeId = new SelectList(db.CrudUserTypes, "CrudUserTypeId", "UserType", cruduser.CrudUserTypeId);
            return View(cruduser);
        }

        // GET: /CrudUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrudUser cruduser = db.CrudUsers.Find(id);
            if (cruduser == null)
            {
                return HttpNotFound();
            }
            ViewBag.CrudUserTypeId = new SelectList(db.CrudUserTypes, "CrudUserTypeId", "UserType", cruduser.CrudUserTypeId);
            return View(cruduser);
        }

        // POST: /CrudUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Username,Password,FullName,UserId,CrudUserTypeId")] CrudUser cruduser)
        {
            if (ModelState.IsValid)
            {
                cruduser.UpdateDate = DateTime.Now;

                db.Entry(cruduser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CrudUserTypeId = new SelectList(db.CrudUserTypes, "CrudUserTypeId", "UserType", cruduser.CrudUserTypeId);
            return View(cruduser);
        }

        // GET: /CrudUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrudUser cruduser = db.CrudUsers.Find(id);
            if (cruduser == null)
            {
                return HttpNotFound();
            }
            return View(cruduser);
        }

        // POST: /CrudUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CrudUser cruduser = db.CrudUsers.Find(id);
            db.CrudUsers.Remove(cruduser);
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
