using RomBe.Logic.RefreshTokenLogic;
using RomBe.Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RomBe.Services.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class RefreshTokensController : ApiController
    {

      
       // [Authorize(Users="Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(new RefreshTokenLogic().GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await new RefreshTokenLogic().RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");
            
        }

    }
}
