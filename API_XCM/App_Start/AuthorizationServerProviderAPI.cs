using API_XCM.Code;
using API_XCM.Models;
using API_XCM.Models.UNITEX;
using API_XCM.Models.XCM.API;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace API_XCM.App_Start
{
    public class AuthorizationServerProviderAPI : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //   
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            if (AuthHelper.SignIn(context.UserName, context.Password))
            {
                var user = AuthHelper.GetUser(context.UserName);

                identity.AddClaim(new Claim(ClaimTypes.Role, AuthHelper.GetRoleName(user.RoleId)));
                identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
                identity.AddClaim(new Claim(ClaimTypes.Actor, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                return;
            }

            //var dbUsers = new UNITEXEntities();

            //if (context.UserName == "g.fusco@unitexpress.it")
            //{
            //    var user = dbUsers.USERS.FirstOrDefault(x => x.EMAIL == context.UserName);

            //    if (user != null && user.PASSWORD == Crypt.Encrypt(context.Password))
            //    {
            //        identity.AddClaim(new Claim(ClaimTypes.Role, "operator"));
            //        identity.AddClaim(new Claim("username", context.UserName));
            //        //identity.AddClaim(new Claim(ClaimTypes.Name, "Hi Admin"));
            //        context.Validated(identity);
            //    }
            //    else
            //    {
            //        context.SetError("invalid_grant", "Provided username and password is incorrect");
            //        return;
            //    }

            //}
            //else
            //{
            //    context.SetError("invalid_grant", "Provided username and password is incorrect");
            //    return;
            //}
        }
    }
}