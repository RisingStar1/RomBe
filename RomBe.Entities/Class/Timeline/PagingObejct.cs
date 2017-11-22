using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class PagingObejct
    {
        public int ChildId { get; set; }
        public int ProActiveTaskIdToSkip { get; set; }
        public int RealTimeTaskIdToSkip { get; set; }
        public int MinWeek { get; set; }
        public int MaxWeek { get; set; }
    }
}
