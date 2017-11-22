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
    public class RealTimeAnswersContentController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /RealTimeAnswersContent/
        public ActionResult Index()
        {
            var realtimeanswerscontents = db.RealTimeAnswersContents.Include(r => r.Language).Include(r => r.RealTimeAnswer);
            return View(realtimeanswerscontents.ToList());
        }

        // GET: /RealTimeAnswersContent/Details/5
        public ActionResult Details(int? id,int languageId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeAnswersContent realtimeanswerscontent = db.RealTimeAnswersContents.Where(r => r.RealTimeAnswerId == id.Value &&
                                                            r.LanguageId == languageId).FirstOrDefault();
            if (realtimeanswerscontent == null)
            {
                return HttpNotFound();
            }
            return View(realtimeanswerscontent);
        }

        // GET: /RealTimeAnswersContent/Create
        public ActionResult Create()
        {
            
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName");
            ViewBag.RealTimeAnswerId = new SelectList(db.RealTimeAnswers, "RealTimeAnswerId", "CreatedBy");
            //ViewBag.RealTimeAnswerId=new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject");
            return View();
        }

        // POST: /RealTimeAnswersContent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="RealTimeAnswerId,LanguageId,AnswerContent,RunningIndex,CreatedBy")] RealTimeAnswersContent realtimeanswerscontent)
        {
            if (ModelState.IsValid)
            {
                realtimeanswerscontent.CreatedBy = Session["CreatedBy"].ToString();
                db.RealTimeAnswersContents.Add(realtimeanswerscontent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", realtimeanswerscontent.LanguageId);
            ViewBag.RealTimeAnswerId = new SelectList(db.RealTimeAnswers, "RealTimeAnswerId", "CreatedBy", realtimeanswerscontent.RealTimeAnswerId);
            return View(realtimeanswerscontent);
        }

        // GET: /RealTimeAnswersContent/Edit/5
        public ActionResult Edit(int? id, int languageId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeAnswersContent realtimeanswerscontent = db.RealTimeAnswersContents.Where(r => r.RealTimeAnswerId == id.Value &&
                                                            r.LanguageId == languageId).FirstOrDefault();
            if (realtimeanswerscontent == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", realtimeanswerscontent.LanguageId);
            ViewBag.RealTimeAnswerId = new SelectList(db.RealTimeAnswers, "RealTimeAnswerId", "CreatedBy", realtimeanswerscontent.RealTimeAnswerId);
            return View(realtimeanswerscontent);
        }

        // POST: /RealTimeAnswersContent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="RealTimeAnswerId,LanguageId,AnswerContent,RunningIndex,CreatedBy")] RealTimeAnswersContent realtimeanswerscontent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(realtimeanswerscontent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", realtimeanswerscontent.LanguageId);
            ViewBag.RealTimeAnswerId = new SelectList(db.RealTimeAnswers, "RealTimeAnswerId", "CreatedBy", realtimeanswerscontent.RealTimeAnswerId);
            return View(realtimeanswerscontent);
        }

        // GET: /RealTimeAnswersContent/Delete/5
        public ActionResult Delete(int? id, int languageId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeAnswersContent realtimeanswerscontent = db.RealTimeAnswersContents.Where(r => r.RealTimeAnswerId == id.Value &&
                                                            r.LanguageId == languageId).FirstOrDefault();
            if (realtimeanswerscontent == null)
            {
                return HttpNotFound();
            }
            return View(realtimeanswerscontent);
        }

        // POST: /RealTimeAnswersContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RealTimeAnswersContent realtimeanswerscontent = db.RealTimeAnswersContents.Find(id);
            db.RealTimeAnswersContents.Remove(realtimeanswerscontent);
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
