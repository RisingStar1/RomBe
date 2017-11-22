using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomBe.Entities;

namespace RomBe.AdminSite.Controllers.Management
{
    public class SystemMessagesController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: SystemMessages
        public ActionResult Index()
        {
            var systemMessages = db.SystemMessages.Include(s => s.Language).Include(s => s.SystemMessagesType);
            return View(systemMessages.ToList());
        }

        // GET: SystemMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemMessage systemMessage = db.SystemMessages.Find(id);
            if (systemMessage == null)
            {
                return HttpNotFound();
            }
            return View(systemMessage);
        }

        // GET: SystemMessages/Create
        public ActionResult Create()
        {
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName");
            ViewBag.SystemMessageTypeId = new SelectList(db.SystemMessagesTypes, "SystemMessageTypeId", "SystemMessageType");
            return View();
        }

        // POST: SystemMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SystemMessageId,MessageCode,SystemMessageTypeId,MessageTitle,MessageContent,OKButtonText,CancelButtonText,Comments,LanguageId")] SystemMessage systemMessage)
        {
            if (ModelState.IsValid)
            {
                db.SystemMessages.Add(systemMessage);
                db.SaveChanges();
                UpdateIsConfigUpdated();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", systemMessage.LanguageId);
            ViewBag.SystemMessageTypeId = new SelectList(db.SystemMessagesTypes, "SystemMessageTypeId", "SystemMessageType", systemMessage.SystemMessageTypeId);
            return View(systemMessage);
        }

        // GET: SystemMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemMessage systemMessage = db.SystemMessages.Find(id);
            if (systemMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", systemMessage.LanguageId);
            ViewBag.SystemMessageTypeId = new SelectList(db.SystemMessagesTypes, "SystemMessageTypeId", "SystemMessageType", systemMessage.SystemMessageTypeId);
            return View(systemMessage);
        }

        // POST: SystemMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SystemMessageId,MessageCode,SystemMessageTypeId,MessageTitle,MessageContent,OKButtonText,CancelButtonText,Comments,LanguageId")] SystemMessage systemMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemMessage).State = EntityState.Modified;
                db.SaveChanges();
                UpdateIsConfigUpdated();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", systemMessage.LanguageId);
            ViewBag.SystemMessageTypeId = new SelectList(db.SystemMessagesTypes, "SystemMessageTypeId", "SystemMessageType", systemMessage.SystemMessageTypeId);
            return View(systemMessage);
        }

        // GET: SystemMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemMessage systemMessage = db.SystemMessages.Find(id);
            if (systemMessage == null)
            {
                return HttpNotFound();
            }
            return View(systemMessage);
        }

        // POST: SystemMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemMessage systemMessage = db.SystemMessages.Find(id);
            db.SystemMessages.Remove(systemMessage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void UpdateIsConfigUpdated()
        {
            GlobalParameter isConfigUpdated = db.GlobalParameters.Where(g => g.ParameterName == "IsConfigUpdated").FirstOrDefault();

            isConfigUpdated.ParameterValueInt = isConfigUpdated.ParameterValueInt + 1;
            db.Entry(isConfigUpdated).State = EntityState.Modified;
            db.SaveChanges();
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
