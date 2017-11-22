using RomBe.Entities.Class.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Response
{
    public class CreateUserTokenResponse:BaseResponse
    {
        public String access_token { get; set; }
        public String token_type { get; set; }
        public int expires_in { get; set; }
        public String refresh_token { get; set; }
       
    }
}
