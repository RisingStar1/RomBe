using RomBe.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using RomBe.Services.Models;
using System.Net;
using RomBe.Services.Filters;
using RomBe.Helpers;


namespace RomBe.AdminSite.Controllers
{
    [LoggedIn]
    public class ProActiveController : Controller
    {
        private RombeEntities db = new RombeEntities();
        //
        // GET: /ProActive/
        public ActionResult Index(String subjectSearch, String otherSearch)
        {
            try
            {
                ProActiveViewModel viewModel = new ProActiveViewModel(subjectSearch, otherSearch);
                return View(viewModel);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                          RouteData.Values["action"].ToString(), e.Message));

                return RedirectToAction("Index");
            }
        }

        //
        // GET: /ProActive/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //
        // GET: /ProActive/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
                ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", 2);
                ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", 2);
                ViewBag.ProActiveTypeId = new SelectList(db.ProActiveTypes, "ProActiveTypeId", "TypeName");
                return View(new ProActiveCreateModel());
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /ProActive/Create
        [HttpPost]
        //[Bind(Include = "ProActiveInformationId,TaskCategoryId,PeriodMin,PeriodMax,CreatedBy,Subject,LanguageId,Title,Information")] 
        public ActionResult Create(ProActiveCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (RombeEntities context = new RombeEntities())
                    {
                        model.ProActiveInformation.CreatedBy = new AdminHelper().GetCreatedBy();
                        model.ProActiveInformation.InsertDate = DateTime.Now;
                        model.ProActiveInformation.UpdateDate = DateTime.Now;


                        context.ProActiveInformations.Add(model.ProActiveInformation);
                        context.SaveChanges();
                        int newIndex = model.ProActiveInformation.ProActiveInformationId;
                        if (newIndex > 0)
                        {
                            model.ProActiveInformationContent.CreatedBy = new AdminHelper().GetCreatedBy(); ;
                            model.ProActiveInformationContent.ProActiveInformationId = newIndex;
                            model.ProActiveInformationContent.InsertDate = DateTime.Now;
                            model.ProActiveInformationContent.UpdateDate = DateTime.Now;

                            context.ProActiveInformationContents.Add(model.ProActiveInformationContent);
                            context.SaveChanges();
                        }
                    }

                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();

                    LoggerHelper.Info(errors);


                    ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
                    ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName");
                    ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", 2);
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /ProActive/Edit/5
        public ActionResult Edit(int? id, int languageId)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProActiveInformationContent proactiveinformationcontent = db.ProActiveInformationContents.Where(a => a.ProActiveInformationId == id && a.LanguageId == languageId).FirstOrDefault();
                ProActiveInformation proactiveinformation = db.ProActiveInformations.Find(id);
                if (proactiveinformationcontent == null || proactiveinformation == null)
                {
                    return HttpNotFound();
                }
                ProActiveCreateModel model = new ProActiveCreateModel();
                model.ProActiveInformation = proactiveinformation;
                model.ProActiveInformationContent = proactiveinformationcontent;

                ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
                ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName");
                ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", model.ProActiveInformation.PeriodTypeId);
                ViewBag.ProActiveTypeId = new SelectList(db.ProActiveTypes, "ProActiveTypeId", "TypeName", model.ProActiveInformation.ProActiveTypeId);

                return View(model);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }


        //
        // POST: /ProActive/Edit/5
        [HttpPost]
        public ActionResult Edit(ProActiveCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (RombeEntities context = new RombeEntities())
                    {
                        model.ProActiveInformation.UpdateDate = DateTime.Now;
                        model.ProActiveInformationContent.UpdateDate = DateTime.Now;

                        context.Entry(model.ProActiveInformationContent).State = EntityState.Modified;
                        context.Entry(model.ProActiveInformation).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();


                    LoggerHelper.Info(errors);
                }
                return RedirectToAction("Index");

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }

            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /ProActive/Delete/5
        public ActionResult Delete(int? id, int languageId)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProActiveInformationContent proactiveinformationcontent = db.ProActiveInformationContents.Where(a => a.ProActiveInformationId == id && a.LanguageId == languageId).FirstOrDefault();
                ProActiveInformation proactiveinformation = db.ProActiveInformations.Find(id);
                if (proactiveinformationcontent == null || proactiveinformation == null)
                {
                    return HttpNotFound();
                }
                ProActiveCreateModel model = new ProActiveCreateModel();
                model.ProActiveInformation = proactiveinformation;
                model.ProActiveInformationContent = proactiveinformationcontent;
                return View(model);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /ProActive/Delete/5
        [HttpPost]
        public ActionResult Delete(ProActiveCreateModel model)
        {
            try
            {

                int _proActiveInformationId = model.ProActiveInformationContent.ProActiveInformationId;
                int _languageId = model.ProActiveInformationContent.LanguageId;

                ProActiveInformationContent proActiveInformationContent = db.ProActiveInformationContents.Where(a => a.ProActiveInformationId == _proActiveInformationId && a.LanguageId == _languageId).FirstOrDefault();
                if (proActiveInformationContent != null)
                {

                    ProActiveInformation proActiveInformation = db.ProActiveInformations.Find(model.ProActiveInformation.ProActiveInformationId);
                    db.ProActiveInformations.Remove(proActiveInformation);

                    db.ProActiveInformationContents.Remove(proActiveInformationContent);
                    db.SaveChanges();


                }
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        public ActionResult Copy(int id)
        {

            try
            {
                ProActiveInformationContent proActiveInformationContent = db.ProActiveInformationContents.Where(a => a.ProActiveInformationId == id).FirstOrDefault();
                if (proActiveInformationContent != null)
                {

                    db.Entry(proActiveInformationContent).State = EntityState.Added;
                    ProActiveInformation proActiveInformation = db.ProActiveInformations.Find(id);
                    db.Entry(proActiveInformation).State = EntityState.Added;
                    db.SaveChanges();


                }
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }
    }
}
