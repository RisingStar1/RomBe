using Newtonsoft.Json;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class BaseTimeline
    {
        public TaskTypeEnum Type { get; set; }
        public bool IsDone { get; set; }
        public int Id { get; set; }
        public int UniqueId { get; set; }
        public TaskStatusEnum? TaskStatus { get; set; }
        public int PeriodMin { get; set; }
        public int PeriodMax { get; set; }
    }
    
}
