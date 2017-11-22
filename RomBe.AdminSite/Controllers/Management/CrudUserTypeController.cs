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
    public class CrudUserTypeController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /CrudUserType/
        public ActionResult Index()
        {
            return View(db.CrudUserTypes.ToList());
        }

        // GET: /CrudUserType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrudUserType crudusertype = db.CrudUserTypes.Find(id);
            if (crudusertype == null)
            {
                return HttpNotFound();
            }
            return View(crudusertype);
        }

        // GET: /CrudUserType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /CrudUserType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CrudUserTypeId,UserType")] CrudUserType crudusertype)
        {
            if (ModelState.IsValid)
            {
                crudusertype.InsertDate = DateTime.Now;
                crudusertype.UpdateDate = DateTime.Now;
                
                db.CrudUserTypes.Add(crudusertype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crudusertype);
        }

        // GET: /CrudUserType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrudUserType crudusertype = db.CrudUserTypes.Find(id);
            if (crudusertype == null)
            {
                return HttpNotFound();
            }
            return View(crudusertype);
        }

        // POST: /CrudUserType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CrudUserTypeId,UserType")] CrudUserType crudusertype)
        {
            if (ModelState.IsValid)
            {
                crudusertype.UpdateDate = DateTime.Now;
                
                db.Entry(crudusertype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crudusertype);
        }

        // GET: /CrudUserType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrudUserType crudusertype = db.CrudUserTypes.Find(id);
            if (crudusertype == null)
            {
                return HttpNotFound();
            }
            return View(crudusertype);
        }

        // POST: /CrudUserType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CrudUserType crudusertype = db.CrudUserTypes.Find(id);
            db.CrudUserTypes.Remove(crudusertype);
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
