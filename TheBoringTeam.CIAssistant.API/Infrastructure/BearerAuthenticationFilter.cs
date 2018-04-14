using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheBoringTeam.CIAssistant.API.Infrastructure
{
    public class BearerAuthenticationAttribute : TypeFilterAttribute
    {
        public BearerAuthenticationAttribute() : base(typeof(BearerAuthenticationFilter))
        {
        }
    }

    public class BearerAuthenticationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == "AuthSchema" && c.Value == "Bearer");
            if (!hasClaim)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
