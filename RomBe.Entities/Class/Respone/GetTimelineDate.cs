using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Timeline;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace RomBe.Entities.Class.Respone
{
    public class GetTimelineDateResponse:BaseResponse
    {
        public int ChildId { get; set; }
        public PagingObejct NextPage { get; set; }
        public PagingObejct PreviousPage { get; set; }
        public List<WeekObject> TimelineContent { get; set; }

        public GetTimelineDateResponse()
        {
            TimelineContent = new List<WeekObject>();
        }
        
    }

    public class WeekObject
    {
        public int WeekNumber { get; set; }
        public List<object> WeekItems { get; set; }
        public WeekObject()
        {
            WeekItems = new List<object>();
        }
    }
    
}
