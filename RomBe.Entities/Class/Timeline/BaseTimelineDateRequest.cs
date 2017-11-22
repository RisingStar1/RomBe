using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Request
{
    public class BaseTimelineDateRequest
    {
        public int ChildId { get; set; }
        public int? MinWeeks { get; set; }
        public int? MaxWeeks { get; set; }
    }
}
