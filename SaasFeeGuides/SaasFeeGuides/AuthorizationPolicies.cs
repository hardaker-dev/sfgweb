using SaasFeeGuides.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SaasFeeGuides
{
    public static class AuthorizationPolicies
    {
        public static ILookup<string, (string type, string[] values)> Policies =
            new[]
        {
                ("Admin", (type:Constants.Strings.JwtClaimIdentifiers.Role, values: new []{Constants.Strings.JwtClaims.ApiAdminAccess })),
                ("Customer", (type:Constants.Strings.JwtClaimIdentifiers.Role, values: new []{Constants.Strings.JwtClaims.ApiAccess })),
                ("None", (type:Constants.Strings.JwtClaimIdentifiers.Role,new string[0] ))
        }.ToLookup(x => x.Item1, x => x.Item2);

        public static bool HasPolicy(this ClaimsPrincipal claimsPrincipal,string policyName)
        {
            var policy = Policies[policyName];
            if (!policy.Any())
                throw new InvalidOperationException($"policy with name:{policyName} does not exist");

            foreach(var (type, values) in policy)
            {
                foreach (var value in values)
                {
                    if (!claimsPrincipal.HasClaim(type, value))
                        return false;
                }
            }
            return true;
        }
              
    }
}
