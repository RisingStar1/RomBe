using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Helpers;
using RomBe.Entities;
using System.Web;
using System.Net;
using System.Net.Http;
using RestSharp;
using Newtonsoft.Json;
using RomBe.Entities.Class.Response;

namespace RomBe.Logic.Authentication.Logic
{
    public class AuthenticationLogic
    {
        #region public methods
        public HttpResponseMessage ValidateClientSecretAndClientId(out Boolean isValid)
        {
            try
            {
                string clientId = string.Empty;
                string clientSecret = string.Empty;
                Client client = null;
                HttpResponseMessage response = new HttpResponseMessage();

                if (new AuthenticationHelper().TryGetBasicCredentials(out clientId, out clientSecret))
                {
                    client = new RomBe.Logic.ClientLogic.ClientLogic().FindClient(clientId);

                    if (client.IsNull())
                    {
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Headers.Add("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", clientId));
                        isValid = false;
                        return response;
                    }


                    if (clientSecret.IsNull())
                    {
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Headers.Add("invalid_clientSecret", "Client secret should be sent.");
                        isValid = false;
                        return response;
                    }

                    if (client.Secret != new HashHelper().GetHash(clientSecret))
                    {
                        response.StatusCode = HttpStatusCode.Unauthorized;
                        response.Headers.Add("invalid_clientSecret", "Client secret is invalid.");
                        isValid = false;
                        return response;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.Headers.Add("invalid_header", "Client id and client secret needed.");
                    isValid = false;
                    return response;
                }
                isValid = true;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public CreateUserTokenResponse GetTokenForFacebookUser(string facebookUserId)
        {
            try
            {
                String baseUrl = String.Format("http://{0}:{1}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);

                RestClient client = new RestClient(baseUrl);
                client.FollowRedirects = false;

                RestRequest request = new RestRequest("/token");
                request.Method = Method.POST;
                request.AddHeader("ContentType", "application/x-www-form-urlencoded");
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", "androidApp");
                request.AddParameter("client_secret", "59FDEEE6486CA796A13AE5437D2B2");
                request.AddParameter("facebookUserId", facebookUserId);

                var response = client.Execute(request);

                CreateUserTokenResponse reuslt = JsonConvert.DeserializeObject<CreateUserTokenResponse>(response.Content);
                return reuslt;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public CreateUserTokenResponse GetTokenForGoogleUser(string googleUserId)
        {
            try
            {
                String baseUrl = String.Format("http://{0}:{1}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);

                RestClient client = new RestClient(baseUrl);
                client.FollowRedirects = false;

                RestRequest request = new RestRequest("/token");
                request.Method = Method.POST;
                request.AddHeader("ContentType", "application/x-www-form-urlencoded");
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", "androidApp");
                request.AddParameter("client_secret", "59FDEEE6486CA796A13AE5437D2B2");
                request.AddParameter("googleUserId", googleUserId);

                var response = client.Execute(request);

                CreateUserTokenResponse reuslt = JsonConvert.DeserializeObject<CreateUserTokenResponse>(response.Content);
                return reuslt;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public CreateUserTokenResponse GetTokenForUser(string email)
        {
            try
            {
                String baseUrl = String.Format("http://{0}:{1}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);

                RestClient client = new RestClient(baseUrl);
                client.FollowRedirects = false;

                RestRequest request = new RestRequest("/token");
                request.Method = Method.POST;
                request.AddHeader("ContentType", "application/x-www-form-urlencoded");
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", "androidApp");
                request.AddParameter("client_secret", "59FDEEE6486CA796A13AE5437D2B2");
                request.AddParameter("username", email);

                var response = client.Execute(request);

                CreateUserTokenResponse reuslt = JsonConvert.DeserializeObject<CreateUserTokenResponse>(response.Content);
                return reuslt;
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion public methods
    }

    
}
