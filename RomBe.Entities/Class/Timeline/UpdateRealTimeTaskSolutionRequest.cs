using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class UpdateRealTimeTaskSolutionRequest : BaseUpdateTaskRequest
    {
        public List<int> SolutionIds { get; set; }
    }
}
