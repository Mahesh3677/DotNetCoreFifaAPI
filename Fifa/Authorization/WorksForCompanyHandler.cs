using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fifa.Authorization
{
    public class WorksForCompanyHandler : AuthorizationHandler<WorksForCompanyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorksForCompanyRequirement req)
        {
            var usserEmail = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            if(usserEmail.EndsWith(req.DomainName))
            {
                context.Succeed(req);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;

        }
    }
}
