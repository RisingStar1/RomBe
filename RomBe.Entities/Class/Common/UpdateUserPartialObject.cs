using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Common
{
    public class UpdateUserPartialObject
    {
        public string Password { get; set; }
        public string FacebookUserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        
    }
    
}
