using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RomBe.AdminSite.Models
{
    public class CalculationModel
    {
        public string Date { get; set; }
        public int RealTime { get; set; }
        public int Tips { get; set; }
        public int Qa { get; set; }
        public int RealTimePayment { get; set; }
        public double TipsPayment { get; set; }
        public int QaPayment { get; set; }
        public double TotalPayment { get; set; }
    }
}