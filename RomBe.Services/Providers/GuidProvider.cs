using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Threading.Tasks;

namespace RomBe.Services.Providers
{
    public class GuidProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> tokens
            = new ConcurrentDictionary<string, AuthenticationTicket>();

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }


        //public async System.Threading.Tasks.Task CreateAsync(AuthenticationTokenCreateContext context)
        //{

        //    var guid = Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n");

        //    var ticket = Crypto.Hash(guid);

        //    tokens.TryAdd(ticket, context.Ticket);

        //    context.SetToken(ticket);
        //}

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async System.Threading.Tasks.Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;

            if (tokens.TryGetValue(context.Token, out ticket))
            {
                if (ticket.Properties.ExpiresUtc.Value < DateTime.UtcNow)
                {
                    tokens.TryRemove(context.Token, out ticket);
                }
                context.SetTicket(ticket);
            }
        }
    }
}