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
    public class PeriodTypeController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /PeriodType/
        public ActionResult Index()
        {
            return View(db.PeriodTypes.ToList());
        }

        // GET: /PeriodType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodType periodtype = db.PeriodTypes.Find(id);
            if (periodtype == null)
            {
                return HttpNotFound();
            }
            return View(periodtype);
        }

        // GET: /PeriodType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /PeriodType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PeriodTypeId,PeriodTypeName")] PeriodType periodtype)
        {
            if (ModelState.IsValid)
            {
                periodtype.InsertDate = DateTime.Now;
                periodtype.UpdateDate = DateTime.Now;
                
                db.PeriodTypes.Add(periodtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(periodtype);
        }

        // GET: /PeriodType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodType periodtype = db.PeriodTypes.Find(id);
            if (periodtype == null)
            {
                return HttpNotFound();
            }
            return View(periodtype);
        }

        // POST: /PeriodType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PeriodTypeId,PeriodTypeName")] PeriodType periodtype)
        {
            if (ModelState.IsValid)
            {
                periodtype.UpdateDate = DateTime.Now;
                
                db.Entry(periodtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(periodtype);
        }

        // GET: /PeriodType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PeriodType periodtype = db.PeriodTypes.Find(id);
            if (periodtype == null)
            {
                return HttpNotFound();
            }
            return View(periodtype);
        }

        // POST: /PeriodType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PeriodType periodtype = db.PeriodTypes.Find(id);
            db.PeriodTypes.Remove(periodtype);
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
