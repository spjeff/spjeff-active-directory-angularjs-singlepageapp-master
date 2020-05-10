using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

// AAD Security  Group
using System.Security.Claims;
using System.Security.Principal;


namespace TodoSPA.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {

        // GET api/<controller>
        [HttpGet]
        public ClaimsPrincipal GetUser()
        {
            return ClaimsPrincipal.Current;
        }


        [HttpPost]
        public string GetData()
        {
            // HELLOWORLD Group
            bool HasGroup = TestControllerHelper.HasClaim(User, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "6bde6e40-ad9c-4f03-9ec3-ba3e2a0387d5");
            if (HasGroup)
            {
                return "REAL DATA HERE";
            } else
            {
                return "ACCESS DENIED, NOT MEMBER OF GROUP";
            }
        }

    }


    // from https://stackoverflow.com/questions/6096299/extension-methods-must-be-defined-in-a-non-generic-static-class
    public static class TestControllerHelper
    {
        // from https://stackoverflow.com/questions/35087307/efficiently-check-role-claim
        [HttpGet]
        public static bool HasClaim(this IPrincipal principal, string claimType, string claimValue, string issuer = null)
        {
            var ci = principal as ClaimsPrincipal;
            if (ci == null)
            {
                return false;
            }
            var claim = ci.Claims.FirstOrDefault(x => x.Type == claimType
                                                 && x.Value == claimValue
                                                 && (issuer == null || x.Issuer == issuer));
            return claim != null;
        }
    }
}