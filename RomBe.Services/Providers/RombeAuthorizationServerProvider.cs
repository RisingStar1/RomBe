using RomBe.Entities;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using RomBe.Logic.ClientLogic;
using RomBe.Entities.Enums;
using RomBe.Helpers;
using RomBe.Logic.UserLogic;
using System.Net;

namespace RomBe.Services.Providers
{
    public class RombeAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                string clientId = string.Empty;
                string clientSecret = string.Empty;
                Client client = null;

                if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
                {
                    context.TryGetFormCredentials(out clientId, out clientSecret);
                }

                if (context.ClientId == null)
                {
                    //Remove the comments from the below line context.SetError, and invalidate context 
                    //if you want to force sending clientId/secrects once obtain access tokens. 
                    context.Validated();
                    //context.SetError("invalid_clientId", "ClientId should be sent.");
                    return Task.FromResult<object>(null);
                }


                client = new ClientLogic().FindClient(context.ClientId);


                if (client == null)
                {
                    context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                    return Task.FromResult<object>(null);
                }

                if (client.ApplicationType == (int)ApplicationTypesEnum.NativeConfidential)
                {
                    if (string.IsNullOrWhiteSpace(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret should be sent.");
                        return Task.FromResult<object>(null);
                    }
                    else
                    {
                        if (client.Secret != new HashHelper().GetHash(clientSecret))
                        {
                            context.SetError("invalid_clientId", "Client secret is invalid.");
                            return Task.FromResult<object>(null);
                        }
                    }
                }

                if (!Convert.ToBoolean(client.Active))
                {
                    context.SetError("invalid_clientId", "Client is inactive.");
                    return Task.FromResult<object>(null);
                }

                context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
                context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

                context.Validated();
                return Task.FromResult<object>(null);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, context);
                return Task.FromResult<object>(null);
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                UserLogic userLogic = new UserLogic();
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                if (allowedOrigin == null) allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });



                //in case of login via facebook
                String facebookUserId = context.Request.ReadFormAsync().Result["facebookUserId"];

                //in case of login via facebook
                String googleUserId = context.Request.ReadFormAsync().Result["googleUserId"];

                User user = null;


                //in case user login afert delete of the app using facebook

                if (!facebookUserId.IsNull())
                {
                    user = await userLogic.GetUserByFacebookId(facebookUserId);

                }

                else if (!googleUserId.IsNull())
                {
                    user = await userLogic.GetUserByGoogleId(googleUserId);

                }

                //in case user login after app delete via rombe
                else if (!context.UserName.IsNull())
                {
                    user = userLogic.GetUserByEmail(context.UserName);
                    if (user.IsNull())
                    {
                        LoggerHelper.Info(string.Format("Email: {0}", context.UserName));
                        context.SetError("The email is incorrect.");
                        return;
                    }
                }


                ////in case user login after app delete via rombe
                //else if (!context.UserName.IsNull() && !context.Password.IsNull())
                //{
                //    user = await userLogic.GetUser(context.UserName, context.Password);
                //    if(user.IsNull())
                //    {
                //        LoggerHelper.Info(string.Format("Email: {0} Password: {1}", context.UserName, context.Password));
                //        context.SetError("The email or password is incorrect.");
                //        return;
                //    }
                //    else if(!user.IsActive)
                //    {
                //        LoggerHelper.Info(string.Format("Email: {0} Password: {1}", context.UserName, context.Password));
                //        context.SetError("Your account is waiting to be activated, Please go to your email and activate it");
                //        return;
                //    }
                //}
                if (user == null)
                {
                    context.SetError("The user name or password is incorrect.");
                    LoggerHelper.Info(facebookUserId);
                    return;
                }


                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                if (!String.IsNullOrEmpty(context.UserName))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    identity.AddClaim(new Claim("sub", context.UserName));
                }
                else
                {
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                    identity.AddClaim(new Claim("sub", user.Email));
                }
                identity.AddClaim(new Claim("role", "user"));
                identity.AddClaim(new Claim("userId", user.UserId.ToString()));

                var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    }
                });


                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, context);
            }
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            try
            {
                var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
                var currentClient = context.ClientId;

                if (originalClient != currentClient)
                {
                    context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                    return Task.FromResult<object>(null);
                }

                // Change auth ticket for refresh token requests
                var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
                //newIdentity.AddClaim(new Claim("newClaim", "newValue"));

                var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
                context.Validated(newTicket);

                return Task.FromResult<object>(null);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, context);
                return Task.FromResult<object>(null);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            try
            {
                foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }

                return Task.FromResult<object>(null);
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, context);
                return Task.FromResult<object>(null);
            }
        }

    }
}