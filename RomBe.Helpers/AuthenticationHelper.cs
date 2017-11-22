using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;

namespace RomBe.Helpers
{
    public class AuthenticationHelper
    {
        public Boolean TryGetBasicCredentials(out string clientId, out string clientSecret)
        {
            // Client Authentication http://tools.ietf.org/html/rfc6749#section-2.3
            // Client Authentication Password http://tools.ietf.org/html/rfc6749#section-2.3.1
            string authorization = HttpContext.Current.Request.Headers.Get("Authorization");
            if (!string.IsNullOrWhiteSpace(authorization) && authorization.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    byte[] data = Convert.FromBase64String(authorization.Substring("Basic ".Length).Trim());
                    string text = Encoding.UTF8.GetString(data);
                    int delimiterIndex = text.IndexOf(':');
                    if (delimiterIndex >= 0)
                    {
                        clientId = text.Substring(0, delimiterIndex);
                        clientSecret = text.Substring(delimiterIndex + 1);
                        return true;
                    }
                }
                catch (FormatException)
                {
                    // Bad Base64 string
                }
                catch (ArgumentException)
                {
                    // Bad utf-8 string
                }
            }

            clientId = null;
            clientSecret = null;
            return false;
        }

        public int? GetCurrentUserId()
        {
            Claim userIdClaim = ((ClaimsIdentity)(HttpContext.Current.User.Identity)).FindFirst("userId");

            if (userIdClaim != null)
            {
                int userId;
                Boolean result = int.TryParse(userIdClaim.Value.ToString(), out userId);
                if (result)
                    return userId;
                else return null;
            }
            else return null; 
        }
    }
}
