using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Repository.Entities;
using Shopping.Authentication.SeedWorks.Helpers;

namespace Shopping.Authentication.Providers
{
    public class AuthorizationRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly IAppRepository _repository;
        public AuthorizationRefreshTokenProvider()
        {
            _repository = Bootstrapper.WindsorContainer.Resolve<IAppRepository>();
        }
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientId = context.Ticket.Properties.Dictionary["as:client_id"];
            string deviceId = context.OwinContext.Get<string>("as:device_id");
            if (string.IsNullOrEmpty(clientId)|| string.IsNullOrEmpty(deviceId))
            {
                return;
            }
            var refreshTokenId = Guid.NewGuid().ToString("n");
            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new RefreshToken
            {
                Id = HashHelper.GetHash(refreshTokenId),
                ClientId = clientId,
                Subject = context.Ticket.Identity.Name,
                DeviceId = deviceId,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)),
            };
            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
            token.ProtectedTicket = context.SerializeTicket();
            var result = await _repository.AddRefreshToken(token);
            if (result)
            {
                context.SetToken(refreshTokenId);
            }
        }
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            string hashedTokenId = HashHelper.GetHash(context.Token);
            var refreshToken = await _repository.FindRefreshToken(hashedTokenId);
            if (refreshToken == null)
            {
                return;
            }
            //Get protectedTicket from refreshToken class
            context.DeserializeTicket(refreshToken.ProtectedTicket);
            await _repository.RemoveRefreshToken(hashedTokenId);
        }
        public void Create(AuthenticationTokenCreateContext context)
        {
        }
        public void Receive(AuthenticationTokenReceiveContext context)
        {
        }
    }
}