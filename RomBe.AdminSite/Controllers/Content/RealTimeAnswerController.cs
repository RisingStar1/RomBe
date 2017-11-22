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
    public class RealTimeAnswerController : Controller
    {
        private RombeEntities db = new RombeEntities();
        
        // GET: /RealTimeAnswer/
        public ActionResult Index()
        {
            var realtimeanswers = db.RealTimeAnswers.Include(r => r.RealTimeQuestion);
            return View(realtimeanswers.ToList());
        }

        // GET: /RealTimeAnswer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeAnswer realtimeanswer = db.RealTimeAnswers.Find(id);
            if (realtimeanswer == null)
            {
                return HttpNotFound();
            }
            return View(realtimeanswer);
        }

        // GET: /RealTimeAnswer/Create
        public ActionResult Create()
        {
            ViewBag.RealTimeQuestionId = new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject");
            return View();
        }

        // POST: /RealTimeAnswer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RealTimeAnswerId,Rank,RealTimeQuestionId,CreatedBy")] RealTimeAnswer realtimeanswer)
        {
            if (ModelState.IsValid)
            {
                realtimeanswer.CreatedBy = Session["CreatedBy"].ToString();
                db.RealTimeAnswers.Add(realtimeanswer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RealTimeQuestionId = new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject", realtimeanswer.RealTimeQuestionId);
            return View(realtimeanswer);
        }

        // GET: /RealTimeAnswer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeAnswer realtimeanswer = db.RealTimeAnswers.Find(id);
            if (realtimeanswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.RealTimeQuestionId = new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject", realtimeanswer.RealTimeQuestionId);
            return View(realtimeanswer);
        }

        // POST: /RealTimeAnswer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RealTimeAnswerId,Rank,RealTimeQuestionId,CreatedBy")] RealTimeAnswer realtimeanswer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(realtimeanswer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RealTimeQuestionId = new SelectList(db.RealTimeQuestions, "RealTimeQuestionId", "Subject", realtimeanswer.RealTimeQuestionId);
            return View(realtimeanswer);
        }

        // GET: /RealTimeAnswer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RealTimeAnswer realtimeanswer = db.RealTimeAnswers.Find(id);
            if (realtimeanswer == null)
            {
                return HttpNotFound();
            }
            return View(realtimeanswer);
        }

        // POST: /RealTimeAnswer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RealTimeAnswer realtimeanswer = db.RealTimeAnswers.Find(id);
            db.RealTimeAnswers.Remove(realtimeanswer);
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
