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

namespace RomBe.AdminSite.Controllers
{
    public class ChildController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /Child/
        public ActionResult Index()
        {
            var children = db.Children.Include(c => c.User);
            return View(children.ToList());
        }

        // GET: /Child/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Child child = db.Children.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        // GET: /Child/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Users, "UserId", "Username");
            return View();
        }

        // POST: /Child/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ParentId,ChildId,FirstName,MiddleName,LastName,DatePfBirth,GenderId")] Child child)
        {
           
                if (ModelState.IsValid)
                {
                    child.InsertDate = DateTime.Now;
                    child.UpdateDate = DateTime.Now;

                    db.Children.Add(child);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ParentId = new SelectList(db.Users, "UserId", "Username", child.UserId);
                return View(child);
            
        }

        // GET: /Child/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Child child = db.Children.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Users, "UserId", "Username", child.UserId);
            return View(child);
        }

        // POST: /Child/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ParentId,ChildId,FirstName,MiddleName,LastName,DatePfBirth,GenderId")] Child child)
        {
            if (ModelState.IsValid)
            {
                child.UpdateDate = DateTime.Now;

                db.Entry(child).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.Users, "UserId", "Username", child.UserId);
            return View(child);
        }

        // GET: /Child/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Child child = db.Children.Find(id);
            if (child == null)
            {
                return HttpNotFound();
            }
            return View(child);
        }

        // POST: /Child/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Child child = db.Children.Find(id);
            db.Children.Remove(child);
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
