using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace API.Policy
{
    public class TokenPolicyHandler : AuthorizationHandler<TokenPolicy>
    {
        private readonly IDistributedCache _cache;

        public TokenPolicyHandler(IDistributedCache cache)
        {
            this._cache = cache;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenPolicy requirement)
        {
            var userid = context.User.Claims.FirstOrDefault(x => x.Type == "userid")?.Value;
            if (userid == null)
            {
                throw new UnauthorizedAccessException();
                //throw new ExceptionManagementHelper()
            }

            var accessTokenKey = userid.ToString() + "_accessToken";
            var cacheToken = _cache.GetString(accessTokenKey);

            if (cacheToken == null)
            {
                throw new UnauthorizedAccessException();
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
