using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Entities.Class.Respone
{
    public class LoginResponse:BaseResponse
    {
        public CreateUserTokenResponse UserToken { get; set; }
        public GetUserDetailsResponse UserDetails { get; set; }
    }
}
