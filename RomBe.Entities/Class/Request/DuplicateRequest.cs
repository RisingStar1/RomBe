using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Request
{
    public class DuplicateRequest
    {
        public int TaskId { get; set; }
        public int TaskType { get; set; }
        public string PaginationType { get; set; }
    }
}
