using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RomBe.Entities;
using System.Data.Entity;
using System.Web.Mvc;

namespace RomBe.Services.Models
{
    
    public class RealTimeCreateModel
    {
        public RealTimeLeadingQuestion RealTimeLeadingQuestion { get; set; }
        public RealTimeLeadingQuestionContent RealTimeLeadingQuestionContent { get; set; }
        public RealTimeSymptomsCongratulation RealTimeSymptomsCongratulation { get; set; }
        public IEnumerable<RealTimeSymptomsContent> RealTimeSymptomsContent { get; set; }
        public IEnumerable<RealTimeSolutionContent> RealTimeSolutionContent { get; set; }

    }
}