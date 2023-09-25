using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Shopping.Authentication.Interfaces;
using Shopping.Authentication.Models;
using Shopping.Authentication.Repository.DbContexts;
using Shopping.Authentication.SeedWorks.Enums;
using Shopping.Authentication.SeedWorks.Exceptions;
using Shopping.Authentication.SeedWorks.Helpers;

namespace Shopping.Authentication.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAppRepository _repository;
        private IPanelRepository _panelRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthorizationServerProvider()
        {
            _repository = Bootstrapper.WindsorContainer.Resolve<IAppRepository>();
            var context = new AuthContext();
            var userStore = new UserStore<ApplicationUser>(context);
            _userManager = new UserManager<ApplicationUser>(userStore);
        }
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
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
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
                context.RequestCompleted();
                return Task.FromResult(0);
            }
            return base.MatchEndpoint(context);
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (!context.TryGetBasicCredentials(out _, out var clientSecret))
            {
                context.TryGetFormCredentials(out _, out clientSecret);
            }
            if (context.ClientId == null)
            {
                context.Validated();
                return Task.FromResult<object>(null);
            }
            var client = _repository.FindClient(context.ClientId);
            if (client == null)
            {
                context.SetError("invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");
                return Task.FromResult<object>(null);
            }
            if (client.ApplicationType == ApplicationType.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                if (client.Secret != HashHelper.GetHash(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");
                    return Task.FromResult<object>(null);
                }
            }
            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }
            string deviceId = context.Parameters.Where(f => f.Key == "device_id").Select(f => f.Value).SingleOrDefault()?[0];
            if (string.IsNullOrEmpty(deviceId))
            {
                context.SetError("invalid_deviceId", "device is empty.");
                return Task.FromResult<object>(null);
            }
            context.OwinContext.Set("as:device_id", deviceId);
            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
            context.Validated();
            return Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                _panelRepository = Bootstrapper.WindsorContainer.Resolve<IPanelRepository>();
                string deviceId = context.OwinContext.Get<string>("as:device_id");
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
                if (allowedOrigin == null) allowedOrigin = "*";
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                var client = _repository.FindClient(context.ClientId);
                if (client == null)
                {
                    context.SetError("invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");
                    return;
                }
                if (client.ApplicationType == ApplicationType.JavaScript)
                {
                    var user = await _panelRepository.FindUser(context.UserName, context.Password);
                    if (user == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                    var roles = await _panelRepository.GetUserRoles(user);
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    identity.AddClaim(new Claim("FirstName", user.FirstName));
                    identity.AddClaim(new Claim("LastName", user.LastName));
                    identity.AddClaim(new Claim("UserId", user.Id));
                    foreach (var role in roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                    var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {
                            "as:client_id", context.ClientId ?? string.Empty
                        },
                        {
                            "userId", user.Id
                        },
                        {
                            "firstName", user.FirstName
                        },
                        {
                            "lastName", user.LastName
                        }
                    });
                    var ticket = new AuthenticationTicket(identity, props);
                    context.Validated(ticket);
                }
                else if (client.ApplicationType == ApplicationType.CustomerUserApp || client.ApplicationType == ApplicationType.ShopUserApp)
                {
                    var appUser = _userManager.Users.SingleOrDefault(item => item.PhoneNumber == context.UserName);
                    var rolesApp = await _repository.GetUserRoles(appUser);
                    if (appUser == null)
                    {
                        context.SetError("invalid_grant", "کاربر یافت نشد");
                        return;
                    }

                    switch (client.ApplicationType)
                    {
                        case ApplicationType.CustomerUserApp:
                        {
                            if (!appUser.CustomerIsActive)
                            {
                                context.SetError("invalid_grant", "کاربر غیرفعال می باشد");
                            }
                            break;
                        }
                        case ApplicationType.ShopUserApp:
                        {
                            if (!appUser.ShopIsActive)
                            {
                                context.SetError("invalid_grant", "کاربر غیرفعال می باشد");
                            }
                            break;
                        }
                    }

                    await VerifyPhoneNumber(appUser, context.Password, context.UserName);
                    var identityApp = new ClaimsIdentity(context.Options.AuthenticationType);
                    identityApp.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                    identityApp.AddClaim(new Claim("UserId", appUser.Id));
                    identityApp.AddClaim(new Claim("MobileNumber", appUser.PhoneNumber));
                    identityApp.AddClaim(new Claim("DeviceId", deviceId));
                    identityApp.AddClaim(new Claim("ShopIsActive", appUser.ShopIsActive.ToString()));
                    identityApp.AddClaim(new Claim("CustomerIsActive", appUser.CustomerIsActive.ToString()));
                    identityApp.AddClaim(new Claim("RegisterDate", appUser.RegisterDate.ToString()));
                    foreach (var role in rolesApp)
                    {
                        identityApp.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                    var appProps = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {
                            "as:client_id", context.ClientId ?? string.Empty
                        },
                        {
                            "userId", appUser.Id
                        },
                        {
                            "mobileNumber", appUser.PhoneNumber
                        }
                    });
                    var appTicket = new AuthenticationTicket(identityApp, appProps);
                    context.Validated(appTicket);
                }
            }
            catch (Exception e)
            {
                context.SetError("invalid_grant", e.Message);
            }
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
        private async Task VerifyPhoneNumber(ApplicationUser user, string code, string number)
        {
            var isVerifiedChangePhoneNumberToken =
                await _userManager.VerifyChangePhoneNumberTokenAsync(user.Id, code, number);
            if (!isVerifiedChangePhoneNumberToken)
                throw new CustomException("کد وارد شده اشتباه یا منقضی شده است");
            var result = await _userManager.ChangePhoneNumberAsync(user.Id, number, code);
            if (!result.Succeeded)
                throw new CustomException("خطای سرور رخ داده است لطفا دوباره تلاش نمایید");
        }
    }
}