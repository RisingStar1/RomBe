using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomBe.Entities;
using RomBe.Helpers;

namespace RomBe.AdminSite.Controllers
{
    public class ToeknClientsController : Controller
    {
        private RombeEntities db = new RombeEntities();

        // GET: /ToeknClients/
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: /ToeknClients/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: /ToeknClients/Create
        public ActionResult Create()
        {
            List<SelectListItem> applicationType = CreateApplicationTypeList();
            List<SelectListItem> activeList = IsActiveList();
            ViewBag.activeId = new SelectList(activeList, "Value", "Text");
            ViewBag.applicationTypeId = new SelectList(applicationType,"Value","Text");
            return View();
        }

        private List<SelectListItem> CreateApplicationTypeList()
        {
            List<SelectListItem> applicationType = new List<SelectListItem>();
            applicationType.Add(new SelectListItem());
            applicationType.Add(new SelectListItem());

            applicationType[0].Text = "JavaScript";
            applicationType[0].Value = "0";
            applicationType[1].Text = "NativeConfidential";
            applicationType[1].Value = "1";
            return applicationType;
        }

        private List<SelectListItem> IsActiveList()
        {
            List<SelectListItem> activeList = new List<SelectListItem>();
            activeList.Add(new SelectListItem());
            activeList.Add(new SelectListItem());

            activeList[0].Text = "False";
            activeList[0].Value = "0";
            activeList[1].Text = "True";
            activeList[1].Value = "1";
            return activeList;
        }

        // POST: /ToeknClients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Secret,Name,ApplicationType,Active,RefreshTokenLifeTime,AllowedOrigin")] Client client)
        {
            if (ModelState.IsValid)
            {
                String secretKey = client.Secret;
                client.Secret = new HashHelper().GetHash(secretKey);
                
                int refreshTokenLife = client.RefreshTokenLifeTime;
                client.RefreshTokenLifeTime = refreshTokenLife * 24 * 60;
                
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: /ToeknClients/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: /ToeknClients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Secret,Name,ApplicationType,Active,RefreshTokenLifeTime,AllowedOrigin")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: /ToeknClients/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: /ToeknClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
