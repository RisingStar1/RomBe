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
    [IsAdmin]
    public class TaskCategoryController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /TaskCategory/
        public ActionResult Index()
        {
            var taskcategories = db.TaskCategories.Include(t => t.PeriodType);
            return View(taskcategories.ToList());
        }

        // GET: /TaskCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskCategory taskcategory = db.TaskCategories.Find(id);
            if (taskcategory == null)
            {
                return HttpNotFound();
            }
            return View(taskcategory);
        }

        // GET: /TaskCategory/Create
        public ActionResult Create()
        {
            ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName");
            return View();
        }

        // GET: /TaskCategory/Create
        [AllowAnonymous]
        public ActionResult CreatePopup()
        {
            ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName");
            return View();
        }

        // POST: /TaskCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CategoryId,CategoryName,PeriodTypeId")] TaskCategory taskcategory)
        {
            if (ModelState.IsValid)
            {
                taskcategory.InsertDate = DateTime.Now;
                taskcategory.UpdateDate = DateTime.Now;
                
                db.TaskCategories.Add(taskcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", taskcategory.PeriodTypeId);
            return View(taskcategory);
        }


        // POST: /TaskCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreatePopup([Bind(Include = "CategoryId,CategoryName,PeriodTypeId")] TaskCategory taskcategory)
        {
            if (ModelState.IsValid)
            {
                taskcategory.InsertDate = DateTime.Now;
                taskcategory.UpdateDate = DateTime.Now;

                db.TaskCategories.Add(taskcategory);
                db.SaveChanges();
                return View("Close");
            }

            ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", taskcategory.PeriodTypeId);
            return View(taskcategory);
        }

        // GET: /TaskCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskCategory taskcategory = db.TaskCategories.Find(id);
            if (taskcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", taskcategory.PeriodTypeId);
            return View(taskcategory);
        }

        // POST: /TaskCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,PeriodTypeId,InsertDate")] TaskCategory taskcategory)
        {
            if (ModelState.IsValid)
            {
                taskcategory.UpdateDate = DateTime.Now;
                
                db.Entry(taskcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", taskcategory.PeriodTypeId);
            return View(taskcategory);
        }

        // GET: /TaskCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskCategory taskcategory = db.TaskCategories.Find(id);
            if (taskcategory == null)
            {
                return HttpNotFound();
            }
            return View(taskcategory);
        }

        // POST: /TaskCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaskCategory taskcategory = db.TaskCategories.Find(id);
            db.TaskCategories.Remove(taskcategory);
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
