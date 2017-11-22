using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomBe.Entities;

namespace RomBe.AdminSite.Controllers
{
    public class MonthlyCheckupListsController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: MonthlyCheckupLists
        public ActionResult Index()
        {
            var monthlyCheckupLists = db.MonthlyCheckupLists.Include(m => m.Language).Include(m => m.TaskCategory).Include(m => m.MonthsList);
            return View(monthlyCheckupLists.ToList());
        }

        // GET: MonthlyCheckupLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCheckupList monthlyCheckupList = db.MonthlyCheckupLists.Find(id);
            if (monthlyCheckupList == null)
            {
                return HttpNotFound();
            }
            return View(monthlyCheckupList);
        }

        // GET: MonthlyCheckupLists/Create
        public ActionResult Create()
        {
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName",2);
            ViewBag.CategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
            ViewBag.MonthId = new SelectList(db.MonthsLists, "MonthId", "Number");
            return View();
        }

        // POST: MonthlyCheckupLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MonthlyCheckupListId,Title,MonthId,LanguageId,CategoryId")] MonthlyCheckupList monthlyCheckupList)
        {
            if (ModelState.IsValid)
            {
                db.MonthlyCheckupLists.Add(monthlyCheckupList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", monthlyCheckupList.LanguageId);
            ViewBag.CategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", monthlyCheckupList.CategoryId);
            ViewBag.MonthId = new SelectList(db.MonthsLists, "MonthId", "Name", monthlyCheckupList.MonthId);
            return View(monthlyCheckupList);
        }

        // GET: MonthlyCheckupLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCheckupList monthlyCheckupList = db.MonthlyCheckupLists.Find(id);
            if (monthlyCheckupList == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", monthlyCheckupList.LanguageId);
            ViewBag.CategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", monthlyCheckupList.CategoryId);
            ViewBag.MonthId = new SelectList(db.MonthsLists, "MonthId", "Name", monthlyCheckupList.MonthId);
            return View(monthlyCheckupList);
        }

        // POST: MonthlyCheckupLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MonthlyCheckupListId,Title,MonthId,LanguageId,CategoryId")] MonthlyCheckupList monthlyCheckupList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlyCheckupList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", monthlyCheckupList.LanguageId);
            ViewBag.CategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", monthlyCheckupList.CategoryId);
            ViewBag.MonthId = new SelectList(db.MonthsLists, "MonthId", "Name", monthlyCheckupList.MonthId);
            return View(monthlyCheckupList);
        }

        // GET: MonthlyCheckupLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCheckupList monthlyCheckupList = db.MonthlyCheckupLists.Find(id);
            if (monthlyCheckupList == null)
            {
                return HttpNotFound();
            }
            return View(monthlyCheckupList);
        }

        // POST: MonthlyCheckupLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonthlyCheckupList monthlyCheckupList = db.MonthlyCheckupLists.Find(id);
            db.MonthlyCheckupLists.Remove(monthlyCheckupList);
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
