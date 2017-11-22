using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomBe.Helpers;
using System.Web.Hosting;

namespace RomBe.Entities.DAL
{
    public class RefreshToeknDAL
    {
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            using (RombeEntities context = new RombeEntities())
            {
                var existingToken = context.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).ToList();

                if (!existingToken.IsNull())
                {
                    HostingEnvironment.QueueBackgroundWorkItem(task =>
                    {
#pragma warning disable 4014
                        foreach (var item in existingToken)
                        {
                            RemoveRefreshToken(item);
                        }
                       
                    });
                    
                }

                context.RefreshTokens.Add(token);

                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                var refreshToken = await context.RefreshTokens.FindAsync(refreshTokenId);

                if (!refreshToken.IsNull())
                {
                    context.RefreshTokens.Remove(refreshToken);
                    return await context.SaveChangesAsync() > 0;
                }

                return false;
            }
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshTokens)
        {
            using (RombeEntities context = new RombeEntities())
            {
                
                    context.RefreshTokens.Attach(refreshTokens);
                    context.RefreshTokens.Remove(refreshTokens);
                
                
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            using (RombeEntities context = new RombeEntities())
            {
                return await context.RefreshTokens.FindAsync(refreshTokenId);
            }
            
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            using (RombeEntities context = new RombeEntities())
            {
                return context.RefreshTokens.ToList();
            }
        }

        public async Task<RefreshToken> UpdateToken(string email,DateTime expiresUtc, string protectedTicket)
        {
            using (RombeEntities context = new RombeEntities())
            {
                RefreshToken _existToken= context.RefreshTokens.Where(r => r.Subject == email).FirstOrDefault();
                if(!_existToken.IsNull())
                {
                    _existToken.ProtectedTicket = protectedTicket;
                    _existToken.ExpiresUtc = expiresUtc;
                    await context.SaveChangesAsync();
                    return _existToken;
                }
                return null;
            }
        }
    }
}
