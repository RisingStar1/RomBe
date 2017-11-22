using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class RealTimeDataFromDBObject
    {
        public RealTimeLeadingQuestion RealTimeLeadingQuestion { get; set; }
        public RealTimeLeadingQuestionContent RealTimeLeadingQuestionContent { get; set; }
        public List<RealTimeSymptomsContent> RealTimeSymptomsContentList { get; set; }
        public List<RealTimeSolutionContent> RealTimeSolutionContentList { get; set; }
        public RealTimeSymptomsCongratulation RealTimeSymptomsCongratulations { get; set; }
        public TaskStatusEnum? TaskStatus { get; set; }
        public string TaskCategory { get; set; }
        public RealTimeDataFromDBObject()
        {
            RealTimeSolutionContentList = new List<RealTimeSolutionContent>();
            RealTimeSymptomsContentList = new List<RealTimeSymptomsContent>();
        }
    }
}
