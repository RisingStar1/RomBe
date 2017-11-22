using RomBe.AdminSite.Models;
using RomBe.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RomBe.AdminSite.Controllers.Management
{
    public class CalculationController : Controller
    {
        // GET: Calculation
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index(FormCollection frm)
        {



            RombeEntities context = new RombeEntities();
            List<CalculationModel> results = new List<CalculationModel>();



            if (frm["QueryDateButton"] != null)
            {
                DateTime startDate = DateTime.Parse(frm["startDate"], new CultureInfo("he-IL"));
                DateTime endDate = DateTime.Parse(frm["endDate"], new CultureInfo("he-IL"));
                CalculationModel newDate = new CalculationModel();
                newDate.Date = string.Format("{0} - {1}", startDate.ToShortDateString(), endDate.ToShortDateString());
                newDate.Qa = CalculateQandA(context, startDate, endDate);
                newDate.RealTime=CalculateRealTime(context, startDate, endDate);
                newDate.Tips=CalculateTips(context, startDate, endDate);

                newDate.QaPayment = CalculateQandACost(newDate.Qa);
                newDate.RealTimePayment = CalculateRealTimeCost(newDate.RealTime);
                newDate.TipsPayment = CalculateTipsCost(newDate.Tips);
                newDate.TotalPayment = newDate.QaPayment + newDate.RealTimePayment + newDate.TipsPayment;

                results.Add(newDate);
                ViewBag.SpecificDateResult = results;
            }

            else
            {
                for (int month = 1; month <= 12; month++)
                {
                    GregorianCalendar calendar = new GregorianCalendar();
                    

                    DateTime startDate = new DateTime(2015, month, 1);
                    DateTime endDate = new DateTime(2015, month, calendar.GetDaysInMonth(2015,month));
                    CalculationModel newDate = new CalculationModel();
                    newDate.Date=string.Format("{0}-{1}", startDate.ToString("MMM", CultureInfo.InvariantCulture),startDate.Year);
                    newDate.Qa = CalculateQandA(context, startDate, endDate);
                    newDate.RealTime = CalculateRealTime(context, startDate, endDate);
                    newDate.Tips = CalculateTips(context, startDate, endDate);

                    newDate.QaPayment = CalculateQandACost(newDate.Qa);
                    newDate.RealTimePayment = CalculateRealTimeCost(newDate.RealTime);
                    newDate.TipsPayment = CalculateTipsCost(newDate.Tips);
                    newDate.TotalPayment = newDate.QaPayment + newDate.RealTimePayment + newDate.TipsPayment;

                    results.Add(newDate);
                    
                }
                ViewBag.AllDatesResult = results;
            }

            
            
            return View();
        }

        private int CalculateRealTime(RombeEntities context, DateTime startDate, DateTime endDate)
        {
            return (from p in context.RealTimeLeadingQuestions
                    where p.InsertDate >= startDate && p.InsertDate <= endDate
                    select p).Count();
        }

        private int CalculateQandA(RombeEntities context, DateTime startDate, DateTime endDate)
        {
            return (from p in context.ProActiveInformations
                    where p.ProActiveTypeId == 2 &&
                          p.InsertDate >= startDate && p.InsertDate <= endDate
                    select p).Count();
        }

        private int CalculateTips(RombeEntities context, DateTime startDate, DateTime endDate)
        {
            return (from p in context.ProActiveInformations
                    where p.ProActiveTypeId == 1 &&
                          p.InsertDate >= startDate && p.InsertDate <= endDate
                    select p).Count();
        }

        private int CalculateRealTimeCost(int numberOfRealTime)
        {
            return 20 * numberOfRealTime;
        }

        private int CalculateQandACost(int numberOfQandA)
        {
            return 10 * numberOfQandA;
        }

        private double CalculateTipsCost(int numberOfTips)
        {

            return ((double)numberOfTips / (double)25) * 70;
        }


    }
}