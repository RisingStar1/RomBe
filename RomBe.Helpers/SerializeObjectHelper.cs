using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Helpers
{
    public class SerializeObjectHelper
    {
        public String ObjectToJson(object toSerialize)
        {
            return JsonConvert.SerializeObject(toSerialize);
        }
    }
}
