using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Timeline
{
    public class ProActiveDataFromDBObject
    {
        public ProActiveInformation ProActiveInformation { get; set; }
        public ProActiveInformationContent ProActiveInformationContent { get;set;}
        public TaskStatusEnum? TaskStatus { get; set; }
    }
}
