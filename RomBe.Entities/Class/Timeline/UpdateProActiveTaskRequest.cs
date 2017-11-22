using RomBe.Entities.Class.Request;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class UpdateProActiveTaskRequest : BaseUpdateTaskRequest
    {
        public TaskTypeEnum TaskType { get; set; }
    }
}
