using RomBe.Entities;
using RomBe.Entities.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomBe.Logic.RefreshTokenLogic
{
    public class RefreshTokenLogic
    {
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            return await new RefreshToeknDAL().AddRefreshToken(token);
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            return await new RefreshToeknDAL().RemoveRefreshToken(refreshTokenId);
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            return await new RefreshToeknDAL().RemoveRefreshToken(refreshToken);
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            return await new RefreshToeknDAL().FindRefreshToken(refreshTokenId);
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return new RefreshToeknDAL().GetAllRefreshTokens();
        }

        public async Task<RefreshToken> UpdateToken(string email, DateTime expiresUtc,string protectedTicket)
        {
            return await new RefreshToeknDAL().UpdateToken(email, expiresUtc, protectedTicket);
        }

    }
}
