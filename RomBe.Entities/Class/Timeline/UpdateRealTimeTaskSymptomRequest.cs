using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class UpdateRealTimeTaskSymptomRequest : BaseUpdateTaskRequest
    {
        public List<int> SymptomsIds { get; set; }
    }
}
