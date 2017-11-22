using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Request
{
    public class GetTimelineContentPaginationRequest : BaseTimelineDateRequest
    {
        public int PaTaskIdToSkip { get; set; }
        public int RtTaskIdToSkip { get; set; }
    }
}
