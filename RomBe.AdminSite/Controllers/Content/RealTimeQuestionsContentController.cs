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
    public class RealTimeQuestionsContentController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /RealTimeQuestionsContent/
        public ActionResult Index()
        {
            var realtimequestionscontents = db.RealTimeQuestionsContents.Include(r => r.Language).Include(r => r.RealTimeQuestion);
            return View(realtimequestionscontents.ToList());
        }

        // GET: /RealTimeQuestionsContent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeQuestionsContent realtimequestionscontent = db.RealTimeQuestionsContents.Find(id);
            if (realtimequestionscontent == null)
            {
                return HttpNotFound();
            }
            return View(realtimequestionscontent);
        }

        // GET: /RealTimeQuestionsContent/Create
        public ActionResult Create()
        {
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName");
            ViewBag.RealTimeQuestionId = new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject");
            return View();
        }

        // POST: /RealTimeQuestionsContent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RealTimeQuestionId,LanguageId,Question,Rank,RunningIndex,CreatedBy")] RealTimeQuestionsContent realtimequestionscontent)
        {
            if (ModelState.IsValid)
            {
                realtimequestionscontent.CreatedBy = Session["CreatedBy"].ToString();
                db.RealTimeQuestionsContents.Add(realtimequestionscontent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", realtimequestionscontent.LanguageId);
            ViewBag.RealTimeQuestionId = new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject", realtimequestionscontent.RealTimeQuestionId);
            return View(realtimequestionscontent);
        }

        // GET: /RealTimeQuestionsContent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeQuestionsContent realtimequestionscontent = db.RealTimeQuestionsContents.Find(id);
            if (realtimequestionscontent == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", realtimequestionscontent.LanguageId);
            ViewBag.RealTimeQuestionId = new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject", realtimequestionscontent.RealTimeQuestionId);
            return View(realtimequestionscontent);
        }

        // POST: /RealTimeQuestionsContent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RealTimeQuestionId,LanguageId,Question,Rank,RunningIndex,CreatedBy")] RealTimeQuestionsContent realtimequestionscontent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(realtimequestionscontent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", realtimequestionscontent.LanguageId);
            ViewBag.RealTimeQuestionId = new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject", realtimequestionscontent.RealTimeQuestionId);
            return View(realtimequestionscontent);
        }

        // GET: /RealTimeQuestionsContent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeQuestionsContent realtimequestionscontent = db.RealTimeQuestionsContents.Find(id);
            if (realtimequestionscontent == null)
            {
                return HttpNotFound();
            }
            return View(realtimequestionscontent);
        }

        // POST: /RealTimeQuestionsContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RealTimeQuestionsContent realtimequestionscontent = db.RealTimeQuestionsContents.Find(id);
            db.RealTimeQuestionsContents.Remove(realtimequestionscontent);
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
