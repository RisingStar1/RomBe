using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Respone
{
    public class IsConfigUpdatedResponse
    {
        public bool NeedToUpdate { get; set; }
        public int? LastUpdateCheck { get; set; }
    }
}
