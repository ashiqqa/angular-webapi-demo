using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TestWebApi.DbContext;
using TestWebApi.Models;

namespace TestWebApi
{
    public class MyAuthorizationServerProvider:OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        //
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var user = UserDb.Login(context.UserName, context.Password);
            if(context.UserName==user.Email && context.Password == user.Password)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Id.ToString()));
                identity.AddClaim(new Claim("username", user.Email));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                context.Validated(identity);
            }
            //else if(context.UserName=="user" && context.Password == "321")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            //    identity.AddClaim(new Claim("username", "user"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "A R Shopon"));
            //    context.Validated(identity);
            //}
            else
            {
                context.SetError("invalid_grant", "User name or password is incorrect!");
            }
        }
    }
}