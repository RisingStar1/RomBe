using RomBe.Entities.Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Respone
{
    public class UpdateTaskResponse:BaseResponse
    {
        public object UpdatedTask { get; set; }
    }
}
