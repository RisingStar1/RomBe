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
    public class MonthlyCheckListsController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: MonthlyCheckLists
        public ActionResult Index()
        {
            var monthlyCheckLists = db.MonthlyCheckLists.Include(m => m.Language).Include(m => m.TaskCategory).Include(m => m.MonthsList);
            return View(monthlyCheckLists.ToList());
        }

        // GET: MonthlyCheckLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCheckList monthlyCheckList = db.MonthlyCheckLists.Find(id);
            if (monthlyCheckList == null)
            {
                return HttpNotFound();
            }
            return View(monthlyCheckList);
        }

        // GET: MonthlyCheckLists/Create
        public ActionResult Create()
        {
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName",2);
            ViewBag.CategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
            ViewBag.MonthId = new SelectList(db.MonthsLists, "MonthId", "Number");
            return View();
        }

        // POST: MonthlyCheckLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MonthlyCheckListId,Title,MonthId,LanguageId,CategoryId")] MonthlyCheckList monthlyCheckList)
        {
            if (ModelState.IsValid)
            {
                db.MonthlyCheckLists.Add(monthlyCheckList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", monthlyCheckList.LanguageId);
            ViewBag.CategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", monthlyCheckList.CategoryId);
            ViewBag.MonthId = new SelectList(db.MonthsLists, "MonthId", "Name", monthlyCheckList.MonthId);
            return View(monthlyCheckList);
        }

        // GET: MonthlyCheckLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCheckList monthlyCheckList = db.MonthlyCheckLists.Find(id);
            if (monthlyCheckList == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", monthlyCheckList.LanguageId);
            ViewBag.CategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", monthlyCheckList.CategoryId);
            ViewBag.MonthId = new SelectList(db.MonthsLists, "MonthId", "Name", monthlyCheckList.MonthId);
            return View(monthlyCheckList);
        }

        // POST: MonthlyCheckLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MonthlyCheckListId,Title,MonthId,LanguageId,CategoryId")] MonthlyCheckList monthlyCheckList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlyCheckList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", monthlyCheckList.LanguageId);
            ViewBag.CategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", monthlyCheckList.CategoryId);
            ViewBag.MonthId = new SelectList(db.MonthsLists, "MonthId", "Name", monthlyCheckList.MonthId);
            return View(monthlyCheckList);
        }

        // GET: MonthlyCheckLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyCheckList monthlyCheckList = db.MonthlyCheckLists.Find(id);
            if (monthlyCheckList == null)
            {
                return HttpNotFound();
            }
            return View(monthlyCheckList);
        }

        // POST: MonthlyCheckLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonthlyCheckList monthlyCheckList = db.MonthlyCheckLists.Find(id);
            db.MonthlyCheckLists.Remove(monthlyCheckList);
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
