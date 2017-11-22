using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Request
{
    public class SubscribeRequest
    {
        [EmailAddress]
        public String Email { get; set; }
    }
}
