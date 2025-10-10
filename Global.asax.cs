using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Security.Claims;
using System.Web.Helpers;
using Microsoft.Owin; // for GetOwinContext extension

namespace web_quanao
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Ensure antiforgery uses a stable unique claim type we control
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        // Patch: some previously issued auth cookies may lack NameIdentifier causing antiforgery to throw.
        // Add one derived from Name (email) or force sign-out so user re-authenticates via current logic which sets it.
        protected void Application_PostAuthenticateRequest()
        {
            var ctx = HttpContext.Current;
            var claimsIdentity = ctx?.User?.Identity as ClaimsIdentity;
            if (claimsIdentity != null && claimsIdentity.IsAuthenticated && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier) == null)
            {
                if (!string.IsNullOrEmpty(claimsIdentity.Name))
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, claimsIdentity.Name));
                }
                else
                {
                    // Cannot recover a stable identifier; sign out the malformed identity.
                    ctx.GetOwinContext().Authentication.SignOut();
                }
            }
        }
    }
}
