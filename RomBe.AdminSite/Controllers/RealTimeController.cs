using RomBe.Services.Models;
using RomBe.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RomBe.Services.Filters;
using RomBe.Helpers;


namespace RomBe.AdminSite.Controllers
{
    [LoggedIn]
    public class RealTimeController : Controller
    {

        private RombeEntities db = new RombeEntities();
        //
        // GET: /RealTime/
        public ActionResult Index(String questionSearch)
        {
            try
            {
                RealTimeViewModel viewModel = new RealTimeViewModel(questionSearch);
                
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
        // GET: /RealTime/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public JsonResult TaskCategoryList(int periodTypeId)
        {
            try
            {
                // normally, you pass a list obtained from ORM or ADO.NET DataTable or DataReader
                return Json(new SelectList(db.TaskCategories.Where(t => t.PeriodTypeId == periodTypeId), "CategoryId", "CategoryName"));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return null;
            }
        }

        
        //
        // GET: /RealTime/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
                ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", 2);
                ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", 2);
                return View(new RealTimeCreateModel());
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddNewSolution(int id)
        {
            try
            {
                ViewBag.id = id;
                return View("Partial/AddNewSolution");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddNewDetectionWay(int id)
        {
            try
            {
                ViewBag.id = id;
                return View("Partial/AddNewDetectionWay");
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
                return RedirectToAction("Index");
            }
        }

        //
        // POST: /RealTime/Create
        [HttpPost]
        public ActionResult Create(RealTimeCreateModel request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", request.RealTimeLeadingQuestion.TaskCategoryId);
                    ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", request.RealTimeLeadingQuestionContent.LanguageId);
                    ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", request.RealTimeLeadingQuestion.PeriodTypeId);
                    var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                    return View(request);
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();

                    
                    LoggerHelper.Info(errors);
                }
                using (RombeEntities context = new RombeEntities())
                {
                    int _languageId = request.RealTimeLeadingQuestionContent.LanguageId;

                    #region RealTimeLeadingQuestionContent


                    request.RealTimeLeadingQuestion.CreatedBy = new AdminHelper().GetCreatedBy();
                    request.RealTimeLeadingQuestion.InsertDate = DateTime.Now;
                    request.RealTimeLeadingQuestion.UpdateDate = DateTime.Now;

                    context.RealTimeLeadingQuestions.Add(request.RealTimeLeadingQuestion);
                    context.SaveChanges();
                    #endregion RealTimeLeadingQuestionContent

                    int realTimeLeadingQuestionId = request.RealTimeLeadingQuestion.RealTimeLeadingQuestionId;

                    #region RealTimeDetectionWayCongratulation

                    request.RealTimeSymptomsCongratulation.CreatedBy = new AdminHelper().GetCreatedBy();
                    request.RealTimeSymptomsCongratulation.RealTimeLeadingQuestionId = realTimeLeadingQuestionId;
                    request.RealTimeSymptomsCongratulation.InsertDate = DateTime.Now;
                    request.RealTimeSymptomsCongratulation.UpdateDate = DateTime.Now;
                    request.RealTimeSymptomsCongratulation.LanguageId = _languageId;

                    context.RealTimeSymptomsCongratulations.Add(request.RealTimeSymptomsCongratulation);
                    context.SaveChanges();

                    #endregion RealTimeDetectionWayCongratulation


                    #region RealTimeLeadingQuestionContent

                    request.RealTimeLeadingQuestionContent.CreatedBy = new AdminHelper().GetCreatedBy(); ;
                    request.RealTimeLeadingQuestionContent.RealTimeLeadingQuestionId = realTimeLeadingQuestionId;
                    request.RealTimeLeadingQuestionContent.InsertDate = DateTime.Now;
                    request.RealTimeLeadingQuestionContent.UpdateDate = DateTime.Now;

                    context.RealTimeLeadingQuestionContents.Add(request.RealTimeLeadingQuestionContent);
                    context.SaveChanges();

                    #endregion RealTimeLeadingQuestionContent

                    #region RealTimeDetectionWayContent
                    //adding detection ways
                    for (int i = 0; i < request.RealTimeSymptomsContent.Count(); i++)
                    {
                        if (request.RealTimeSymptomsContent.ElementAt(i).SymptomsContent != "ignore")
                            AddNewRealTimeSymptoms(request.RealTimeSymptomsContent.ElementAt(i), _languageId, realTimeLeadingQuestionId);
                    }

                    #endregion RealTimeDetectionWayContent

                    #region RealTimeSolutionContent
                    //adding solutions
                    for (int i = 0; i < request.RealTimeSolutionContent.Count(); i++)
                    {
                        if (request.RealTimeSolutionContent.ElementAt(i).SolutionContent != "ignore")
                            AddNewRealTimeSolution(request.RealTimeSolutionContent.ElementAt(i), _languageId, realTimeLeadingQuestionId);
                    }

                    #endregion RealTimeSolutionContent
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

        private void AddNewRealTimeSolution(RealTimeSolutionContent item, int _languageId, int realTimeLeadingQuestionId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    RealTimeSolution _realTimeSolution = new RealTimeSolution();
                    _realTimeSolution.CreatedBy = new AdminHelper().GetCreatedBy(); ;
                    _realTimeSolution.RealTimeLeadingQuestionId = realTimeLeadingQuestionId;
                    _realTimeSolution.InsertDate = DateTime.Now;
                    _realTimeSolution.UpdateDate = DateTime.Now;

                    context.RealTimeSolutions.Add(_realTimeSolution);
                    context.SaveChanges();
                    int _realTimeSolutionId = _realTimeSolution.RealTimeSolutionId;


                    RealTimeSolutionContent _realTimeSolutionContent = item;
                    _realTimeSolutionContent.CreatedBy = new AdminHelper().GetCreatedBy(); ;
                    _realTimeSolutionContent.LanguageId = _languageId;
                    _realTimeSolutionContent.RealTimeSolutionId = _realTimeSolutionId;
                    _realTimeSolutionContent.InsertDate = DateTime.Now;
                    _realTimeSolutionContent.UpdateDate = DateTime.Now;

                    context.RealTimeSolutionContents.Add(_realTimeSolutionContent);
                    context.SaveChanges();
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
             
            }
        }

        private void AddNewRealTimeSymptoms(RealTimeSymptomsContent item, int _languageId, int realTimeLeadingQuestionId)
        {
            try
            {
                using (RombeEntities context = new RombeEntities())
                {
                    RealTimeSymptom _realTimeSymptom = new RealTimeSymptom();
                    _realTimeSymptom.CreatedBy = new AdminHelper().GetCreatedBy();
                    _realTimeSymptom.RealTimeLeadingQuestionId = realTimeLeadingQuestionId;
                    _realTimeSymptom.InsertDate = DateTime.Now;
                    _realTimeSymptom.UpdateDate = DateTime.Now;

                    context.RealTimeSymptoms.Add(_realTimeSymptom);
                    context.SaveChanges();
                    int _realTimeDetectionWaysId = _realTimeSymptom.RealTimeSymptomsId;


                    RealTimeSymptomsContent _realTimeSymptomsContent = item;
                    _realTimeSymptomsContent.CreatedBy = new AdminHelper().GetCreatedBy();
                    _realTimeSymptomsContent.LanguageId = _languageId;
                    _realTimeSymptomsContent.RealTimeSymptomsId = _realTimeDetectionWaysId;
                    _realTimeSymptomsContent.InsertDate = DateTime.Now;
                    _realTimeSymptomsContent.UpdateDate = DateTime.Now;

                    context.RealTimeSymptomsContents.Add(_realTimeSymptomsContent);
                    context.SaveChanges();
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, AdminLoggerMessageFormat.GetMessage(RouteData.Values["controller"].ToString(),
                                                          RouteData.Values["action"].ToString(), e.Message));
            }
        }

        //
        // GET: /RealTime/Edit/5
        public ActionResult Edit(int? id, int languageId)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                RealTimeLeadingQuestion realTimeLeadingQuestion = db.RealTimeLeadingQuestions.Find(id);
                RealTimeLeadingQuestionContent realTimeLeadingQuestionContent = db.RealTimeLeadingQuestionContents.Where(a => a.LanguageId == languageId && a.RealTimeLeadingQuestionId == id).FirstOrDefault();
                RealTimeSymptomsCongratulation realTimeDetectionWayCongratulation = db.RealTimeSymptomsCongratulations.Where(a => a.LanguageId == languageId && a.RealTimeLeadingQuestionId == id).FirstOrDefault();
                IEnumerable<RealTimeSolutionContent> realTimeSolutionContent = db.RealTimeSolutionContents.Where(a => a.RealTimeSolution.RealTimeLeadingQuestionId == id && a.LanguageId == languageId).ToList();
                IEnumerable<RealTimeSymptomsContent> realTimeDetectionWayContent = db.RealTimeSymptomsContents.Where(a => a.RealTimeSymptom.RealTimeLeadingQuestionId == id && a.LanguageId == languageId).ToList();

                if (realTimeLeadingQuestionContent == null || realTimeLeadingQuestion == null || realTimeSolutionContent == null || realTimeDetectionWayContent == null)
                {
                    return HttpNotFound();
                }
                RealTimeCreateModel model = new RealTimeCreateModel();

                model.RealTimeLeadingQuestion = realTimeLeadingQuestion;
                model.RealTimeLeadingQuestionContent = realTimeLeadingQuestionContent;

                model.RealTimeSolutionContent = realTimeSolutionContent;
                model.RealTimeSymptomsContent = realTimeDetectionWayContent;

                model.RealTimeSymptomsCongratulation = realTimeDetectionWayCongratulation;


                ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName", model.RealTimeLeadingQuestion.TaskCategoryId);
                ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName", model.RealTimeLeadingQuestionContent.LanguageId);
                ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName", model.RealTimeLeadingQuestion.PeriodTypeId);
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
        // POST: /RealTime/Edit/5
        [HttpPost]
        public ActionResult Edit(RealTimeCreateModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName",model.RealTimeLeadingQuestion.TaskCategoryId);
                    ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName",model.RealTimeLeadingQuestionContent.LanguageId);
                    ViewBag.PeriodTypeId = new SelectList(db.PeriodTypes, "PeriodTypeId", "PeriodTypeName",model.RealTimeLeadingQuestion.PeriodTypeId);
                    var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                    return View(model);
                }
                else
                {
                    var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();

                    
                    LoggerHelper.Info(errors);
                }

                int _languageId = model.RealTimeLeadingQuestionContent.LanguageId;
                int _realTimeLeadingQuestionId = model.RealTimeLeadingQuestion.RealTimeLeadingQuestionId;

                if (model.RealTimeSolutionContent != null)
                {
                    foreach (RealTimeSolutionContent item in model.RealTimeSolutionContent)
                    {
                        if (String.IsNullOrEmpty(item.CreatedBy) && item.RealTimeSolutionId == 0 && item.LanguageId == 0)
                        {
                            AddNewRealTimeSolution(item, _languageId, _realTimeLeadingQuestionId);
                            continue;
                        }

                        if (item.SolutionContent == "remove")
                        {
                            db.RealTimeSolutionContents.Attach(item);
                            db.RealTimeSolutionContents.Remove(item);
                        }
                        else
                        {
                            item.UpdateDate = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        db.SaveChanges();

                        RealTimeSolution update = db.RealTimeSolutions.Where(a => a.RealTimeSolutionId == item.RealTimeSolutionId).First();
                        update.UpdateDate = DateTime.Now;
                        db.SaveChanges();

                    }
                }
                if (model.RealTimeSymptomsContent != null)
                {
                    foreach (RealTimeSymptomsContent item in model.RealTimeSymptomsContent)
                    {
                        if (String.IsNullOrEmpty(item.CreatedBy) && item.RealTimeSymptomsId == 0 && item.LanguageId == 0)
                        {
                            AddNewRealTimeSymptoms(item, _languageId, _realTimeLeadingQuestionId);
                            continue;
                        }
                        if (item.SymptomsContent == "remove")
                        {
                            db.RealTimeSymptomsContents.Attach(item);
                            db.RealTimeSymptomsContents.Remove(item);
                        }
                        else
                        {
                            item.UpdateDate = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        db.SaveChanges();

                        RealTimeSymptom update = db.RealTimeSymptoms.Where(a => a.RealTimeSymptomsId == item.RealTimeSymptomsId).First();
                        update.UpdateDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }

                model.RealTimeSymptomsCongratulation.UpdateDate = DateTime.Now;
                model.RealTimeLeadingQuestion.UpdateDate = DateTime.Now;
                model.RealTimeLeadingQuestionContent.UpdateDate = DateTime.Now;

                db.Entry(model.RealTimeLeadingQuestion).State = EntityState.Modified;
                db.Entry(model.RealTimeLeadingQuestionContent).State = EntityState.Modified;
                db.Entry(model.RealTimeSymptomsCongratulation).State = EntityState.Modified;
                db.SaveChanges();

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
        // GET: /RealTime/Delete/5
        public ActionResult Delete(int? id, int languageId)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                RealTimeLeadingQuestion realTimeLeadingQuestion = db.RealTimeLeadingQuestions.Find(id);
                RealTimeLeadingQuestionContent realTimeLeadingQuestionContent = db.RealTimeLeadingQuestionContents.Where(a => a.LanguageId == languageId && a.RealTimeLeadingQuestionId == id).FirstOrDefault();

                IEnumerable<RealTimeSolution> realTimeSolution = db.RealTimeSolutions.Where(a => a.RealTimeLeadingQuestionId == id).ToList();
                IEnumerable<RealTimeSolutionContent> realTimeSolutionContent = db.RealTimeSolutionContents.Where(a => a.RealTimeSolution.RealTimeLeadingQuestionId == id && a.LanguageId == languageId).ToList();

                IEnumerable<RealTimeSymptom> realTimeDetectionWay = db.RealTimeSymptoms.Where(a => a.RealTimeLeadingQuestionId == id).ToList();
                IEnumerable<RealTimeSymptomsContent> realTimeDetectionWayContent = db.RealTimeSymptomsContents.Where(a => a.RealTimeSymptom.RealTimeLeadingQuestionId == id && a.LanguageId == languageId).ToList();



                if (realTimeLeadingQuestion == null || realTimeLeadingQuestionContent == null || realTimeSolution == null ||
                    realTimeSolutionContent == null || realTimeDetectionWay == null || realTimeDetectionWayContent == null)
                {
                    return HttpNotFound();
                }
                RealTimeDeleteModel model = new RealTimeDeleteModel();
                model.RealTimeLeadingQuestion = realTimeLeadingQuestion;
                model.RealTimeLeadingQuestionContent = realTimeLeadingQuestionContent;
                model.RealTimeSymptom = realTimeDetectionWay;
                model.RealTimeSymptomsContent = realTimeDetectionWayContent;
                model.RealTimeSolution = realTimeSolution;
                model.RealTimeSolutionContent = realTimeSolutionContent;

                ViewBag.TaskCategoryId = new SelectList(db.TaskCategories, "CategoryId", "CategoryName");
                ViewBag.LanguageId = new SelectList(db.Languages, "LanguageId", "LanguageName");
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
        // POST: /RealTime/Delete/5
        [HttpPost]
        public ActionResult Delete(RealTimeDeleteModel model)
        {
            try
            {

                    if (model.RealTimeSymptomsContent != null)
                    {
                        foreach (RealTimeSymptomsContent item in model.RealTimeSymptomsContent)
                        {
                            db.RealTimeSymptomsContents.Attach(item);
                            db.RealTimeSymptomsContents.Remove(item);
                            
                        }
                    }

                    db.SaveChanges();
                    
                    if (model.RealTimeSymptom != null)
                    {
                        foreach (RealTimeSymptom item in model.RealTimeSymptom)
                        {
                            db.RealTimeSymptoms.Attach(item);
                            db.RealTimeSymptoms.Remove(item);
                           
                        }
                    }

                    db.SaveChanges();
                    if (model.RealTimeSolutionContent != null)
                    {
                        foreach (RealTimeSolutionContent item in model.RealTimeSolutionContent)
                        {
                            db.RealTimeSolutionContents.Attach(item);
                            db.RealTimeSolutionContents.Remove(item);
                           
                        }
                    }

                    db.SaveChanges();
                    if (model.RealTimeSolution != null)
                    {
                        foreach (RealTimeSolution item in model.RealTimeSolution)
                        {
                            db.RealTimeSolutions.Attach(item);
                            db.RealTimeSolutions.Remove(item);
                            
                        }
                    }
                    db.SaveChanges();

                    RealTimeSymptomsCongratulation realTimeDetectionWayCongratulation = db.RealTimeSymptomsCongratulations.Where(r => r.LanguageId == model.RealTimeLeadingQuestionContent.LanguageId &&
r.RealTimeLeadingQuestionId == model.RealTimeLeadingQuestion.RealTimeLeadingQuestionId).FirstOrDefault();

                    if (realTimeDetectionWayCongratulation != null)
                    {

                        db.RealTimeSymptomsCongratulations.Remove(realTimeDetectionWayCongratulation);
                       
                    }

                    RealTimeLeadingQuestionContent realTimeLeadingQuestionContent = db.RealTimeLeadingQuestionContents.Where(
    a => a.LanguageId == model.RealTimeLeadingQuestionContent.LanguageId && a.RealTimeLeadingQuestionId == model.RealTimeLeadingQuestionContent.RealTimeLeadingQuestionId).FirstOrDefault();

                    if (realTimeLeadingQuestionContent != null)
                    {
                        
                        db.RealTimeLeadingQuestionContents.Remove(realTimeLeadingQuestionContent);
                        
                    }

                    RealTimeLeadingQuestion realTimeLeadingQuestion = db.RealTimeLeadingQuestions.Find(model.RealTimeLeadingQuestion.RealTimeLeadingQuestionId);

                    if (realTimeLeadingQuestion != null)
                    {
                        
                        db.RealTimeLeadingQuestions.Remove(realTimeLeadingQuestion);
                        
                    }

                    db.SaveChanges();


                    
                
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
