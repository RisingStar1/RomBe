using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class BaseUpdateTaskRequest
    {
        public int ChildId { get; set; }
        public int TaskId { get; set; }
        public TaskStatusEnum TaskStatus { get; set; }
    }
}
