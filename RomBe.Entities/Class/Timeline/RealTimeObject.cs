using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class RealTimeObject : BaseTimeline
    {
        public string Question { get; set; }
        public string Subject { get; set; }
        public string PositiveAnswer { get; set; }
        public string NegativeAnswer { get; set; }
        public string TaskCategory { get; set; }
        public string CompletionMessage { get; set; }
        public string DismissionMessage { get; set; }
        public List<RealTimeSymptomObject> Symptoms { get; set; }
        public List<RealTimeSolutionObject> Solutions { get; set; }
        public RealTimeObject()
        {
           
            Solutions = new List<RealTimeSolutionObject>();
            Symptoms = new List<RealTimeSymptomObject>();
        }
    }
   
    public class RealTimeSymptomObject : BaseRealTimeData { }
    public class RealTimeSolutionObject : BaseRealTimeData { }
    public class BaseRealTimeData
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }
    }

}
