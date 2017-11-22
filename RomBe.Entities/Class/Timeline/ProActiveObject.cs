using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class ProActiveObject : BaseTimeline
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Action { get; set; }
        
    }
}
