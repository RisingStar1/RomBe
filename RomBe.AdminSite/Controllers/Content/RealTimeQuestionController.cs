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
    public class RealTimeQuestionController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /RealTimeQuestion/
        public ActionResult Index()
        {
            var realtimequestions = db.RealTimeQuestions.Include(r => r.TaskCategory);
            return View(realtimequestions.ToList());
        }

        // GET: /RealTimeQuestion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeQuestion realtimequestion = db.RealTimeQuestions.Find(id);
            if (realtimequestion == null)
            {
                return HttpNotFound();
            }
            return View(realtimequestion);
        }

        // GET: /RealTimeQuestion/Create
        public ActionResult Create()
        {
            ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: /RealTimeQuestion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RealTimeQuestionId,TaskCategoryId,PeriodMin,PeriodMax,Subject,Priority,CreatedBy")] RealTimeQuestion realtimequestion)
        {
            if (ModelState.IsValid)
            {
                realtimequestion.CreatedBy = Session["CreatedBy"].ToString();
                db.RealTimeQuestions.Add(realtimequestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", realtimequestion.TaskCategoryId);
            return View(realtimequestion);
        }

        // GET: /RealTimeQuestion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeQuestion realtimequestion = db.RealTimeQuestions.Find(id);
            if (realtimequestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", realtimequestion.TaskCategoryId);
            return View(realtimequestion);
        }

        // POST: /RealTimeQuestion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RealTimeQuestionId,TaskCategoryId,PeriodMin,PeriodMax,Subject,Priority,CreatedBy")] RealTimeQuestion realtimequestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(realtimequestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", realtimequestion.TaskCategoryId);
            return View(realtimequestion);
        }

        // GET: /RealTimeQuestion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeQuestion realtimequestion = db.RealTimeQuestions.Find(id);
            if (realtimequestion == null)
            {
                return HttpNotFound();
            }
            return View(realtimequestion);
        }

        // POST: /RealTimeQuestion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RealTimeQuestion realtimequestion = db.RealTimeQuestions.Find(id);
            db.RealTimeQuestions.Remove(realtimequestion);
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
