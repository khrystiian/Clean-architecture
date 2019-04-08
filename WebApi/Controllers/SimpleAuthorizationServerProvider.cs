using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Infrastructure.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace UI.Controllers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {


        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //   
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            try
            {
                using (var db = new TrainAppEntities())
                {
                    if (db != null)
                    {
                        var allPassengers = db.Passengers.ToList();
                        if (allPassengers != null)
                        {
                            var passenger = allPassengers.Where(u => u.Email == context.UserName && u.Password == context.Password).FirstOrDefault().Email;
                            if (!string.IsNullOrEmpty(passenger))
                            {
                                identity.AddClaim(new Claim(Convert.ToString(context.UserName), Convert.ToString(passenger)));

                                var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {
                                    "userdisplayname", context.UserName
                                },
                                {
                                     "role", "admin"
                                }
                             });

                                var ticket = new AuthenticationTicket(identity, props);
                                context.Validated(ticket);
                            }
                            else
                            {
                                context.SetError("invalid_grant", "Provided username and password is incorrect");
                                context.Rejected();
                            }
                        }
                    }
                    else
                    {
                        context.SetError("invalid_grant", "Provided username and password is incorrect");
                        context.Rejected();
                    }
                    return;
                }
            }
            catch (Exception e)
            {

                throw;
            }
            
        }
    }
}
