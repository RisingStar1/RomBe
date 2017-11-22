using RomBe.Entities;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;
using RomBe.Logic.RefreshTokenLogic;
using RomBe.Helpers;

namespace RomBe.Services.Providers
{
    public class RombeRefreshTokenProvider : IAuthenticationTokenProvider
    {

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            try
            {
                var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

                if (string.IsNullOrEmpty(clientid))
                {
                    return;
                }

                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                RefreshToken isUpdated = await UpdateExistingRefreshToekn(context, refreshTokenLifeTime);
                if (!isUpdated.IsNull())
                {
                    context.SetToken(isUpdated.Id);
                }
                else
                {

                    var refreshTokenId = Guid.NewGuid().ToString("n");

                    var token = new RefreshToken()
                    {
                        //Id = new HashHelper().GetHash(refreshTokenId),
                        Id = refreshTokenId,
                        ClientId = clientid,
                        Subject = context.Ticket.Identity.Name,
                        IssuedUtc = DateTime.UtcNow,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                    };

                    context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                    token.ProtectedTicket = context.SerializeTicket();
                    var result = await new RefreshTokenLogic().AddRefreshToken(token);

                    if (result)
                    {
                        context.SetToken(refreshTokenId);
                    }
                }

            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, context);
            }

        }
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            try
            {
                RefreshTokenLogic refreshTokenLogic = new RefreshTokenLogic();
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                //string hashedTokenId = new HashHelper().GetHash(context.Token);

                var refreshToken = await refreshTokenLogic.FindRefreshToken(context.Token);
                //var refreshToken = await refreshTokenLogic.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    //var result = await refreshTokenLogic.RemoveRefreshToken(hashedTokenId);

                }
            }
            catch (Exception e)
            {
                LoggerHelper.Error(e, context);
            }

        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        private async Task<RefreshToken> UpdateExistingRefreshToekn(AuthenticationTokenCreateContext context, string refreshTokenLifeTime)
        {
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime));
            context.Ticket.Properties.ExpiresUtc = expiresAt;
            string protectedTicket = context.SerializeTicket();
            RefreshToken isUpdated = await new RefreshTokenLogic().UpdateToken(context.Ticket.Identity.Name, expiresAt, protectedTicket);
            return isUpdated;
        }
    }
}