using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Common
{
    public class AndroidJsonObject
    {
        public String Title { get; set; }
        public String Message { get; set; }
        //public String Badge { get; set; }
        public String Sound { get; set; }
        public int NotificationId { get; set; }
        public TaskTypeEnum TaskType { get; set; }
        public int TaskId { get; set; }
        public int ChildId { get; set; }
        public bool IsActionable { get; set; }
        public AndroidJsonObject()
        {
            Sound = "sound.caf";
        }
    }
}
