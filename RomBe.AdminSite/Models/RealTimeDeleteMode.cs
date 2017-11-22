using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RomBe.Entities;
using System.Data.Entity;

namespace RomBe.Services.Models
{
    public class RealTimeDeleteModel
    {
        public RealTimeLeadingQuestion RealTimeLeadingQuestion { get; set; }
        public RealTimeLeadingQuestionContent RealTimeLeadingQuestionContent { get; set; }
        public RealTimeSymptomsCongratulation RealTimeSymptomsCongratulation { get; set; }
        public IEnumerable<RealTimeSymptom> RealTimeSymptom { get; set; }
        public IEnumerable<RealTimeSymptomsContent> RealTimeSymptomsContent { get; set; }
        public IEnumerable<RealTimeSolution> RealTimeSolution { get; set; }
        public IEnumerable<RealTimeSolutionContent> RealTimeSolutionContent { get; set; }

    }
}