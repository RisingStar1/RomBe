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
    public class ProActiveInformationContentController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /ProActiveInformationContent/
        public ActionResult Index()
        {
            var proactiveinformationcontents = db.ProActiveInformationContents.Include(p => p.Language).Include(p => p.ProActiveInformation);
            return View(proactiveinformationcontents.ToList());
        }

        // GET: /ProActiveInformationContent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProActiveInformationContent proactiveinformationcontent = db.ProActiveInformationContents.Find(id);
            if (proactiveinformationcontent == null)
            {
                return HttpNotFound();
            }
            return View(proactiveinformationcontent);
        }

        // GET: /ProActiveInformationContent/Create
        public ActionResult Create()
        {
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName");
            ViewBag.ProActiveInformationId = new SelectList(db.ProActiveInformations, "ProActiveInformationId", "Subject");
            return View();
        }

        // POST: /ProActiveInformationContent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ProActiveInformationId,LanguageId,Title,Information,CreatedBy")] ProActiveInformationContent proactiveinformationcontent)
        {
            if (ModelState.IsValid)
            {
                proactiveinformationcontent.CreatedBy = Session["CreatedBy"].ToString();
                db.ProActiveInformationContents.Add(proactiveinformationcontent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", proactiveinformationcontent.LanguageId);
            ViewBag.ProActiveInformationId = new SelectList(db.ProActiveInformations, "ProActiveInformationId", "CreatedBy", proactiveinformationcontent.ProActiveInformationId);
            return View(proactiveinformationcontent);
        }

        // GET: /ProActiveInformationContent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProActiveInformationContent proactiveinformationcontent = db.ProActiveInformationContents.Find(id);
            if (proactiveinformationcontent == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", proactiveinformationcontent.LanguageId);
            ViewBag.ProActiveInformationId = new SelectList(db.ProActiveInformations, "ProActiveInformationId", "CreatedBy", proactiveinformationcontent.ProActiveInformationId);
            return View(proactiveinformationcontent);
        }

        // POST: /ProActiveInformationContent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ProActiveInformationId,LanguageId,Title,Information,CreatedBy")] ProActiveInformationContent proactiveinformationcontent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proactiveinformationcontent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", proactiveinformationcontent.LanguageId);
            ViewBag.ProActiveInformationId = new SelectList(db.ProActiveInformations, "ProActiveInformationId", "CreatedBy", proactiveinformationcontent.ProActiveInformationId);
            return View(proactiveinformationcontent);
        }

        // GET: /ProActiveInformationContent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProActiveInformationContent proactiveinformationcontent = db.ProActiveInformationContents.Find(id);
            if (proactiveinformationcontent == null)
            {
                return HttpNotFound();
            }
            return View(proactiveinformationcontent);
        }

        // POST: /ProActiveInformationContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProActiveInformationContent proactiveinformationcontent = db.ProActiveInformationContents.Find(id);
            db.ProActiveInformationContents.Remove(proactiveinformationcontent);
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
