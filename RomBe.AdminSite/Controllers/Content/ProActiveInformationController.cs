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
    [LoggedIn]
    public class ProActiveInformationController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /ProActiveInformation/
        public ActionResult Index()
        {
            var proactiveinformations = db.ProActiveInformations.Include(p => p.TaskCategory);
            return View(proactiveinformations.ToList());
        }

        // GET: /ProActiveInformation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProActiveInformation proactiveinformation = db.ProActiveInformations.Find(id);
            if (proactiveinformation == null)
            {
                return HttpNotFound();
            }
            return View(proactiveinformation);
        }

        // GET: /ProActiveInformation/Create
        public ActionResult Create()
        {
            ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: /ProActiveInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ProActiveInformationId,TaskCategoryId,PeriodMin,PeriodMax,CreatedBy,Subject")] ProActiveInformation proactiveinformation)
        {
            if (ModelState.IsValid)
            {
                proactiveinformation.CreatedBy = Session["CreatedBy"].ToString();
                db.ProActiveInformations.Add(proactiveinformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", proactiveinformation.TaskCategoryId);
            return View(proactiveinformation);
        }

        // GET: /ProActiveInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProActiveInformation proactiveinformation = db.ProActiveInformations.Find(id);
            if (proactiveinformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", proactiveinformation.TaskCategoryId);
            return View(proactiveinformation);
        }

        // POST: /ProActiveInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ProActiveInformationId,TaskCategoryId,PeriodMin,PeriodMax,CreatedBy,Subject")] ProActiveInformation proactiveinformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proactiveinformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", proactiveinformation.TaskCategoryId);
            return View(proactiveinformation);
        }

        // GET: /ProActiveInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProActiveInformation proactiveinformation = db.ProActiveInformations.Find(id);
            if (proactiveinformation == null)
            {
                return HttpNotFound();
            }
            return View(proactiveinformation);
        }

        // POST: /ProActiveInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProActiveInformation proactiveinformation = db.ProActiveInformations.Find(id);
            db.ProActiveInformations.Remove(proactiveinformation);
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
