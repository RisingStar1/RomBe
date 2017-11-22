using Newtonsoft.Json;
using RomBe.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RomBe.Entities.Class.Common
{
    public class ChildObject
    {
        public String FirstName { get; set; }
        
        public DateTime BirthDate { get; set; }
        public GenderTypeEnum Gender { get; set; }
        public int? ChildId { get; set; }
    }
}
